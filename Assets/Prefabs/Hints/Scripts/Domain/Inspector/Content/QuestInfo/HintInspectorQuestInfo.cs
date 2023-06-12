using System;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Domain
{
    [CreateAssetMenu(
        fileName = "HintInspectorQuestInfo",
        menuName = "Domain/Hint/HintInspectorQuestInfo"
    )]
    public sealed class HintInspectorQuestInfo : HintStateInspector<HintState>
    {
        public bool IsViewed()
        {
            return this.state.isCompleted;
        }

        public void NotifyAboutQuestInfoViewed()
        {
            if (this.IsViewed())
            {
                throw new Exception("Quest info has already viewed!");
            }

            this.state.isCompleted = true;
            var json = JsonUtility.ToJson(this.state);
            this.repository.Update(this.hintId, json);
            this.NotifyAboutStateChanged();
            this.NotifyAboutTriggered();
        }

        protected override HintState CreateDefaultState()
        {
            return new HintState();
        }
    }
}