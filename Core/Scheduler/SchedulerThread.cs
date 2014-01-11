using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;


namespace FeralTic.DX11
{
    public class SchedulerThread
    {
        private Thread thr;

        private ResourceScheduler scheduler;
        private RenderDevice device;

        private bool running = false;

        public SchedulerThread(ResourceScheduler scheduler, RenderDevice device)
        {
            this.scheduler = scheduler;
            this.device = device;
        }

        public void Start()
        {
            if (this.running) { return; }
            this.running = true;

            this.thr = new Thread(new ThreadStart(this.Run));
            this.thr.Priority = ThreadPriority.BelowNormal;
            this.thr.Start();
        }

        public void Stop()
        {
            this.running = false;
        }

        private void Run()
        {
            while (this.running)
            {
                ISchedulerTask task = this.scheduler.GetTask();

                if (task != null)
                {
                    task.Process();
                }
                Thread.Sleep(10);
            }
        }

    }
}
