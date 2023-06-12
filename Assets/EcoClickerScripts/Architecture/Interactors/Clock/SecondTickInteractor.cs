using System.Collections.Generic;
using VavilichevGD.Architecture;

namespace SinSity.Domain {
    public sealed class SecondTickInteractor : Interactor, IUpdateListenerInteractor {
        
        #region Const

        public const float SECOND_FRAME = 1.0f;

        #endregion

        public override bool onCreateInstantly { get; } = true;


        private float frameTime;

        private IEnumerable<IListener> secondListeners;


        public override void OnInitialized() {
            base.OnInitialized();
            this.secondListeners = this.GetInteractors<IListener>();
        }

        public void OnUpdate(float unscaledDeltaTime) {
            this.frameTime += unscaledDeltaTime;
            while (this.frameTime >= SECOND_FRAME) {
                this.frameTime -= SECOND_FRAME;
                foreach (var secondListener in this.secondListeners) {
                    secondListener.OnSecondTick();
                }
            }
        }

        public interface IListener : IInteractor {
            void OnSecondTick();
        }
    }
}