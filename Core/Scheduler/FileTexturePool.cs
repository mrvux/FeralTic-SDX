using FeralTic.DX11;
using FeralTic.DX11.Resources;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11
{
    public abstract class FileTexturePool<TElement,TResource, TTask> 
        where TElement : FileTexturePoolElement<TResource, TTask>
        where TResource : IDxResource
        where TTask : AbstractLoadTask<TResource>
    {
        private object m_lock = new object();

        private List<TElement> pool = new List<TElement>();

        public int ElementCount { get { return pool.Count; } }

        public event EventHandler OnElementLoaded;

        protected abstract TElement CreateElement(RenderDevice device, string path, bool mips, bool async = false);

        public bool TryGetFile(RenderDevice device, string path, bool domips,bool async, out TResource texture)
        {
            lock (m_lock)
            {
                foreach (TElement element in this.pool)
                {
                    if (element.DoMips == domips && element.FileName == path)
                    {
                        if (element.Status == eShedulerTaskStatus.Completed)
                        {
                            element.IncrementCounter();
                            texture = element.Resource;
                            return true;
                        }
                        else
                        {
                            element.IncrementCounter();
                            texture = default(TResource);
                            return false;
                        }
                    }
                }
            }

            TElement elem = this.CreateElement(device, path, domips, async);
            elem.OnLoadComplete += (e) =>
                {                      
                    if (this.OnElementLoaded != null) { this.OnElementLoaded(this, new EventArgs()); }
                };
            
            lock (m_lock)
            {
                if (elem.Status != eShedulerTaskStatus.Error)
                {
                    this.pool.Add(elem);
                    if (elem.Status == eShedulerTaskStatus.Completed)
                    {
                        texture = elem.Resource;
                        return true;
                    }
                    else
                    {
                        texture = default(TResource);
                        return false;
                    }

                }
                else
                {
                    texture = default(TResource);
                    return false;
                }
            }
        }



        public void DecrementAll()
        {
            lock (m_lock)
            {
                foreach (TElement e in this.pool)
                {
                    e.DecrementCounter();
                }
            }
        }

        public void Flush()
        {
            lock (m_lock)
            {
                List<TElement> newlist = new List<TElement>();
                foreach (TElement e in this.pool)
                {
                    if (e.RefCount < 0 || (e.Status == eShedulerTaskStatus.Error || e.Status == eShedulerTaskStatus.Aborted))
                    {
                        if (e.Status == eShedulerTaskStatus.Queued)
                        {
                            e.Abort();
                        }
                        else if (e.Resource != null) 
                        { 
                            e.Resource.Dispose(); 
                        }
                        
                    }
                    else
                    {
                        newlist.Add(e);
                    }
                }
                this.pool = newlist;
            }
        }

        public void Dispose()
        {
            foreach (TElement elem in this.pool)
            {
                elem.Resource.Dispose();
            }
            this.pool.Clear();
        }
    }
}
