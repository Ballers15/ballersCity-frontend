using System;

namespace SinSity.Domain
{
    public interface IProfileLevelHandler
    {
        event Action<object, LevelUp> OnLevelRewardReceivedEvent;
        
        int reachLevel { get; set; }
        
        void OnProfileLevelRisen();
    }
}