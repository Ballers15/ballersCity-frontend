using System;
using UnityEngine;

namespace SinSity.Domain
{
    [CreateAssetMenu(
        fileName = "HintInspectorShopInfo",
        menuName = "Domain/Hint/HintInspectorShopInfo"
    )]
    public sealed class HintInspectorShopInfo : HintStateInspector<HintState>
    {
        public bool IsViewed()
        {
            return this.state.isCompleted;
        }

        public void NotifyAboutShopInfoViewed()
        {
            if (this.IsViewed())
            {
                throw new Exception("Shop info has already viewed!");
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