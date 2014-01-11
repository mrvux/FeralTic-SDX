using FeralTic.DX11.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeralTic.DX11
{
    public delegate void FileTextureLoadedDelegate(FileTexturePoolElement element);

    public class FileTexturePoolElement
    {
        public event FileTextureLoadedDelegate OnLoadComplete;

        private FileTexture2dLoadTask m_task;

        public FileTexturePoolElement(RenderDevice device, string path, bool mips, bool async = false)
        {
            this.FileName = path;
            this.DoMips = mips;
            this.refcount = 1;


            if (async)
            {
                this.m_task = new FileTexture2dLoadTask(device, path, mips);
                m_task.StatusChanged += task_StatusChanged;

                device.ResourceScheduler.AddTask(m_task);
            }
            else
            {
                try
                {
                    this.Texture = TextureLoader.LoadFromFile(device, path, mips);
                    this.Status = eShedulerTaskStatus.Completed;
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
                this.Texture = m_task.Resource;

                if (this.OnLoadComplete != null)
                {
                    this.OnLoadComplete(this);
                }
            }
        }

        public eShedulerTaskStatus Status { get; private set; }

        public IDxTexture2D Texture { get; private set; }
        public string FileName { get; private set; }
        public bool DoMips { get; private set; }

        private int refcount = 0;

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
