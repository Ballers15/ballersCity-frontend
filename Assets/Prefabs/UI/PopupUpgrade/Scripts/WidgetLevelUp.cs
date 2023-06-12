using SinSity.Core;
using SinSity.Domain;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Tools;

namespace SinSity.UI {
    public class WidgetLevelUp : MonoBehaviour {
	    [SerializeField] private IdleObjectLevelPanel panelLevelValue;
		[SerializeField] private IdleObjectLevelImprovementInfoPanel panelLevelimprovementInfo;
		[SerializeField] private UIPanelPrice panelPrice;
		[SerializeField] private Text textIncomeValue;
		[SerializeField] private ParticleSystem fxNextLevel;
		[SerializeField] private WidgetIdleObjectNextLevelBtn buttonUpgrade;
		[SerializeField] private Text textRank;
		[SerializeField] private Text textProductionTime;
		[SerializeField] private Text textAutowork;
		[SerializeField] private WidgetIdleCharacters widgetCharacters;
		
		private IdleObject idleObject;
		private IdleObjectsUpgradeForAdInteractor upgradeForAdInteractor;
		private TutorialPipelineInteractor tutorialInteractor;

		private void Awake() {
			Initialize();
		}
		
		private void OnEnable() {
			if (idleObject == null) return;
			
			Localization.OnLanguageChanged += OnLanguageChanged;
			idleObject.OnLevelRisenEvent += OnIdleObjectOnLevelRisen;

			if (idleObject.isInitialized)
				UpdateView();
		}
		
		private void OnDisable() {
			if (idleObject == null) return;
			
			Localization.OnLanguageChanged -= OnLanguageChanged;
			idleObject.OnLevelRisenEvent -= OnIdleObjectOnLevelRisen;
		}

		private void Initialize() {
			Game.OnGameInitialized += OnGameInitialized;
		}
		
		private void OnGameInitialized(Game game) {
			Game.OnGameInitialized -= OnGameInitialized;
			upgradeForAdInteractor = Game.GetInteractor<IdleObjectsUpgradeForAdInteractor>();
			tutorialInteractor = Game.GetInteractor<TutorialPipelineInteractor>();
		}
		
		public void Setup(IdleObject idle) {
			idleObject = idle;
			buttonUpgrade.Setup(idle);
			widgetCharacters.Setup(idle);
		}

		private void UpdateView() {
			var level = idleObject.state.level;
			panelLevelValue.SetValue(level);
			panelLevelimprovementInfo.UpdateView(idleObject.levelImprovementBlock, level);
			UpdateRank();
			UpdateProductionTime();
			UpdateAutowork();
			UpdatePrice();
			UpdateIncomePerSec();
		}

		private void UpdateRank() {
			var rank = idleObject.levelImprovementBlock.blockLevel;
			textRank.text = $"Rank {rank.ToString()}";
		}
		
		private void UpdateProductionTime() {
			var time = idleObject.state.incomePeriod;
			textProductionTime.text = $"{time} s.";
		}
		
		private void UpdateAutowork() {
			var strAutowork = (idleObject.state.autoPlayEnabled) ? "Yes" : "No";
			textAutowork.text = strAutowork;
		}

		private void UpdatePrice() {
			panelPrice.SetPrice(idleObject.state.priceImprovement);
		}

		private void UpdateIncomePerSec() {
			var dictionary = BigNumberLocalizator.GetSimpleDictionary();
			var incomePerSecString = idleObject.incomePerSec.ToString(BigNumber.FORMAT_XXX_XC,dictionary);
			var secToSting = Localization.GetTranslation("ID_SEC");
			textIncomeValue.text = $"{incomePerSecString}/{secToSting}";
		}

		private void OnLanguageChanged() {
			if (!idleObject.isInitialized)
				return;
			
			UpdateIncomePerSec();
			UpdatePrice();
		}

		private void OnIdleObjectOnLevelRisen(int newlevel, bool success) {
			if (success) {
				UpdateView();
				fxNextLevel.Emit(1);
			}
		}

		private void OnNotEnoughMoneyAnimationOver_Handle() {
			if (!tutorialInteractor.isTutorialCompleted) return;
	        
			upgradeForAdInteractor.SelectAsUpgradeForAd(this.idleObject);
		}
    }
}