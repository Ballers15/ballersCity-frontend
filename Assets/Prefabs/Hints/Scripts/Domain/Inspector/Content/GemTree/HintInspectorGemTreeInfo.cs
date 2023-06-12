using System;
using UnityEngine;

namespace SinSity.Domain
{
    [CreateAssetMenu(
        fileName = "HintInspectorGemTreeInfo",
        menuName = "Domain/Hint/HintInspectorGemTreeInfo"
    )]
    public sealed class HintInspectorGemTreeInfo : HintStateInspector<HintState>
    {
        public bool IsViewed()
        {
            return this.state.isCompleted;
        }

        public void NotifyAboutGemTreeInfoViewed()
        {
            if (this.IsViewed())
            {
                throw new Exception("Gem tree info has already viewed!");
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