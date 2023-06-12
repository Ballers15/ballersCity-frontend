using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI
{
    [Serializable]
    public class UIPopupUpgradeForADProperties : UIProperties
    {
        [SerializeField] public Image idleObjectImage;
        [SerializeField] public Text textDescription;
        [SerializeField] public Button getButton;
        [SerializeField] public Button btnClose;
        [SerializeField] public AudioClip audioClipCloseClick;
    }
}
