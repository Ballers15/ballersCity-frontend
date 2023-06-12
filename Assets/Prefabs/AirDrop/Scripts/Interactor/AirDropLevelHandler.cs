using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Domain
{
    [CreateAssetMenu(
        fileName = "AirDropLevelHandler",
        menuName = "Domain/AirDrop/New AirDropLevelHandler"
    )]
    public sealed class AirDropLevelHandler : ScriptableProfileLevelHandler
    {
        public override void OnStart()
        {
            base.OnStart();
            if (this.profileLevelInteractor.currentLevel < this.reachLevel)
            {
                return;
            }

            this.UnlockAirDrop();
        }

        #region OnProfileLevelRisen

        public override void OnProfileLevelRisen()
        {
            this.UnlockAirDrop();
            this.ReceiveReward();
        }

        private void UnlockAirDrop()
        {
            var airDropInteractor = Game.GetInteractor<AirDropInteractor>();
            airDropInteractor.UnlockAirDrop(this);
        }

        #endregion
    }
}