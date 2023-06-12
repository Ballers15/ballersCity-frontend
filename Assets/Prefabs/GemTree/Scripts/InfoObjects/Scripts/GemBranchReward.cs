namespace SinSity.Core {
    public struct GemBranchReward {
        public int gemsReward;
        public bool jackpot;

        public GemBranchReward(int gemsReward, bool jackpot) {
            this.gemsReward = gemsReward;
            this.jackpot = jackpot;
        }
    }
}