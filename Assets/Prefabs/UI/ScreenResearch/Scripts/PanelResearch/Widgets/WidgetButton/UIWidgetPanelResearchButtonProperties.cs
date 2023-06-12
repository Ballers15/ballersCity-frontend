using System;
using Orego.Util;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VavilichevGD.Audio;

namespace SinSity.UI
{
    [Serializable]
    public class UIWidgetPanelResearchButtonProperties : UIWidgetPanelResearchProperties 
    {
        [SerializeField] private Button btn;
        [SerializeField] private AudioClip sfxError;

        public override void AddListener(UnityAction callback)
        {
            this.btn.onClick.AddListener(callback);
        }

        public override void RemoveListener(UnityAction callback)
        {
            this.btn.onClick.RemoveListener(callback);
        }

        public override void RemoveAllListeners()
        {
            this.btn.RemoveListeners();
        }

        public void PlayError() {
            SFX.PlaySFX(sfxError);
        }
    }
}