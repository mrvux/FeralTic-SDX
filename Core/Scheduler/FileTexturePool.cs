using FeralTic.DX11;
using FeralTic.DX11.Resources;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11
{

    public class FileTexturePool
    {
        private object m_lock = new object();

        private List<FileTexturePoolElement> pool = new List<FileTexturePoolElement>();

        public event EventHandler OnElementLoaded;

        public bool TryGetFile(RenderDevice device, string path, bool domips,bool async, out IDxTexture2D texture)
        {
            lock (m_lock)
            {
                foreach (FileTexturePoolElement element in this.pool)
                {
                    if (element.DoMips == domips && element.FileName == path)
                    {
                        if (element.Status == eShedulerTaskStatus.Completed)
                        {
                            element.IncrementCounter();
                            texture = element.Texture;
                            return true;
                        }
                        else
                        {
                            element.IncrementCounter();
                            texture = null;
                            return false;
                        }
                    }
                }
            }


            FileTexturePoolElement elem = new FileTexturePoolElement(device, path, domips, async);
            elem.OnLoadComplete += elem_OnLoadComplete;
            
            lock (m_lock)
            {
                if (elem.Status != eShedulerTaskStatus.Error)
                {
                    this.pool.Add(elem);
                    if (elem.Status == eShedulerTaskStatus.Completed)
                    {
                        texture = elem.Texture;
                        return true;
                    }
                    else
                    {
                        texture = null;
                        return false;
                    }

                }
                else
                {
                    texture = null;
                    return false;
                }
            }

        }

        void elem_OnLoadComplete(FileTexturePoolElement element)
        {
            if (this.OnElementLoaded != null)
            {
                this.OnElementLoaded(this, new EventArgs());
            }
        }

        public void DecrementAll()
        {
            lock (m_lock)
            {
                foreach (FileTexturePoolElement e in this.pool)
                {
                    e.DecrementCounter();
                }
            }
        }

        public void Flush()
        {
            lock (m_lock)
            {
                List<FileTexturePoolElement> newlist = new List<FileTexturePoolElement>();
                foreach (FileTexturePoolElement e in this.pool)
                {
                    if (e.RefCount < 0 || (e.Status == eShedulerTaskStatus.Error || e.Status == eShedulerTaskStatus.Aborted))
                    {
                        if (e.Texture != null) { e.Texture.Dispose(); }
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
            foreach (FileTexturePoolElement elem in this.pool)
            {
                elem.Texture.Dispose();
            }
            this.pool.Clear();
        }
    }
}
