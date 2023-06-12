using System;
using System.Linq;
using EcoClickerScripts.Tools;
using Orego.Util;
using SinSity.Domain;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;
using VavilichevGD.UI;

namespace SinSity.UI {
	public class UIPopupCheats : UIPopup<UIProperties, UIPopupArgs> {

		[SerializeField] private Button buttonAddCleanEnergy;
		[SerializeField] private BigNumber softAmount;
		[Space]
		[SerializeField] private Button buttonAddGems;
		[SerializeField] private int gemsAmount;
		[Space]
		[SerializeField] private Button buttonAddExperience;
		[SerializeField] private Button buttonClose;
		[SerializeField] private Toggle toggleActiveUI;
		[Space]
		[SerializeField] private InputField inputCardNumber;
		[SerializeField] private InputField inputCardAmount;
		[SerializeField] private Button btnAddCard;

		private Cheats cheats;

		protected override void Start() {
			base.Start();
			
			this.cheats = new Cheats();
		}

		private void OnEnable() {
			buttonAddCleanEnergy.AddListener(OnAddCleanEnergyButtonClick);
			buttonAddGems.AddListener(OnAddGemsButtonClick);
			buttonAddExperience.AddListener(OnAddExperienceButtonClick);
			toggleActiveUI.onValueChanged.AddListener(OnToggleActiveUIValueChanged);
			buttonClose.AddListener(OnCloseButtonClick);
			btnAddCard.AddListener(OnBtnAddCardClick);
		}

		private void OnDisable() {
			buttonAddCleanEnergy.RemoveListener(OnAddCleanEnergyButtonClick);
			buttonAddGems.RemoveListener(OnAddGemsButtonClick);
			buttonAddExperience.RemoveListener(this.OnAddExperienceButtonClick);
			toggleActiveUI.onValueChanged.RemoveListener(this.OnToggleActiveUIValueChanged);
			buttonClose.RemoveListener(this.OnCloseButtonClick);
			btnAddCard.AddListener(OnBtnAddCardClick);
		}


		#region CALLBACKS

		private void OnAddCleanEnergyButtonClick() {
			cheats.AddCleanEnergy(softAmount);
		}

		private void OnAddGemsButtonClick() {
			cheats.AddGems(gemsAmount);
		}

		private void OnAddExperienceButtonClick() {
			cheats.AddExperience();
		}
		
		private void OnToggleActiveUIValueChanged(bool newValue) {
			var uiInteractor = GetInteractor<UIInteractor>();
			var uiController = uiInteractor.uiController;
			uiController.SetVisibleUI(newValue);
		}

		private void OnCloseButtonClick() {
			Hide();
		}
		
		private void OnBtnAddCardClick() {
			var cardNumber = Convert.ToInt32(inputCardNumber.text);
			var cardAmount = Convert.ToInt32(inputCardAmount.text);
			var cardsInteractor = Game.GetInteractor<CardsInteractor>();
			var cardsCollection = cardsInteractor.GetCardsCollection();
			var cards = cardsCollection.GetAllCards().ToArray();
			var cardIndex = Math.Min(cards.Length, cardNumber);
			cardsInteractor.IncreaseCardAmount(cards[cardIndex].id, cardAmount);
			inputCardNumber.text = "";
			inputCardAmount.text = "";
		}

		#endregion
	}
}