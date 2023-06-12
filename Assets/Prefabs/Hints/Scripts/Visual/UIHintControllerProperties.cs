using System;
using UnityEngine;

namespace SinSity.Core
{
    [Serializable]
    public sealed class UIHintControllerProperties : UIProperties
    {
        [SerializeField]
        public UIHintInpector[] uiHintInpectorPrefs;
    }
}