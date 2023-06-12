using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SinSity.Core
{
    [CreateAssetMenu(
        fileName = "IdleObjectUpgradeForADConfig",
        menuName = "Game/IdleObject/New IdleObjectUpgradeForADConfig"
    )]
    public class IdleObjectUpgradeForADConfig : ScriptableObject
    {
        [Tooltip("Idle object from upgrade for AD begins")]
        [SerializeField] private int m_minIdleObjectNumber = 5;
        [Tooltip("How many last builded objects can be upgrade for AD")]
        [SerializeField] private int m_lastBuildedObjectsCount = 3;
        [Tooltip("Income period affects how many levels can be upgraded")]
        [SerializeField] private int m_incomePeriodSeconds = 30;
        [SerializeField] private float m_upgradeDiscount = 0.2f;


        public int minIdleObjectNumber => this.m_minIdleObjectNumber;
        public int lastBuildedObjectsCount => this.m_lastBuildedObjectsCount;
        public int incomePeriodSeconds => this.m_incomePeriodSeconds;
        public float upgradeDiscount => this.m_upgradeDiscount;
    }
}