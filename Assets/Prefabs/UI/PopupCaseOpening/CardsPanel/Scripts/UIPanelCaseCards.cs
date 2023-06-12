using System.Collections;
using UnityEngine;
using VavilichevGD;

namespace SinSity.UI {
	public class UIPanelCaseCards : AnimObject {

		[SerializeField]
		private Transform startPoint;
		[SerializeField]
		private float scaleStart = 0f;
		[SerializeField]
		private float speed = 1f;
		[SerializeField]
		private UICaseCard[] cards;

		protected const string BOOL_AWAY = "away";

		public delegate void CardsFlewAway(UIPanelCaseCards uiPanelCards);
		public event CardsFlewAway OnCardsFlewAway;

		private void Awake() {
			Initialize();
		}

		private void Initialize() {
			SetActiveAnimator(false);
			SetActiveCards(false);
		}

		private void SetActiveAnimator(bool isActive) {
			animator.enabled = isActive;
		}

		public void SetActiveCards(bool isActive) {
			foreach (UICaseCard card in cards)
				card.gameObject.SetActive(isActive);
		}

		
//		public void StartFly(ShopProductCase productCase) {
//			CaseRewardProperties[] randomRewardProperties = GetSetuppedCardsWithRandomRewards(productCase);
//			PrepareAnimator();
//
//			int rewardsCount = randomRewardProperties.Length;
//			StartCoroutine("StartFlyRoutine", rewardsCount);
//		}

//		private CaseRewardProperties[] GetSetuppedCardsWithRandomRewards(ShopProductCase productCase) {
//			CaseRewardProperties[] randomRewardProperties = productCase.GetRandomRewards();
//			for (int i = 0; i < randomRewardProperties.Length; i++) {
//				CaseReward reward = randomRewardProperties[i].caseReward;
//				cards[i].Setup(reward);
//			}
//			return randomRewardProperties;
//		}

		private void PrepareAnimator() {
			animator.enabled = false;
			SetBoolFalse(BOOL_AWAY);
			StopCoroutine("StartFlyRoutine");
			for (int i = 0; i < cards.Length; i++)
				cards[i].Stop();
		}

		private IEnumerator StartFlyRoutine(int count) {
			float delay = 0.15f;
			PrepareCardsForFlying(count);
			for (int i = 0; i < count; i++) {
				cards[i].StartAnimationFly(startPoint, speed);
				yield return new WaitForSeconds(delay);
			}
		}

		private void PrepareCardsForFlying(int count) {
			for (int i = 0; i < count; i++) {
				cards[i].PrepareToFly(startPoint.position);
				cards[i].gameObject.SetActive(true);
			}
		}

		public void FlyCardsAway() {
			animator.enabled = true;
			SetBoolTrue(BOOL_AWAY);
		}

		public void Handle_CardsFlewAway() {
			if (OnCardsFlewAway != null)
				OnCardsFlewAway(this);
		}

		private void Reset() {
			if (cards == null || cards.Length == 0)
				cards = GetComponentsInChildren<UICaseCard>(true);
		}

		public void ApplyRewards() {
			foreach(UICaseCard card in cards) {
//				if (card.isActive)
//					card.ApplyReward();
			}
		}
	}
}