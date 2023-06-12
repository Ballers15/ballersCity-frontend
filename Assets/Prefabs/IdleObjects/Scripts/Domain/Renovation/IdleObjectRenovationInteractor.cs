using System.Collections;
using System.Collections.Generic;
using SinSity.Core;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Domain
{
    public sealed class IdleObjectRenovationInteractor : Interactor,
        IModernizationAsyncListenerInteractor,
        IIdleObjectExternalMultipliersHolderInteractor
    {
        #region Const

        public const string RENOVATION_COEFFICIENT_ID = "renovation_coeffcient_id";

        private const int RENOVATION_COEFFICIENT = 2;

        #endregion

        private IdleObjectsInteractor idleObjectsInteractor;

        private ModernizationInteractor modernizationInteractor;

        private NotBuildedSlotsInteractor notBuildedSlotsInteractor;

        private CleanSlotsInteractor cleanSlotsInteractor;

        public override bool onCreateInstantly { get; } = true;


        public override void OnInitialized()
        {
            base.OnInitialized();
            this.modernizationInteractor = this.GetInteractor<ModernizationInteractor>();
            this.idleObjectsInteractor = this.GetInteractor<IdleObjectsInteractor>();
            this.notBuildedSlotsInteractor = this.GetInteractor<NotBuildedSlotsInteractor>();
            this.cleanSlotsInteractor = this.GetInteractor<CleanSlotsInteractor>();
        }


        #region OnRenovationAsync

        public IEnumerator OnStartModernizationAsync()
        {
            var idleObjects = this.idleObjectsInteractor.GetIdleObjects();
            foreach (var idleObject in idleObjects)
            {
                yield return RenovateIdleObject(idleObject);
            }

            yield return null;
        }

        private IEnumerator RenovateIdleObject(IdleObject idleObject)
        {
            var idleObjectState = idleObject.state;
            idleObject.ResetObject();
            //yield return this.cleanSlotsInteractor.OnIdleObjectReset(idleObject);
            yield return this.notBuildedSlotsInteractor.OnIdleObjectReset(idleObject);
            this.UpdateRenovationCoefficient(idleObjectState);
            idleObject.NotifyAboutStateChanged();
            yield return new WaitForEndOfFrame();
        }

        private void UpdateRenovationCoefficient(IdleObjectState idleObjectState)
        {
            var nextLevelCoefficient = modernizationInteractor.modernizationData.multiplier;
            var newRenovationCoefficient = new Coefficient(nextLevelCoefficient);
            var incomeConstMultiplicator = idleObjectState.incomeConstMultiplicator;
            incomeConstMultiplicator.SetCoefficient(RENOVATION_COEFFICIENT_ID, newRenovationCoefficient);
        }

        #endregion

        #region OnReady

        public Dictionary<string, Coefficient> GetStaticMultiplierMapBy(IdleObject idleObject)
        {
            var levelCoefficient = modernizationInteractor.modernizationData.multiplier;
            
            return new Dictionary<string, Coefficient>
            {
                {RENOVATION_COEFFICIENT_ID, new Coefficient(levelCoefficient)}
            };
        }

        public Dictionary<string, TimeCoefficient> GetTimeMultiplierMapBy(IdleObject idleObject)
        {
            return new Dictionary<string, TimeCoefficient>();
        }

        #endregion
    }
}