using System;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI {
    [Serializable]
    public class UIPopupModernizationInfoProperties : UIProperties
    {
        [SerializeField]
        private Text m_textFieldCaption;
        [SerializeField]
        private Text m_textFieldDescription;
        

        public Text textFieldCaption => m_textFieldCaption;
        public Text textFieldDescription => m_textFieldDescription;

        [Serializable]
        public class Pair {
            public Content content;
            public PopupType type;
        }
        
        [Serializable]
        public struct Content {
            public string captionTranslationKey;
            public string descrTranslationKey;
        }
        
        [Serializable]
        public enum PopupType
        {
            Score,
            Modernization,
        };
        
        [SerializeField] public Button[] buttonsClose;
        [SerializeField] public Pair[] popupContent;
    }
}