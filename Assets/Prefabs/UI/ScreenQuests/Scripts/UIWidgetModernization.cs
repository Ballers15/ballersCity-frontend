using System;
using Orego.Util;
using SinSity.Domain;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture;
using VavilichevGD.Audio;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Tools;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public sealed class UIWidgetModernization : UIWidget<UIWidgetModernizationProperties>
    {
        public static event Action OnHintClickedEvent;
        
        private ModernizationInteractor modernizationInteractor;

        public void OnEnable()
        {
            this.modernizationInteractor = Game.GetInteractor<ModernizationInteractor>();
            if (modernizationInteractor.modernizationData.isAvailable && modernizationInteractor.modernizationData.scores > 0)
            {
                properties.containerModernization.SetVisible();
                this.properties.buttonHint.onClick.AddListener(this.OnHintBtnClick);
                this.properties.buttonStartModernization.onClick.AddListener(this.OnStartModernizationClick);
            }
            else
            {
                properties.containerModernization.SetInvisible();
                if(!modernizationInteractor.modernizationData.isAvailable)
                    modernizationInteractor.OnModernizationIsAvalible += this.OnModernizationIsAvalible;
                if(modernizationInteractor.modernizationData.scores == 0)
                    modernizationInteractor.OnModScoreChangedEvent += this.OnModScoreChanged;
            }
        }

        private void OnDisable()
        {
            this.properties.buttonStartModernization.onClick.RemoveAllListeners();
            this.properties.buttonHint.onClick.RemoveAllListeners();
        }
        
        private void OnHintBtnClick()
        {
            UIInteractor uiInteractor = GetInteractor<UIInteractor>();

            UIPopupModernizationInfo popupHint = uiInteractor.GetUIElement<UIPopupModernizationInfo>();
            popupHint.SetPopupType(UIPopupModernizationInfoProperties.PopupType.Score);
            popupHint.Show();
        }

        private void OnModernizationIsAvalible()
        {
            modernizationInteractor.OnModernizationIsAvalible -= this.OnModernizationIsAvalible;
            properties.containerModernization.SetVisible();
        }

        private void OnModScoreChanged(object interactor,object sender)
        {
            modernizationInteractor.OnModScoreChangedEvent -= this.OnModScoreChanged;
            properties.containerModernization.SetVisible();
        }

        public void OnStartModernizationClick()
        {
            UIInteractor uiInteractor = GetInteractor<UIInteractor>();

            var popupModernization = uiInteractor.ShowElement<UIPopupModernization>();
            popupModernization.OnDialogueResults += OnDialogueResults;
            SFX.PlaySFX(this.properties.audioClipStartModernization);
        }

        private void OnDialogueResults(UIPopupArgs e)
        {
            UIPopupModernization popupModernization = (UIPopupModernization)e.uiElement;
            popupModernization.OnDialogueResults -= OnDialogueResults;

            if (e.result == UIPopupResult.Apply)
            {
                UIInteractor uiInteractor = GetInteractor<UIInteractor>();
                UIScreenQuests screenQuests = uiInteractor.GetUIElement<UIScreenQuests>();
                screenQuests.Hide();
                this.modernizationInteractor.StartModernization();
            }
            this.LogPopupResults(e);
        }

        private void LogPopupResults(UIPopupArgs e) {
            var result = e.result == UIPopupResult.Apply
                ? ModernizationAnalytics.RESULT_VALUE_MDRN_HAPPEND
                : ModernizationAnalytics.RESULT_VALUE_CLOSED;
            this.modernizationInteractor.analytics.LogModernizationPopupResults(result);
        }

        public Button GetModernizationBtn()
        {
            return properties.buttonStartModernization;
        }
    }
}