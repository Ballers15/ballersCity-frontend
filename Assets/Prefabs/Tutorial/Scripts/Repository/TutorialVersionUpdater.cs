using VavilichevGD.Architecture;

namespace SinSity.Repo
{
    public sealed class TutorialVersionUpdater : IVersionUpdater<TutorialStatistics>
    {
        public bool UpdateVersion(ref TutorialStatistics data)
        {
            if (data == null)
            {
                data = this.BuildStatistics();
                return true;
            }

            return false;
        }

        private TutorialStatistics BuildStatistics()
        {
            var tutorialStatistics = new TutorialStatistics
            {
                isCompleted = false,
                currentStageId = null,
                currentStageJson = null
            };
            return tutorialStatistics;
        }
    }
}