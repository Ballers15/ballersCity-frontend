using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Domain
{
    [CreateAssetMenu(
        fileName = "ResearchLevelHandler",
        menuName = "Domain/Research/New ResearchLevelHandler"
    )]
    public sealed class ResearchLevelHandler : ScriptableProfileLevelHandler
    {
        #region OnProfileLevelRisen

        public override void OnProfileLevelRisen()
        {
            var researchInteractor = Game.GetInteractor<ResearchStateInteractor>();
            if (researchInteractor.isResearchUnlocked)
            {
                return;
            }
#if DEBUG
            Debug.Log("<color=green>UNLOCK RESEARCH</color>");
#endif
            researchInteractor.UnlockResearch(this);
            this.ReceiveReward();
        }
        #endregion
    }
}