using SinSity.Core;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Quests;

namespace SinSity.Meta.Quests
{
    public sealed class QuestInspectorBuildSmth : QuestInspector
    {
        private IdleObjectsInteractor idleObjectsInteractor;
        
        public QuestInspectorBuildSmth(Quest quest) : base(quest)
        {
        }

        protected override void SubscribeOnEvents()
        {
            IdleObject.OnIdleObjectBuilt += OnOnIdleObjectBuilt;
            this.CheckState();
        }

        public override void CheckState()
        {
            this.idleObjectsInteractor = Game.GetInteractor<IdleObjectsInteractor>();
            var idleObjectId = this.quest.GetState<QuestStateBuildSmth>().idleObjectId;
            var idleObject = this.idleObjectsInteractor.GetIdleObject(idleObjectId);
            if (idleObject.isBuilt)
            {
                this.quest.Complete();
            }
        }

        private void OnOnIdleObjectBuilt(IdleObject idleobject, IdleObjectState newstate)
        {
            var questStateBuildSmth = this.quest.GetState<QuestStateBuildSmth>();
            if (idleobject.id == questStateBuildSmth.idleObjectId)
            {
                this.quest.Complete();
            }
        }

        protected override void UnsubscribeFromEvents()
        {
            IdleObject.OnIdleObjectBuilt -= OnOnIdleObjectBuilt;
        }

        protected override float GetProgressNormalized()
        {
            return this.quest.state.isCompleted
                ? 1f
                : 0f;
        }

        protected override string GetProgressDescription()
        {
            var value = quest.state.isCompleted
                ? 1
                : 0;

            return $"{value}/1";
        }
    }
}