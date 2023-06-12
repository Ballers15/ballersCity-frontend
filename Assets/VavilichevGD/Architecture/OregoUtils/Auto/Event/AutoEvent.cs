using System;
using System.Linq;

namespace Orego.Util
{
    public class AutoEvent<T> : AutoAbstractEvent<Action<T>>
    {
        public virtual void Invoke(T value)
        {
            var i = 0;
            while (i < this.listeners.Count)
            {
                var listener = this.listeners[i++];
                listener.Invoke(value);
            }
        }
    }

    public class AutoEvent : AutoAbstractEvent<Action>
    {
        public virtual void Invoke()
        {
            var i = 0;
            while (i < this.listeners.Count)
            {
                var listener = this.listeners[i++];
                listener.Invoke();
            }
        }
    }
}