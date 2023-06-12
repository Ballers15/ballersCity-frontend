using System.Collections.Generic;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Domain
{
    public sealed class SaveGameInteractor : Interactor, IUpdateListenerInteractor
    {
        #region Const

        private const float AUTO_SAVE_EVERY_SEC = 30.0f;

        #endregion

        public override bool onCreateInstantly { get; } = false;

        private float autoSaveTimerValue;

        private IEnumerable<ISaveListenerInteractor> saveInteractors;

        private RewindTimeInteractor rewindTimeInteractor;

        public override void OnInitialized()
        {
            base.OnInitialized();
            this.saveInteractors = Game.GetInteractors<ISaveListenerInteractor>();
        }


        public void OnUpdate(float unscaledDeltaTime)
        {
            this.autoSaveTimerValue += unscaledDeltaTime;
            if (this.autoSaveTimerValue >= AUTO_SAVE_EVERY_SEC)
            {
                this.autoSaveTimerValue -= AUTO_SAVE_EVERY_SEC;
                this.SaveAll();
            }
        }

        public void SaveAll()
        {
            Debug.Log("SAVEGAME INTERACTOR:: SAVE ALL");
            foreach (var saveInteractor in this.saveInteractors)
            {
                saveInteractor.OnSave();
            }
        }
    }
}