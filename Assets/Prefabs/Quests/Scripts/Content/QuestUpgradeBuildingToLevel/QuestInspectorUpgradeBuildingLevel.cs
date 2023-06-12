using System;
using SinSity.Core;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Quests;

namespace SinSity.Quests.Meta
{
    public sealed class QuestInspectorUpgradeBuildingLevel : QuestInspector
    {
        private IdleObjectsInteractor idleObjectsInteractor;

        private readonly QuestStateUpgradeBuildingLevel state;

        public QuestInspectorUpgradeBuildingLevel(Quest quest) : base(quest)
        {
            this.state = (QuestStateUpgradeBuildingLevel) quest.state;
        }

        protected override void SubscribeOnEvents()
        {
            this.idleObjectsInteractor = Game.GetInteractor<IdleObjectsInteractor>();
            IdleObject.OnIdleObjectBuilt += this.OnIdleObjectBuilt;
            IdleObject.OnIdleObjectLevelRisen += this.OnIdleObjectLevelRisen;
            this.CheckState();
        }

        private void OnIdleObjectBuilt(IdleObject idleobject, IdleObjectState idleObjectState)
        {
            this.OnIdleObjectLevelRisen(idleobject, idleobject.state.level, true);
        }

        private void OnIdleObjectLevelRisen(IdleObject idleObject, int newlevel, bool success)
        {
            var idleObjectId = idleObject.id;
            if (idleObjectId != this.state.idleObjectId)
            {
                return;
            }

            this.CheckState();
        }

        protected override void UnsubscribeFromEvents()
        {
            IdleObject.OnIdleObjectLevelRisen -= this.OnIdleObjectLevelRisen;
            IdleObject.OnIdleObjectBuilt -= this.OnIdleObjectBuilt;
        }

        protected override float GetProgressNormalized()
        {
            var idleObject = this.idleObjectsInteractor.GetIdleObject(this.state.idleObjectId);
            var passedLevelSteps = (float) (idleObject.state.level - this.state.startLevel);
            var allSteps = (float) (this.state.targetLevel - this.state.startLevel);
            return passedLevelSteps / allSteps;
        }

        protected override string GetProgressDescription()
        {
            var idleObject = this.idleObjectsInteractor.GetIdleObject(this.state.idleObjectId);
            var currentLevel = idleObject.state.level;
            var targetLevel = this.state.targetLevel;
            return $"{Math.Min(currentLevel, targetLevel)}/{targetLevel}";
        }

        public override void CheckState()
        {
            var idleObjectId = this.state.idleObjectId;
            var idleObject = this.idleObjectsInteractor.GetIdleObject(idleObjectId);
            var newlevel = idleObject.state.level;
            if (newlevel < this.state.targetLevel)
            {
                this.quest.NotifyQuestStateChanged();
            }
            else
            {
                this.quest.Complete();
            }
        }
    }
}