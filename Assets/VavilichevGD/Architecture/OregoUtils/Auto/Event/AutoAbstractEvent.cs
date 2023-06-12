using System.Collections.Generic;

namespace Orego.Util
{
    public abstract class AutoAbstractEvent<A> : IAutoEvent
    {
        protected readonly List<A> listeners;

        protected AutoAbstractEvent()
        {
            this.listeners = new List<A>();
        }

        public virtual void AddListener(A action)
        {
            if (action != null)
            {
                this.listeners.Add(action);
            }
        }

        public virtual void RemoveListener(A action)
        {
            if (action != null)
            {
                this.listeners.Remove(action);
            }
        }

        public virtual void Dispose()
        {
            this.listeners.Clear();
        }
    }
}