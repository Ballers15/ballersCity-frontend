using VavilichevGD.Meta.Rewards;

namespace SinSity.Core {
    public struct CharacterReward {
        public ICharacter character;
        public RewardInfo rewardInfo;

        public CharacterReward(ICharacter character, RewardInfo rewardInfo) {
            this.character = character;
            this.rewardInfo = rewardInfo;
        }
    }
}