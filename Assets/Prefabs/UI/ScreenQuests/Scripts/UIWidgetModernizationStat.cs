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
    public sealed class UIWidgetModernizationStat : UIWidget<UIWidgetModernizationStatProperties>
    {
        private ModernizationInteractor modernizationInteractor;

        public void OnEnable()
        {
            //TODO: refresh() if game is initialized Game.isInitialized;
            this.modernizationInteractor = Game.GetInteractor<ModernizationInteractor>();
            this.modernizationInteractor.OnModernizationDataStateChanged += this.OnModernizationDataStateChanged;
            
            if(Game.isInitialized) 
                this.Refresh();
            
            Localization.OnLanguageChanged += OnLanguageChanged;
        }

        private void OnLanguageChanged()
        {
            this.Refresh();
        }

        private void OnDisable()
        {
            this.modernizationInteractor.OnModernizationDataStateChanged -= this.OnModernizationDataStateChanged;
            Localization.OnLanguageChanged -= OnLanguageChanged;
        }

        public void OnRefresh()
        {
            this.Refresh();
        }

        private void OnModernizationDataStateChanged(ModernizationInteractor interactor, object sender)
        {
            this.Refresh();
        }

        private void Refresh()
        {
            string localizedStringMult = Localization.GetTranslation("ID_INCOME_MULT");
            string localizedStringScore = Localization.GetTranslation("ID_MODERN_SCORE");
            this.properties.textIncome.text = string.Format(localizedStringMult, modernizationInteractor.modernizationData.multiplierInPercent);
            this.properties.textScore.text = string.Format(localizedStringScore, modernizationInteractor.modernizationData.scores);
        }
    }
}