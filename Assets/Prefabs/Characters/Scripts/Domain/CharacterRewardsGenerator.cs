using System.Collections.Generic;
using System.Linq;
using SinSity.Core;
using VavilichevGD.Meta.Rewards;

namespace SinSity.Domain {
    public class CharacterRewardsGenerator {
        private CharactersInteractor _charactersInteractor;
        
        public CharacterRewardsGenerator(CharactersInteractor charactersInteractor) {
            _charactersInteractor = charactersInteractor;
        }

        public List<CharacterReward> Generate() {
            var activeCharacters = _charactersInteractor.GetActiveCharacters();
            var charactersRewards = new List<CharacterReward>();
            foreach (var activeCharacter in activeCharacters) {
                var rewardInfo = activeCharacter.GetRandomDailyReward();
                var characterReward = new CharacterReward(activeCharacter, rewardInfo);
                charactersRewards.Add(characterReward);
            }
            return charactersRewards;
        }
    }
}