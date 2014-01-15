using FeralTic.DX11.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11
{
    public delegate void FileTextureLoadedDelegate<TResource, TTask>(FileTexturePoolElement<TResource, TTask> element)
        where TResource : IDxResource
        where TTask : AbstractLoadTask<TResource>;


    public abstract class FileTexturePoolElement<TResource, TTask>
        where TResource : IDxResource
        where TTask : AbstractLoadTask<TResource>
    {
        public eShedulerTaskStatus Status { get; private set; }

        public TResource Resource { get; private set; }
        public string FileName { get; private set; }
        public bool DoMips { get; private set; }

        private int refcount = 0;

        public event FileTextureLoadedDelegate<TResource, TTask> OnLoadComplete;

        private TTask m_task;

        protected abstract TTask CreateTask(RenderDevice device);

        public FileTexturePoolElement(RenderDevice device, string path, bool mips, bool async = false)
        {
            this.FileName = path;
            this.DoMips = mips;
            this.refcount = 1;


            if (async)
            {
                this.m_task = this.CreateTask(device);
                m_task.StatusChanged += task_StatusChanged;

                device.ResourceScheduler.AddTask(m_task);
            }
            else
            {
                try
                {
                    this.m_task = this.CreateTask(device);
                    this.m_task.Process();

                    if (this.m_task.Status == eShedulerTaskStatus.Completed)
                    {
                        this.Resource = this.m_task.Resource;
                    }
                    this.Status = this.m_task.Status;
                }
                catch
                {
                    this.Status = eShedulerTaskStatus.Error;
                }
            }
        }

        private void task_StatusChanged(ISchedulerTask task)
        {
            this.Status = task.Status;
            if (this.Status == eShedulerTaskStatus.Completed || this.Status == eShedulerTaskStatus.Error)
            {
                this.Resource = m_task.Resource;

                if (this.OnLoadComplete != null)
                {
                    this.OnLoadComplete(this);
                }
            }
        }

        public void IncrementCounter()
        {
            this.refcount++;
        }

        public void DecrementCounter()
        {
            this.refcount--;
        }

        public int RefCount
        {
            get { return this.refcount; }
        }
    }

}
