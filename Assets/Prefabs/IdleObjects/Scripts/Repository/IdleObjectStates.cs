using System;
using System.Collections.Generic;
using UnityEngine;

namespace SinSity.Core
{
    [Serializable]
    public sealed class IdleObjectStates
    {
        public List<string> stateJsons;

        public IdleObjectStates()
        {
            this.stateJsons = new List<string>();
        }

        public void SetStates(IdleObjectState[] states)
        {
            this.stateJsons.Clear();
            foreach (var state in states)
            {
                var stateJson = state.GetJson();
                this.stateJsons.Add(stateJson);
            }
        }

        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }
    }
}