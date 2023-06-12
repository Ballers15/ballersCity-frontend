using System;
using SinSity.Core;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Domain
{
    public abstract class ScriptableProfileLevelHandler : ScriptableObject,
        IProfileLevelHandler,
        IBankStateWithoutNotification
    {
        [SerializeField]
        protected ProfileLevelUnlockConfig unlockConfig;

        #region Event

        public event Action<object, LevelUp> OnLevelRewardReceivedEvent;

        #endregion

        public int reachLevel { get; set; }

        protected ProfileLevelInteractor profileLevelInteractor { get; private set; }

        public virtual void OnStart()
        {
            this.profileLevelInteractor = Game.GetInteractor<ProfileLevelInteractor>();
        }

        public abstract void OnProfileLevelRisen();

        protected virtual void ReceiveReward()
        {
            var levelUp = this.unlockConfig.GenerateLevelUp(this.reachLevel);
            levelUp.Apply(this);
            this.OnLevelRewardReceivedEvent?.Invoke(this, levelUp);
        }
    }
}