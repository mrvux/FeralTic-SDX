using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeralTic.DX11
{
    public abstract class RenderStates<T> : IDisposable where T : IDisposable
    {
        private Dictionary<string, T> states = new Dictionary<string, T>();

        private List<string> statekeys = new List<string>();

        protected RenderStates() { }

        protected abstract void Initialize();
        public abstract string EnumName { get ; }

        protected void AddState(string key, T state)
        {
            this.statekeys.Add(key);
            this.states.Add(key, state);
        }

        public T GetState(string key)
        {
            return this.states[key];
        }

        public string[] StateKeys
        {
            get { return this.statekeys.ToArray(); }
        }

        public virtual void Dispose()
        {
            foreach (T state in this.states.Values)
            {
                state.Dispose();
            }
        }
    }
}
