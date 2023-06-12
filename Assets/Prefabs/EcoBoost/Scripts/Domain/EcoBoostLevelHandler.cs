using System;
using System.Collections;
using SinSity.Repo;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Domain
{
    [CreateAssetMenu(
        fileName = "EcoBoostLevelHandler",
        menuName = "Domain/Ecoboost/New EcoBoostLevelHandler"
    )]
    public sealed class EcoBoostLevelHandler : ScriptableProfileLevelHandler
    {
        #region OnProfileLevelRisen

        public override void OnProfileLevelRisen()
        {
            var ecoBoostInteractor = Game.GetInteractor<EcoBoostInteractor>();
            if (ecoBoostInteractor.isEcoboostUnlocked)
            {
                return;
            }

            ecoBoostInteractor.UnlockEcoBoost(this);
            this.ReceiveReward();
        }
 
        #endregion
    }
}