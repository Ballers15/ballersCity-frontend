using System;
using SinSity.Core;
using UnityEngine;

namespace SinSity.UI
{
    [Serializable]
    public sealed class UIScreenGemTreeProperties : UIProperties
    {
        public UIGemTree gemTree;
        public UIPanelGemTreeLevel panelGemTreeLevel;
        public UIButtonGemTreeUpgrade buttonGemTreeUpgrade;
    }
}