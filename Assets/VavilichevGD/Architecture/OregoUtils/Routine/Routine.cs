using System;
using System.Collections;
using UnityEngine;

namespace Orego.Util
{
    public sealed class Routine
    {
        #region Event

        public event Action OnFinishedEvent;

        public event Action OnCanceledEvent;

        #endregion

        private readonly MonoBehaviour dispatcher;

        private Coroutine routine;

        public Routine(MonoBehaviour dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        #region Start

        public void Start(IEnumerator enumerator)
        {
            if (this.IsWorking())
            {
                return;
            }

            var wrappedEnumerator = this.GetWrappedEnumerator(enumerator);
            var routine = this.dispatcher.StartCoroutine(wrappedEnumerator);
            this.routine = routine;
        }

        #endregion

        #region Join
        
        /**
         * Experimental.
         */

        public IEnumerator Join(IEnumerator enumerator)
        {
            if (this.IsWorking())
            {
                yield break;
            }

            var wrappedEnumerator = this.GetWrappedEnumerator(enumerator);
            var routine = this.dispatcher.StartCoroutine(wrappedEnumerator);
            this.routine = routine;
            yield return routine;
        }
        
        #endregion

        private IEnumerator GetWrappedEnumerator(IEnumerator enumerator)
        {
            yield return enumerator;
            this.routine = null;
            this.OnFinishedEvent?.Invoke();
        }

        #region Cancel

        public void Cancel()
        {
            if (!this.IsWorking())
            {
                return;
            }

            this.dispatcher.StopCoroutine(this.routine);
            this.routine = null;
            this.OnCanceledEvent?.Invoke();
        }

        #endregion

        #region IsWorking

        public bool IsWorking()
        {
            return this.routine != null;
        }

        #endregion
    }
}