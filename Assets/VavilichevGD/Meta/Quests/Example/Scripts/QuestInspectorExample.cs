using UnityEngine;

namespace VavilichevGD.Meta.Quests.Examples {
    public class QuestInspectorExample : QuestInspector {

        public QuestInspectorExample(Quest quest) : base(quest) { }
        
        protected override void SubscribeOnEvents() {
            Debug.Log("Quest started. Inspector subscribed on events");
        }

        protected override void UnsubscribeFromEvents() {
            Debug.Log("Quest canceled. Inspector unsubscribed on events");
        }

        protected override float GetProgressNormalized() {
            throw new System.NotImplementedException();
        }

        protected override string GetProgressDescription() {
            throw new System.NotImplementedException();
        }

        public override void CheckState()
        {
            throw new System.NotImplementedException();
        }
    }
}