using Orego.Util;
using SinSity.Domain;
using SinSity.Repo;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture;
using VavilichevGD.Audio;
using VavilichevGD.UI;

namespace SinSity.UI {
	public class UIButtonSettings : UIWidget<UIProperties> {

		[SerializeField] private Button button;
		[SerializeField] private AudioClip audioClipSettingsClick;
		[SerializeField] private int orderDefault = 310;
		[SerializeField] private int orderInTutorial = 600;
		[SerializeField] private Canvas myCanvas;


		private UIInteractor uiInteractor;
		private TutorialRepository tutorialRepository;
		private TutorialPipelineInteractor tutorialInteractor;


		public override void Initialize() {
			base.Initialize();
			this.uiInteractor = Game.GetInteractor<UIInteractor>();
			this.tutorialRepository = Game.GetRepository<TutorialRepository>();
			this.tutorialInteractor = Game.GetInteractor<TutorialPipelineInteractor>();
		}

		#region LIFECYCLE

		private void OnEnable() {
			this.button.AddListener(OnClick);
		}

		private void OnDisable() {
			this.button.RemoveListener(OnClick);
		}

		private void Start() {
			if (!Game.isInitialized)
				Game.OnGameInitialized += this.OnGameInitialized;
			else
				this.UpdateMyState();
		}

		#endregion



		private void UpdateMyState() {
			var tutorialStatistics = this.tutorialRepository.GetStatistics();

			if (tutorialStatistics.isCompleted)
				this.SetOrder(this.orderDefault);
			else {
				this.SetOrder(this.orderInTutorial);
				this.tutorialInteractor.OnTutorialCompleteEvent -= this.OnTutorialComplete;
				this.tutorialInteractor.OnTutorialCompleteEvent += this.OnTutorialComplete;
			}
		}

		private void SetOrder(int order) {
			this.myCanvas.sortingOrder = order;
		}


		private void Reset() {
			if (this.myCanvas == null)
				this.myCanvas = this.GetComponent<Canvas>();
		}


		#region CALLBACKS

		private void OnGameInitialized(Game game) {
			Game.OnGameInitialized -= this.OnGameInitialized;
			this.UpdateMyState();
		}

		private void OnTutorialComplete() {
			this.tutorialInteractor.OnTutorialCompleteEvent -= this.OnTutorialComplete;
			this.UpdateMyState();
		}

		private void OnClick() {
			var alreadyActive = this.uiInteractor.uiController.IsActiveUIElement<UIPopupSettings>();
			if (!alreadyActive) {
				var popup = this.uiInteractor.ShowElement<UIPopupSettings>();
				SFX.PlaySFX(this.audioClipSettingsClick);
				this.SetOrder(this.orderDefault);

				popup.OnUIElementClosedCompletelyEvent += this.OnPopupClosedCompletelyEvent;
			}
			else {
				this.UpdateMyState();
			}
		}

		private void OnPopupClosedCompletelyEvent(UIElement uielement) {
			var popup = (UIPopupSettings) uielement;
			popup.OnUIElementClosedCompletelyEvent -= this.OnPopupClosedCompletelyEvent;
			this.UpdateMyState();
		}

		#endregion

	}
}