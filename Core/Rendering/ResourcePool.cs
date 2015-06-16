using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.Direct3D11;

namespace FeralTic.DX11
{
    public class ResourcePoolEntry<T> where T : class , IDisposable
    {
        private bool islocked;
        public bool IsLocked { get { return this.islocked; } }

        public T Element { get; private set; }

        public ResourcePoolEntry(T element)
        {
            this.Element = element;
            this.Lock();
        }

        public void Lock()
        {
            this.islocked = true;
        }

        public void UnLock()
        {
            this.islocked = false;
        }

        public static implicit operator T(ResourcePoolEntry<T> entry)
        {
            return entry.Element;
        }
    }


    public class ResourcePool<T> : IDisposable where T : class, IDisposable
    {
        protected List<ResourcePoolEntry<T>> pool = new List<ResourcePoolEntry<T>>();
        protected DxDevice device;

        protected object lockObject = new object();

        public ResourcePool(DxDevice device)
        {
            this.device = device;
        }

        internal IEnumerable<ResourcePoolEntry<T>> AllResources
        {
            get { return this.pool; }
        }

        public int Count
        {
            get { return this.pool.Count; }
        }

        public void UnlockAll()
        {
            lock (lockObject)
            {
                foreach (ResourcePoolEntry<T> entry in this.pool)
                {
                    if (entry.IsLocked) { entry.UnLock(); }
                }
            }
        }

        public bool UnLock(T resource)
        {
            lock (lockObject)
            {
                foreach (ResourcePoolEntry<T> entry in this.pool)
                {
                    if (entry.Element == resource && entry.IsLocked)
                    {
                        entry.UnLock();
                        return true;
                    }
                }
            }
            return false;
        }

        public void ClearUnlocked()
        {
            lock (lockObject)
            {
                List<ResourcePoolEntry<T>> todelete = new List<ResourcePoolEntry<T>>();
                foreach (ResourcePoolEntry<T> entry in this.pool)
                {
                    if (!entry.IsLocked) { todelete.Add(entry); }
                }

                foreach (ResourcePoolEntry<T> entry in todelete)
                {
                    this.pool.Remove(entry);
                    entry.Element.Dispose();
                }
            }
            }


        public void Dispose()
        {
            lock (lockObject)
            {
                foreach (ResourcePoolEntry<T> entry in this.pool)
                {
                    entry.Element.Dispose();
                }
                this.pool.Clear();
            }
        }
    }
}
