using System;
using System.Linq;
using SinSity.Meta.Quests;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Quests;

namespace SinSity.Domain
{
    [CreateAssetMenu(
        fileName = "HintInspectorFirstQuest",
        menuName = "Domain/Hint/HintInspectorFirstQuest"
    )]
    public sealed class HintInspectorFirstQuest : HintStateInspector<HintState>
    {
        private MiniQuestInteractor miniQuestInteractor;

        private MainQuestsInteractor mainQuestsInteractor;

        private TutorialPipelineInteractor tutorialPipelineInteractor;

        public bool IsViewed()
        {
            return this.state.isCompleted;
        }

        public void NotifyAboutFirstQuestViewed()
        {
            if (this.IsViewed())
            {
                throw new Exception("First quest info has already viewed!");
            }

            this.state.isCompleted = true;
            var json = JsonUtility.ToJson(this.state);
            this.repository.Update(this.hintId, json);
            this.NotifyAboutStateChanged();
            this.NotifyAboutTriggered();
        }

        #region Init

        protected override HintState CreateDefaultState()
        {
            return new HintState();
        }

        #endregion

        #region OnReady

        public override void OnReady()
        {
            base.OnReady();
            this.miniQuestInteractor = Game.GetInteractor<MiniQuestInteractor>();
            this.mainQuestsInteractor = Game.GetInteractor<MainQuestsInteractor>();
            this.tutorialPipelineInteractor = Game.GetInteractor<TutorialPipelineInteractor>();
        }

        #endregion

        public override void OnStart()
        {
            base.OnStart();
            if (this.state.isCompleted)
            {
                return;
            }

            this.miniQuestInteractor.OnQuestChangedEvent += this.OnQuestChanged;
            this.mainQuestsInteractor.OnQuestChangedEvent += this.OnQuestChanged;
        }

        private void OnQuestChanged(Quest quest)
        {
            if (!this.tutorialPipelineInteractor.isTutorialCompleted)
            {
                return;
            }

            if (!quest.isCompleted)
            {
                return;
            }

            this.miniQuestInteractor.OnQuestChangedEvent -= this.OnQuestChanged;
            this.mainQuestsInteractor.OnQuestChangedEvent -= this.OnQuestChanged;
            this.NotifyAboutStateChanged();
        }
    }
}