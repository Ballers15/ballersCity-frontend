using System;
using UnityEngine;

namespace SinSity.UI
{
    [Serializable]
    public class UIScreenResearchProperties : UIProperties
    {
        [SerializeField]
        public UIPanelResearch[] panels;
    }
}