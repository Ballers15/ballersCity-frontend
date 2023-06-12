using System;
using System.Linq;
using Ecorobotics;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Core {
    public class Character : ICharacter {
        public event Action<ICharacter> OnCharacterActiveStateChanged;
        
        public string id => info.id;
        public ICard characterCard { get; }
        public IIdleObject idleObject { get; }
        public ICharacterInfo info { get; }
        public bool isActive { get; private set;}

        public Character(CharacterConstructParams characterParams) {
            info = characterParams.info;
            characterCard = characterParams.card;
            idleObject = characterParams.idleObject;
            isActive = IsCharacterActive();
            characterCard.OnAmountChanged += CheckActiveState;
        }
        
        private bool IsCharacterActive() {
            return characterCard.amount > 0;
        }

        private void CheckActiveState(ICard obj) {
            if(isActive == IsCharacterActive()) return;

            isActive = IsCharacterActive();
            OnCharacterActiveStateChanged?.Invoke(this);
        }
        
        public void ApplyIncomeMultiplierToIdleObject() {
            var characterIncomeMultiplier = GetCharacterIncomeMultiplier();
            idleObject.AddIncomeMultiplier(id, characterIncomeMultiplier);
        }
        
        private Coefficient GetCharacterIncomeMultiplier() {
            return new Coefficient(info.characterIncomeMultiplier);
        }

        public void RemoveIncomeMultiplierFromIdleObject() {
            idleObject.RemoveIncomeMultiplier(id);
        }

        public RewardInfo GetRandomDailyReward() {
            var objectsWithWeights = info.rewardsWithWeights.Select(valuesWithWeight => valuesWithWeight.GetObjectWithWeight()).ToArray();
            var random = new RandomizerWeight(objectsWithWeights);
            return random.GetRandom<RewardInfo>();
        }
    }
}