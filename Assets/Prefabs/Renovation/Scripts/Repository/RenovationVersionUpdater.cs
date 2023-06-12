using VavilichevGD.Architecture;

namespace SinSity.Repo
{
    public sealed class RenovationVersionUpdater : IVersionUpdater<RenovationStatistics>
    {
        #region Const

        private const long START_LEVEL = 1;

        private const long PASSED_QUEST_COUNT = 0;

        private const long TARGET_QUEST_COUNT = 3;

        #endregion

        public bool UpdateVersion(ref RenovationStatistics data)
        {
            if (data == null)
            {
                data = this.BuildStatistics();
                return true;
            }

            return false;
        }

        private RenovationStatistics BuildStatistics()
        {
            return new RenovationStatistics
            {
                level = START_LEVEL,
                passedQuestCount = PASSED_QUEST_COUNT,
                targetQuestCount = TARGET_QUEST_COUNT
            };
        }
    }
}