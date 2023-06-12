using UnityEngine;

namespace SinSity.Domain
{
    [CreateAssetMenu(
        menuName = "Domain/EcoBoostConfig/New",
        fileName = "EcoBoostConfig"
    )]
    public sealed class EcoBoostConfig : ScriptableObject
    {
        [SerializeField]
        public int idleObjectMultiplier = 2;

        [SerializeField]
        public int durationSeconds = 14400;

        [SerializeField]
        public int limitDurationTime = 57600;
    }
}