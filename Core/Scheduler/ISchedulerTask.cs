using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FeralTic.DX11.Resources;

namespace FeralTic.DX11
{
    public enum eShedulerTaskStatus { Queued, Loading, Completed,Error,Aborted }

    public delegate void TaskStatusChangedDelegate(ISchedulerTask task);

    public interface ISchedulerTask
    {
        RenderDevice Device { get; }
        eShedulerTaskStatus Status { get; }
        bool IsDirty { get; }

        event TaskStatusChangedDelegate StatusChanged;

        void Process();
        void Dispose();
        void MarkForAbort();   
    }

    public abstract class AbstractLoadTask<T> : ISchedulerTask where T : IDxResource
    {
        public T Resource { get; protected set; }

        public RenderDevice Device { get; protected set; }

        public bool IsDirty { get; protected set; }

        public AbstractLoadTask(RenderDevice device)
        {
            this.Device = device;
        }

        private eShedulerTaskStatus status;

        public eShedulerTaskStatus Status
        {
            get { return this.status; }
        }

        public event TaskStatusChangedDelegate StatusChanged;

        private void SetStatus(eShedulerTaskStatus status)
        {
            this.status = status;

            if (this.StatusChanged != null)
            {
                this.StatusChanged(this);
            }
        }

        public void Process()
        {
            if (this.IsDirty) { this.SetStatus(eShedulerTaskStatus.Aborted); return; }

            this.SetStatus(eShedulerTaskStatus.Loading);

            try
            {
                this.DoProcess();

                if (this.IsDirty)
                {
                    this.Dispose();
                    this.SetStatus(eShedulerTaskStatus.Aborted);
                }
                else
                {
                    this.SetStatus(eShedulerTaskStatus.Completed);
                }
            }
            catch
            {
                this.SetStatus(eShedulerTaskStatus.Error);
            }
        }

        protected abstract void DoProcess();
        

        public void MarkForAbort()
        {
            if (this.status != eShedulerTaskStatus.Completed)
            {
                this.IsDirty = true;
            }
        }

        public abstract void Dispose();
    }



}
