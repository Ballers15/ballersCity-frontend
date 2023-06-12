using UnityEngine;

namespace SinSity.Core {
    [CreateAssetMenu(fileName = "ModernizationConfig", menuName = "Core/Modernization/New ModernizationConfig")]
    public class ModernizationConfig : ScriptableObject {
        [SerializeField] private int m_scoresForObjectBuilding = 1;
        [SerializeField] private int m_scoresForObjectUpgradeStage = 1;
        [SerializeField] private int m_scoresForTimePeriod = 1;
        [SerializeField] private int m_timePeriodForScoresReward = 30;
        [SerializeField] private int m_idleObjectsCountToUnlock = 5;
        [SerializeField] private int m_updateIdleToLevelToUnlock = 10;

        public int scoresForObjectBuilding => this.m_scoresForObjectBuilding;
        public int scoresForObjectUpgradeStage => this.m_scoresForObjectUpgradeStage;
        public int scoresForTimePeriod => this.m_scoresForTimePeriod;
        public int timePeriodForScoresReward => this.m_timePeriodForScoresReward;
        public int idleObjectsCountToUnlock => this.m_idleObjectsCountToUnlock;
        public int updateIdleToLevelToUnlock => this.m_updateIdleToLevelToUnlock;
    }
}