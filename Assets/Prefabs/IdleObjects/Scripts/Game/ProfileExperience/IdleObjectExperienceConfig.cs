using UnityEngine;

namespace SinSity.Core
{
    [CreateAssetMenu(
        fileName = "IdleObjectExperienceConfig",
        menuName = "Game/IdleObject/New IdleObjectExperienceConfig"
    )]
    public sealed class IdleObjectExperienceConfig : ScriptableObject
    {
        [SerializeField]
        public uint experienceForBuild = 10;

        [SerializeField]
        public uint experienceForImprovement = 1;
    }
}