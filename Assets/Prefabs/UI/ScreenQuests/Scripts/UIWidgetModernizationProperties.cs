using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI
{
    [Serializable]
    public sealed class UIWidgetModernizationProperties : UIProperties
    {
        public RectTransform containerModernization;
        
        [SerializeField]
        private Button mButtonStartModernization;

        [SerializeField]
        private Button mButtonHint;
        
        [SerializeField]
        public AudioClip audioClipStartModernization;

        public Button buttonStartModernization
        {
            get { return this.mButtonStartModernization; }
        }
        
        public Button buttonHint
        {
            get { return this.mButtonHint; }
        }
    }
}