using Orego.Util;
using SinSity.Core;
using SinSity.Domain;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;
using VavilichevGD.UI;

namespace SinSity.UI {
	public class IdleObjectLevelUpPopup : MonoBehaviour {

		[SerializeField] private IdleObjectLevelPanel panelLevelValue;
		[SerializeField] private IdleObjectLevelImprovementInfoPanel panelLevelimprovementInfo;
		[SerializeField] private UIPanelPrice panelPrice;
		[SerializeField] private Text textIncomeValue;
		[SerializeField] private ParticleSystem fxNextLevel;
		[Header("Upgrade for AD")]
		[SerializeField] private Image backgroundImage;
		[SerializeField] private Sprite regularUpgradeBackground;
		[SerializeField] private Sprite adUpgradeBackground;
		[SerializeField] private RectTransform regularPricePanel;
		[SerializeField] private RectTransform adPricePanel;
		[SerializeField] private IdleObjectNextLevelBtn buttonUpgrade;
		[SerializeField] private idleObjectNextLevelForAdBtn buttonUpgradeForAd;

		private Canvas canvas;
		private IdleObject idleObject;
		private IdleObjectsUpgradeForAdInteractor upgradeForAdInteractor;
		private TutorialPipelineInteractor tutorialInteractor;
		private CardsInteractor _cardsInteractor;
		private UIPopupUpgradeForAD upgradeForADPopup;
		private bool isUpgradeForAD;
		private BankInteractor bankInteracktor;


		private void Awake() {
			Initialize();
		}

		private void Initialize() {
			canvas = gameObject.GetComponent<Canvas>();
			idleObject = gameObject.GetComponentInParent<IdleObject>();
			idleObject.OnInitialized += OnIdleObjectInitialized;
			Game.OnGameInitialized += OnGameInitialized;
		}

		private void OnGameInitialized(Game game) {
			Game.OnGameInitialized -= OnGameInitialized;
			_cardsInteractor = Game.GetInteractor<CardsInteractor>();
			this.upgradeForAdInteractor = Game.GetInteractor<IdleObjectsUpgradeForAdInteractor>();
			this.bankInteracktor = Game.GetInteractor<BankInteractor>();
			this.tutorialInteractor = Game.GetInteractor<TutorialPipelineInteractor>();
			var uiInteractor = Game.GetInteractor<UIInteractor>();
			this.upgradeForADPopup = uiInteractor.GetUIElement<UIPopupUpgradeForAD>();
		}

		private void OnIdleObjectInitialized() {
			idleObject.OnInitialized -= OnIdleObjectInitialized;
			UpdateView();
		}

		private void UpdateView() {
			int level = idleObject.state.level;
			panelLevelValue.SetValue(level);
			panelLevelimprovementInfo.UpdateView(idleObject.levelImprovementBlock, level);
			UpdatePrice();
			UpdateIncomePerSec();
		}

		private void UpdatePrice() {
			panelPrice.SetPrice(idleObject.state.priceImprovement);
		}

		private void UpdateIncomePerSec() {
			var dictionary = BigNumberLocalizator.GetSimpleDictionary();
			string incomePerSecString = idleObject.incomePerSec.ToString(BigNumber.FORMAT_XXX_XC,dictionary);
			string secToSting = Localization.GetTranslation("ID_SEC");
			textIncomeValue.text = $"{incomePerSecString}/{secToSting}";
		}

		private void OnEnable() {
			if(bankInteracktor != null && this.HasMoneyOnNextLevel() && this.isUpgradeForAD)
				this.SetupAsUpgradeRegular();
			
			Localization.OnLanguageChanged += OnLanguageChanged;
			idleObject.OnLevelRisenEvent += OnIdleObjectOnLevelRisen;
			this.buttonUpgradeForAd.OnADStartPlaying += this.OnAdUpgradeButtonClicked;
			this.buttonUpgradeForAd.OnUpgradeForAdEndedEvent += this.OnAdUpgradeEnded;
			
			if(this.upgradeForADPopup != null)
				this.upgradeForADPopup.OnCloseClickEvent += this.OnUpgradeForADPopupClosed;

			if (_cardsInteractor != null)
				_cardsInteractor.OnCardAmountChanged += OnCardAmountChanged;

			if (idleObject.isInitialized)
				UpdateView();
		}

		private bool HasMoneyOnNextLevel()
		{
			return bankInteracktor.IsEnoughtSoftCurrency(idleObject.state.priceImprovement);
		}

		private void OnUpgradeForADPopupClosed()
		{
			/*this.SetupAsUpgradeRegular();*/
			this.upgradeForADPopup.Hide();
		}

		#region UPGRADE FOR AD

		private void OnAdUpgradeButtonClicked()
		{
			this.SetVisualAsRegular();
		}

		private void OnAdUpgradeEnded()
		{
			this.SetButtonsAsRegular();
		}
		
		private void SetupAsUpgradeForAd()
		{
			this.isUpgradeForAD = true;
			this.SetVisualAsAD();
			this.SetButtonsAsAD();
		}

		private void SetupAsUpgradeRegular()
		{
			this.isUpgradeForAD = false;
			this.SetVisualAsRegular();
			this.SetButtonsAsRegular();
		}

		private void SetVisualAsAD()
		{
			this.backgroundImage.sprite = this.adUpgradeBackground;
			this.regularPricePanel.SetInvisible();
			this.adPricePanel.SetVisible();
		}
		
		private void SetButtonsAsAD()
		{
			this.buttonUpgrade.SetInvisible();
			this.buttonUpgradeForAd.SetVisible();
		}
		
		private void SetVisualAsRegular()
		{
			this.backgroundImage.sprite = this.regularUpgradeBackground;
			this.regularPricePanel.SetVisible();
			this.adPricePanel.SetInvisible();
			this.UpdateIncomePerSec();
			this.UpdatePrice();
		}
		
		private void SetButtonsAsRegular()
		{
			this.buttonUpgrade.SetVisible();
			this.buttonUpgradeForAd.SetInvisible();
		}

		#endregion
		
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
		
		private void OnCardAmountChanged(ICard obj) {
			UpdateView();
		}
		
		private void OnDisable() {
			Localization.OnLanguageChanged -= OnLanguageChanged;
			idleObject.OnLevelRisenEvent -= OnIdleObjectOnLevelRisen;
			this.buttonUpgradeForAd.OnADStartPlaying -= this.OnAdUpgradeButtonClicked;
			if(this.upgradeForADPopup != null)
				this.upgradeForADPopup.OnCloseClickEvent -= this.OnUpgradeForADPopupClosed;
			this.buttonUpgradeForAd.OnUpgradeForAdEndedEvent -= this.OnAdUpgradeEnded;
			if (_cardsInteractor != null)
				_cardsInteractor.OnCardAmountChanged -= OnCardAmountChanged;
			//this.SetupAsUpgradeRegular();
		}

		public void SetActive(bool isActive) {
			if (isActive) {
				gameObject.SetActive(true);
//				canvas.enabled = true;
			}
			else {
//				canvas.enabled = false;
				gameObject.SetActive(false);
			}
		}
		
		private void OnNotEnoughMoneyAnimationOver_Handle()
		{
			if (!this.upgradeForAdInteractor.CanUpgradeForAd(this.idleObject) || !tutorialInteractor.isTutorialCompleted) return;
	        
			this.upgradeForAdInteractor.SelectAsUpgradeForAd(this.idleObject);
			this.SetupAsUpgradeForAd();
		}
	}
}