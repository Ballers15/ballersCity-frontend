using UnityEngine;

namespace SinSity.Domain
{
    [CreateAssetMenu(
        fileName = "DefaultProfileLevelHandler",
        menuName = "Domain/ProfileLevel/New DefaultProfileLevelHandler"
    )]
    public class DefaultProfileLevelHandler : ScriptableProfileLevelHandler
    {
        public override void OnProfileLevelRisen()
        {
            this.ReceiveReward();
        }
    }
}