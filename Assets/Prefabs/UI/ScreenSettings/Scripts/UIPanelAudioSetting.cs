using UnityEngine.UI;
using VavilichevGD.UI;

namespace SinSity.UI {
	public abstract class UIPanelAudioSetting : UIPanel<UIPanelAudioSettingProperties> {

		protected virtual void OnEnable() {
			properties.btnSetting.onClick.AddListener(OnBtnClick);
			UpdateState();
		}

		protected abstract void OnBtnClick();
		protected abstract void UpdateState();

		protected virtual void OnDisable() {
			properties.btnSetting.onClick.RemoveListener(OnBtnClick);
		}

		protected virtual void Reset() {
			if (!properties.btnSetting)
				properties.btnSetting = GetComponentInChildren<Button>();
		}

		protected void SwitchSpriteToEnabled() {
			properties.markerDisabledGO.SetActive(true);
		}

		protected void SwitchSpriteToDisabled() {
			properties.markerDisabledGO.SetActive(false);
		}
	}
}