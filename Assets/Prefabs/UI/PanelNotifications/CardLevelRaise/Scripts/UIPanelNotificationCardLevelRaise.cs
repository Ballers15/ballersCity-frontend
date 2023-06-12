using System;
using SinSity.Core;
using VavilichevGD.Architecture;
using VavilichevGD.Audio;

namespace SinSity.UI {
    public class UIPanelNotificationCardLevelRaise : UIPanelNotification<UIPanelNotificationCardLevelRaiseProperties> {
        
        private IdleObjectsInteractor ioInteractor;
        
        public override void Initialize() {
            base.Initialize();

            Game.OnGameInitialized += OnGameInitialized;
        }

        private void OnGameInitialized(Game game) {
            Game.OnGameInitialized -= OnGameInitialized;
            ioInteractor = Game.GetInteractor<IdleObjectsInteractor>();
        }

        public void Setup(ICard cardObject) {
            /*int speedValue = (int)Math.Pow(2, cardObject.currentLevelIndex);
            this.properties.SetSpeedValue(speedValue);

            string idleObjectId = cardObject.info.targetIdleObjectId;
            IdleObject io = ioInteractor.GetIdleObject(idleObjectId);
            this.properties.SetIcon(io.info.spriteIcon);
            
            SFX.PlaySFX(this.properties.audioClipShow);*/
        }
    }
}