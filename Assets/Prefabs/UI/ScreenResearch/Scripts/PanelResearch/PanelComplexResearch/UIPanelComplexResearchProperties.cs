using System;
using UnityEngine;

namespace SinSity.UI
{
    [Serializable]
    public sealed class UIPanelComplexResearchProperties : UIPanelResearchProperties
    {
        [SerializeField]
        private UIWidgetComplexResearchReward[] m_widgetRewards;

        public UIWidgetComplexResearchReward[] widgetRewards
        {
            get { return this.m_widgetRewards; }
        }

        public Vector3 GetIconPosition(int index) {
            return m_widgetRewards[index].GetIconPosition();
        }
    }
}