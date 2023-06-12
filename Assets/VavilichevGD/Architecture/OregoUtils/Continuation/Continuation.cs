using System;
using System.Collections;

namespace Orego.Util
{
    public sealed class Continuation
    {
        private bool isFinished;

        private Continuation()
        {
        }

        public void Continue()
        {
            if (this.isFinished)
            {
                throw new Exception($"Continuation {this.GetHashCode()} has already resumed!");
            }

            this.isFinished = true;
        }

        public static IEnumerator Suspend(Action<Continuation> action)
        {
            if (action == null)
            {
                throw new Exception("Action is null!");
            }

            var continuation = new Continuation();
            action.Invoke(continuation);
            while (!continuation.isFinished)
            {
                yield return null;
            }
        }
    }

    public sealed class Continuation<T>
    {
        private bool isFinished;

        private T result;

        private Continuation()
        {
        }

        public void Continue(T result)
        {
            if (this.isFinished)
            {
                throw new Exception($"Continuation {this.GetHashCode()} has already resumed!");
            }

            this.isFinished = true;
            this.result = result;
        }

        public static IEnumerator Suspend(Reference<T> result, Action<Continuation<T>> action)
        {
            if (action == null)
            {
                throw new Exception("Action is null!");
            }

            var continuation = new Continuation<T>();
            action.Invoke(continuation);
            while (!continuation.isFinished)
            {
                yield return null;
            }

            result.value = continuation.result;
        }
    }
}