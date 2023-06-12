using UnityEngine;

namespace SinSity.Core
{
    [CreateAssetMenu(
        fileName = "ScriptableVersion",
        menuName = "Util/New ScriptableVersion"
    )]
    public sealed class ScriptableVersion : ScriptableObject
    {
        [SerializeField]
        public int value;
    }
}