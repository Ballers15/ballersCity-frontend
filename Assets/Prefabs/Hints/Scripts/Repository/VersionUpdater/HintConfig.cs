using UnityEngine;

namespace SinSity.Repo
{
    public sealed class HintConfig : ScriptableObject
    {
        [SerializeField] 
        public string[] hintIdSet;
    }
}