using System.Collections;
using SinSity.Achievements;
using IdleClicker.Gameplay;
using SinSity.Core;
using SinSity.Domain;
using SinSity.Meta;
using SinSity.Meta.Quests;
using SinSity.UI;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Meta.Rewards;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;

namespace SinSity.Scripts
{
    public class InteractorsBaseEcoClicker : InteractorsBase
    {
        
        public override void CreateAllInteractors() {
            
            this.CreateInteractor<BootInteractor>();
            this.CreateInteractor<SecondTickInteractor>();
            this.CreateInteractor<ADSInteractor>();
            this.CreateInteractor<LocalizationInteractor>();
            this.CreateInteractor<SaveGameInteractor>();
            this.CreateInteractor<GameTimeInteractor>();
            this.CreateInteractor<BankInteractor>();
            
            this.CreateInteractor<IdleObjectsInteractor>();
            this.CreateInteractor<IdleObjectRewindTimeInteractor>();
            this.CreateInteractor<IdleObjectExperienceInteractor>();
            this.CreateInteractor<IdleObjectsUpgradeForAdInteractor>();
            
            this.CreateInteractor<NotBuildedSlotsInteractor>();
            this.CreateInteractor<CleanSlotsInteractor>();
            this.CreateInteractor<ShopInteractor>();
            this.CreateInteractor<RealPaymentInteractor>();
            this.CreateInteractor<MiniQuestInteractor>();
            this.CreateInteractor<MainQuestsInteractor>();
            
            this.CreateInteractor<CardsInteractor>();
            
            this.CreateInteractor<CharactersInteractor>();

            this.CreateInteractor<RewindTimeInteractor>();
            this.CreateInteractor<EcoBoostInteractor>();
            this.CreateInteractor<TimeBoosterActivationInteractor>();
            this.CreateInteractor<TutorialPipelineInteractor>();
            this.CreateInteractor<HintSystemInteractor>();
            this.CreateInteractor<DailyRewardInteractor>();
            this.CreateInteractor<AirDropInteractor>();
            
            this.CreateInteractor<ResearchObjectDataInteractor>();
            this.CreateInteractor<ResearchObjectTimerInteractor>();
            this.CreateInteractor<ResearchObjectRewardInteractor>();
            this.CreateInteractor<ResearchStateInteractor>();
            
            this.CreateInteractor<ProfileExperienceDataInteractor>();
            this.CreateInteractor<ProfileLevelInteractor>();
            
            this.CreateInteractor<GemTreeStateInteractor>();
            this.CreateInteractor<GemTreeTimerInteractor>();
            this.CreateInteractor<GemTreeBranchDataInteractor>();
            
            this.CreateInteractor<CoffeeBoostInteractor>();
            this.CreateInteractor<UIInteractorEcoClicker>();

            this.CreateInteractor<FortuneWheelInteractor>();
            
            this.CreateInteractor<DailyRewardsFromCharactersInteractor>();

            //this.CreateInteractor<AchievementInteractor>();
        }


        public Coroutine FastInitialize()
        {
            return Coroutines.StartRoutine(FastInitializeRoutine());
        }

        private IEnumerator FastInitializeRoutine() {
            GameTimeInteractor gameTimeInteractor = GetInteractor<GameTimeInteractor>();
            yield return gameTimeInteractor.InitializeInteractor();
        }
    }
}