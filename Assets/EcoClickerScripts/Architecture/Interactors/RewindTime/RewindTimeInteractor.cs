using System;
using System.Collections;
using System.Collections.Generic;
using VavilichevGD.Architecture;

namespace SinSity.Domain {
    public sealed class RewindTimeInteractor : Interactor {
        #region Events

        public event Action<RewindTimeIntent> OnRewindTimeFinishedEvent;

        #endregion

        public override bool onCreateInstantly { get; } = true;


        private IEnumerable<IRewindTimeAsyncListenerInteractor> rewindTimeAsyncListeners;

        public override void OnInitialized() {
            base.OnInitialized();
            this.rewindTimeAsyncListeners = this.GetInteractors<IRewindTimeAsyncListenerInteractor>();
        }

        public IEnumerator RewindTime(RewindTimeIntent intent) {
            foreach (var listener in this.rewindTimeAsyncListeners) {
                yield return listener.OnRewindTimeAsync(intent);
            }

            this.OnRewindTimeFinishedEvent?.Invoke(intent);
        }
    }
}