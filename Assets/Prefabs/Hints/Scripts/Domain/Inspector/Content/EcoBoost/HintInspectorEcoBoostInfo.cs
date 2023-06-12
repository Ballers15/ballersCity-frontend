using System;
using UnityEngine;

namespace SinSity.Domain
{
    [CreateAssetMenu(
        fileName = "HintInspectorEcoBoostInfo",
        menuName = "Domain/Hint/HintInspectorEcoBoostInfo"
    )]
    public sealed class HintInspectorEcoBoostInfo : HintStateInspector<HintState>
    {
        public bool IsViewed()
        {
            return this.state.isCompleted;
        }

        public void NotifyAboutEcoBoostInfoViewed()
        {
            if (this.IsViewed())
            {
                throw new Exception("Eco boost info has already viewed!");
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