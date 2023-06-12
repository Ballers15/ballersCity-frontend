using System.Collections;
using Orego.Util;
using SinSity.Achievements;
using SinSity.Core;
using SinSity.Meta;
using SinSity.Repo;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;

namespace SinSity.Scripts {
    public class RepositoriesBaseEcoClicker : RepositoriesBase {
        
        public override void CreateAllRepositories() {
            this.CreateRepository<GameTimeRepository>();
            this.CreateRepository<BankRepository>();
            this.CreateRepository<IdleObjectsRepository>();
            this.CreateRepository<MainQuestRepository>();
            this.CreateRepository<ShopRepository>();
            this.CreateRepository<CardsRepository>();
            this.CreateRepository<MiniQuestsRepository>();
            this.CreateRepository<RenovationRepository>();
            this.CreateRepository<EcoBoostRepository>();
            this.CreateRepository<ADSRepository>();
            this.CreateRepository<LocalizationRepository>();
            this.CreateRepository<TutorialRepository>();
            this.CreateRepository<HintRepository>();
            this.CreateRepository<DailyRewardRepository>();
            this.CreateRepository<AirDropRepository>();
            this.CreateRepository<ResearchDataRepository>();
            this.CreateRepository<ProfileExperienceRepository>();
            this.CreateRepository<GemTreeRepository>();
            this.CreateRepository<CoffeeBoostRepository>();
            this.CreateRepository<FortuneWheelRepository>();
            this.CreateRepository<AchievementRepository>();
            this.CreateRepository<DailyRewardsFromCharactersRepository>();
        }

        protected override IEnumerator InitializeAllRepositoriesRoutine() {
            yield return base.InitializeAllRepositoriesRoutine();
            yield return this.UpdateVersionInRepositories();
        }

        private IEnumerator UpdateVersionInRepositories() {
            var repositories = this.GetRepositories<IUpdateVersionRepository>();
            var isRequiredUpdate = true;
            while (isRequiredUpdate) {
                isRequiredUpdate = false;
                foreach (var repository in repositories) {
                    var isUpdatedReference = new Reference<bool>();

                    yield return repository.OnUpdateVersion(isUpdatedReference);
                    if (isUpdatedReference.value) {

#if DEBUG
                        Debug.Log("UPDATED REPO: " + repository.GetType().Name);
#endif
                        isRequiredUpdate = true;
                        break;
                    }
                }
            }
        }

        public Coroutine FastInitialize() {
            return Coroutines.StartRoutine(FastInitializeRoutine());
        }

        private IEnumerator FastInitializeRoutine() {
            GameTimeRepository gameTimeRepository = GetRepository<GameTimeRepository>();
            yield return gameTimeRepository.InitializeRepository();
        }
    }
}