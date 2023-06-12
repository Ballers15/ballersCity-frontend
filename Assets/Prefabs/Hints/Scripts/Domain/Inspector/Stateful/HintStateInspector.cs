using UnityEngine;

namespace SinSity.Domain
{
    public abstract class HintStateInspector<T> : HintInspector where T : HintState
    {
        public T state { get; private set; }

        protected sealed override void OnInitState(string hintValue)
        {
            if (hintValue != null)
            {
                this.state = JsonUtility.FromJson<T>(hintValue);
                return;
            }

            var defaultState = this.CreateDefaultState();
            this.state = defaultState;
            var json = JsonUtility.ToJson(defaultState);
            this.repository.Update(this.hintId, json);
        }

        protected abstract T CreateDefaultState();
    }
}