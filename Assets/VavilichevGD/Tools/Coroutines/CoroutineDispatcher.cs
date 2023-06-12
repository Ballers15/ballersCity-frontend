using System.Collections;
using UnityEngine;

namespace VavilichevGD.Tools
{
    public sealed class CoroutineDispatcher {

        public bool isWorking => routine != null;

        private readonly MonoBehaviour mono;
        
        private Coroutine routine;

        public CoroutineDispatcher(MonoBehaviour mono) {
            this.mono = mono;
        }

        public Coroutine Start(IEnumerator enumerator) {
            if (isWorking)
                return null;

            this.routine = mono.StartCoroutine(enumerator);
            return routine;
        }

        public Coroutine StartForce(IEnumerator enumerator) {
            Stop();
            return Start(enumerator);
        }
        
        public void Stop() {
            if (!isWorking)
                return;

            mono.StopCoroutine(routine);
            routine = null;
        }
    }
}