using System;
using UnityEngine;

namespace SinSity.Domain
{
    [CreateAssetMenu(
        menuName = "Domain/MiniQuestConfig/New",
        fileName = "MiniQuestConfig"
    )]
    public sealed class MiniQuestConfig : ScriptableObject
    {
        [SerializeField]
        public int maxActiveQuestCount;
        [SerializeField]
        private int m_resetQuestsHour = 3;
        public int resetQuestsHour => this.m_resetQuestsHour;
    }
}