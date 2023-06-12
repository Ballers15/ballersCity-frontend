using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Orego.Util;
using SinSity.Core;
using SinSity.Data;
using SinSity.Repo;
using VavilichevGD.Architecture;
using VavilichevGD.Meta.Quests;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Tools;

namespace SinSity.Domain {
    public class DailyRewardsFromCharactersInteractor : Interactor {
        public override bool onCreateInstantly { get; } = true;
        
        private DailyRewardsFromCharactersRepository repository;
        private DailyRewardsFromCharactersData data;
        private MiniQuestInteractor miniQuestsInteractor;
        private CharactersInteractor charactersInteractor;

        protected override IEnumerator InitializeRoutineNew() {
            miniQuestsInteractor = GetInteractor<MiniQuestInteractor>();
            repository = GetRepository<DailyRewardsFromCharactersRepository>();
            charactersInteractor = GetInteractor<CharactersInteractor>();
            var firstTimeReceiveDate = GetNextRewardReceiveDate();

            yield return repository.LoadData(firstTimeReceiveDate);

            data = repository.GetData();
            TryGenerateRewards();
            SubscribeOnSecondTick();
        }
        
        private DateTime GetNextRewardReceiveDate() {
            var curDate = GameTime.now;
            var resetDate = curDate.ChangeTime(0, 0, 0, 0);
            return curDate < resetDate ? resetDate : resetDate.AddDays(1);
        }

        private void TryGenerateRewards() {
            if (CantGenerateRewards()) return;
            
            GetAndReceiveRewards();
            SetNewRewardsReceiveDate();
        }

        private bool CantGenerateRewards() {
            return !IsRewardAvailable() || ThereAreNoActiveCharacters() || !IsQuestsUnlockedAndCompleted();
        }

        private bool IsRewardAvailable() {
            return GameTime.now >= data.lastRewardCollectionTimeSerialized.GetDateTime();
        }

        private bool ThereAreNoActiveCharacters() {
            var activeCharacters = charactersInteractor.GetActiveCharacters();
            return activeCharacters.IsEmpty();
        }

        private bool IsQuestsUnlockedAndCompleted() {
            return miniQuestsInteractor.isQuestsUnlocked && miniQuestsInteractor.AllActiveQuestsCompleted();
        }

        private void GetAndReceiveRewards() {
            var rewards = charactersInteractor.GenerateCharactersRewards();
            foreach (var reward in rewards.Select(characterReward => new Reward(characterReward.rewardInfo))) {
                reward.Apply(this, true);
            }
        }

        private void SetNewRewardsReceiveDate() {
            var rewardsReceiveNextDate = GetNextRewardReceiveDate();
            data = new DailyRewardsFromCharactersData(rewardsReceiveNextDate);
            repository.SaveData(data);
        }

        private void SubscribeOnSecondTick() {
            GameTime.OnSecondTickEvent += OnSecondTick;
        }

        private void OnSecondTick() {
            TryGenerateRewards();
        }
    }
}