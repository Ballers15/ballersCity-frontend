using System;
using UnityEngine;

namespace SinSity.UI
{
    [Serializable]
    public sealed class UIOfflineIncomeControllerProperties : UIProperties
    {
        [SerializeField]
        public UIPopupOfflineCollected popup;

        [SerializeField]
        public float offlineSecondsTrashold = 120.0f;

        [SerializeField] 
        public int x3ForGemsPrice = 100;
    }
}