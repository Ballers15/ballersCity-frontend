using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Domain
{
    [CreateAssetMenu(
        fileName = "GemTreeLevelHandler",
        menuName = "Domain/GemTree/New GemTreeLevelHandler"
    )]
    public sealed class GemTreeLevelHandler : ScriptableProfileLevelHandler
    {
        #region OnProfileLevelRisen

        public override void OnProfileLevelRisen()
        {
            var gemTreeInteractor = Game.GetInteractor<GemTreeStateInteractor>();
            if (gemTreeInteractor.isTreeUnlocked)
            {
                return;
            }
#if DEBUG
            Debug.Log("<color=green>UNLOCK GEM TREE</color>");
#endif
            //TODO: UNCOMMENT WHEN GEM TREE HAS UNDER DEVELOPMENT:
            gemTreeInteractor.UnlockTree(this);
            this.ReceiveReward();
        }

        #endregion
    }
}