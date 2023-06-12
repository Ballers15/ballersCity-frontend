using System;
using UnityEngine;

namespace SinSity.Core
{
    [Serializable]
    public sealed class UITutorialControllerProperties : UIProperties
    {
        [SerializeField]
        public UITutorialStage[] uiStagePrefs;
    }
}