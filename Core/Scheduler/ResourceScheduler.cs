using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FeralTic.DX11
{
    public class ResourceScheduler
    {
        private RenderDevice device;

        private List<SchedulerThread> threads = new List<SchedulerThread>();

        private List<ISchedulerTask> tasklist = new List<ISchedulerTask>();
        private object m_lock = new object();

        private int thrcount;

        public int PendingTasks { get { return tasklist.Count; } }

        public ResourceScheduler(RenderDevice device, int threadcount = 1)
        {
            this.device = device;
            this.thrcount = threadcount;
        }

        public void Initialize()
        {
            for (int i = 0; i < this.thrcount; i++)
            {
                SchedulerThread thread = new SchedulerThread(this, device);
                this.threads.Add(thread);
                thread.Start();
            }
        }

        public void AddTask(ISchedulerTask task)
        {
            lock (m_lock)
            {
                this.tasklist.Add(task);
            }
        }

        public ISchedulerTask GetTask()
        {
            ISchedulerTask task = null;
            lock (m_lock)
            {
                if (tasklist.Count > 0)
                {
                    task = tasklist[0];
                    tasklist.RemoveAt(0);
                }
            }
            return task;
        }

        public void Dispose()
        {
            foreach (SchedulerThread thread in this.threads)
            {
                thread.Stop();
            }
        }
    }
}
