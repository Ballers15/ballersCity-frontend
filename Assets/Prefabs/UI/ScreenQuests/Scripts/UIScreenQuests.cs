using UnityEngine;
using VavilichevGD.UI;
using VavilichevGD.Audio;
using VavilichevGD.Architecture;
using SinSity.Services;
using Orego.Util;
using SinSity.Domain;
using SinSity.Repo;
using UnityEngine.UI;

namespace SinSity.UI
{
    public sealed class UIScreenQuests : UIPopup<UIScreenQuestsProperties, UIPopupArgs>
    {
        [SerializeField] private Button btnClose;
        
        public delegate void OnUIScreenQuestsStateChangeHandler(UIScreenQuests screenQuests, bool isActive);
        public static event OnUIScreenQuestsStateChangeHandler OnUIScreenQuestsStateChanged;

        private UIWidgetMainQuest widgetMainQuest;
        private UIWidgetMiniQuestController widgetMiniQuestController;
        private MiniQuestInteractor miniQuestInteractor;
        private MiniQuestsRepository miniQuestsRep;
        private Vector2 positionContainerQuestsDefault;

        protected override void Awake()
        {
            base.Awake();
            this.widgetMainQuest = this.GetComponentInChildren<UIWidgetMainQuest>();
            this.widgetMiniQuestController = new UIWidgetMiniQuestController(
                this.GetComponentsInChildren<UIWidgetMiniQuest>(true)
            );
            this.miniQuestsRep = Game.GetRepository<MiniQuestsRepository>();
            this.miniQuestInteractor = Game.GetInteractor<MiniQuestInteractor>();

            this.positionContainerQuestsDefault = this.properties.containerQuests.anchoredPosition;
        }

        public override void Initialize()
        {
            base.Initialize();
            this.HideInstantly();
        }

        #region Show

        public override void Show()
        {
            base.Show();
            this.widgetMainQuest.OnRefresh();
            this.widgetMiniQuestController.OnRefresh();
        }

        #endregion

        protected override void NotifyAboutStateChanged(bool activated)
        {
            base.NotifyAboutStateChanged(activated);
            OnUIScreenQuestsStateChanged?.Invoke(this, activated);
        }

        private void OnEnable() {
            this.properties.containerQuests.anchoredPosition = positionContainerQuestsDefault;
            this.widgetMiniQuestController.OnEnable();
            this.miniQuestInteractor.OnResetTimeChangedEvent += OnResetTimeChanged;
            btnClose.onClick.AddListener(OnCloseBtnClick);
        }

        private void OnResetTimeChanged(object sender)
        {
            if (sender.GetType() != this.GetType())
            {
                miniQuestsRep.statistic.resetWasUsed = false;
                miniQuestsRep.Save();
            }
        }
        
        private void OnDisable()
        {
            btnClose.onClick.RemoveListener(OnCloseBtnClick);
            this.miniQuestInteractor.OnResetTimeChangedEvent -= OnResetTimeChanged;
            this.widgetMiniQuestController.OnDisable();
        }
        
        
        private void OnCloseBtnClick() {
            NotifyAboutResults(new UIPopupArgs(this, UIPopupResult.Close));
            Hide();
        }
    }
}