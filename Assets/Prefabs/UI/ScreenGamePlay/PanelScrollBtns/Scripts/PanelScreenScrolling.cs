using System;
using SinSity.Core;
using SinSity.Domain;
using SinSity.UI;
using Orego.Util;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Audio;
using VavilichevGD.UI;

namespace IdleClicker.UI
{
    public sealed class PanelScreenScrolling : UIPanel<UIPanelScreenScrollingProperties>
    {
        [SerializeField]
        private Sounds m_sounds;

        private TutorialPipelineInteractor tutorialPipelineInteractor;

        private UIInteractor uiInteractor;
        private UIController uiController;

        protected override void Awake()
        {
            base.Awake();
            this.HideButtons();
        }

        private void OnEnable()
        {
            uiInteractor = Game.GetInteractor<UIInteractor>();
            uiController = uiInteractor.uiController;
            this.properties.btnMoveUp.onClick.AddListener(this.OnMoveUpBtnClick);
            this.properties.btnMoveDown.onClick.AddListener(this.OnMoveDownBtnClick);
        }

        public void OnMoveUpBtnClick()
        {
            CameraController.instance.MoveUp();
            SFX.PlaySFX(this.m_sounds.audioClipClick);
        }

        public void OnMoveDownBtnClick()
        {
            CameraController.instance.MoveDown();
            SFX.PlaySFX(this.m_sounds.audioClipClick);
        }

        private void OnDisable()
        {
            this.properties.btnMoveUp.onClick.RemoveListener(this.OnMoveUpBtnClick);
            this.properties.btnMoveDown.onClick.RemoveListener(this.OnMoveDownBtnClick);
        }

        protected override void OnGameInitialized()
        {
            base.OnGameInitialized();
            this.tutorialPipelineInteractor = this.GetInteractor<TutorialPipelineInteractor>();
        }

        protected override void OnGameStart()
        {
            base.OnGameStart();
            if (this.tutorialPipelineInteractor.isTutorialCompleted)
            {
                this.ShowButtons();
            }
            else
            {
                this.tutorialPipelineInteractor.OnTutorialCompleteEvent += this.OnTutorialComplete;
            }
        }

        private void OnTutorialComplete()
        {
            this.tutorialPipelineInteractor.OnTutorialCompleteEvent -= this.OnTutorialComplete;
            SetCameraRenderLayerToDefault();
            this.ShowButtons();
        }
        
        protected void SetCameraRenderLayerToDefault() {
            UIInteractor uiInteractor = Game.GetInteractor<UIInteractor>();
            Camera camera = uiInteractor.uiController.gameObject.GetComponentInChildren<Camera>(true);
            camera.cullingMask = (1 << LayerMask.NameToLayer("UI"));
        }

        private void HideButtons()
        {
            this.properties.btnMoveDown.SetInvisible();
            this.properties.btnMoveUp.SetInvisible();
        }

        private void ShowButtons()
        {
            this.properties.btnMoveDown.SetVisible();
            this.properties.btnMoveUp.SetVisible();
        }

        [Serializable]
        public sealed class Sounds
        {
            [SerializeField]
            public AudioClip audioClipClick;
        }
    }
}