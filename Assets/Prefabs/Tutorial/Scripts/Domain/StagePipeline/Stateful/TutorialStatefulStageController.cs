using UnityEngine;

namespace SinSity.Domain
{
    public abstract class TutorialStatefulStageController<T> : TutorialStageController where T : TutorialState
    {
        public T currentState;

        public override void OnBeginListen()
        {
            this.currentState = this.CreateDefaultState();
        }

        public override void OnContinueListen(string currentStageJson)
        {
            this.currentState = JsonUtility.FromJson<T>(currentStageJson);
        }

        protected abstract T CreateDefaultState();

        public override string GetState()
        {
            return JsonUtility.ToJson(this.currentState);
        }
    }
}