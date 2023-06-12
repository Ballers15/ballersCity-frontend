using SinSity.Core;
using VavilichevGD.Audio;

namespace SinSity.UI
{
    public class UIPanelNotificationLevelRaise : UIPanelNotification<UIPanelNotificationLevelRaiseProperties>
    {
        public virtual void Setup(IdleObject idleObject) {
            properties.imgIdleObjectIcon.sprite = idleObject.info.spriteIcon;
            SFX.PlaySFX(this.properties.audioClipShow);
        }
    }
}