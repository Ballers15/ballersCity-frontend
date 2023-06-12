using System;
using UnityEngine;

namespace SinSity.Domain
{
    [CreateAssetMenu(
        fileName = "HintInspectorResearchInfo",
        menuName = "Domain/Hint/HintInspectorResearchInfo"
    )]
    public sealed class HintInspectorResearchInfo : HintStateInspector<HintState>
    {
        public bool IsViewed()
        {
            return this.state.isCompleted;
        }

        public void NotifyAboutResearchInfoViewed()
        {
            if (this.IsViewed())
            {
                throw new Exception("Research info has already viewed!");
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