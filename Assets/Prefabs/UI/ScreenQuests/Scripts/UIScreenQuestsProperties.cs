using System;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI
{
    [Serializable]
    public class UIScreenQuestsProperties : UIProperties {
        public RectTransform containerQuests;

        [SerializeField]
        private Button m_buttonMiniQuestsResetForAd;

        [SerializeField]
        public AudioClip audioClipMiniQuestsResetForAd;
    }
}