using VavilichevGD.Audio;
using VavilichevGD.UI;
using SinSity.Domain;
using VavilichevGD.Architecture;
using VavilichevGD.LocalizationFramework;

namespace SinSity.UI
{
    public class UIWidgetModernizationPop : UIWidget<UIWidgetModerizationPopProperties>
    {
        private ModernizationInteractor modernizationInteractor;

        private void OnEnable()
        {
            this.modernizationInteractor = Game.GetInteractor<ModernizationInteractor>();
            this.modernizationInteractor.OnModernizationDataStateChanged += this.OnModernizationDataStateChanged;
            Localization.OnLanguageChanged += OnLanguageChanged;
            this.Refresh();
        }

        private void OnModernizationDataStateChanged(ModernizationInteractor interactor, object sender)
        {
            this.Refresh();
        }

        private void Refresh()
        {
            this.properties.textIncome.text = modernizationInteractor.modernizationData.multiplierInPercent.ToString() + '%';
            this.properties.textScore.text = modernizationInteractor.modernizationData.scores.ToString();
            this.properties.textFutureIncome.text =  $"{modernizationInteractor.modernizationData.multiplierInPercent + modernizationInteractor.modernizationData.scores}%";
        }

        private void OnLanguageChanged()
        {
            UpdateDescription();
        }

        private void UpdateDescription()
        {
            /*string localizationTaskText = Localization.GetTranslation("ID_QUESTS_RENO_DESCR");
            this.properties.textTask.text = string.Format(localizationTaskText, targetQuestsCount);*/
        }

        private void OnDisable()
        {
            this.modernizationInteractor.OnModernizationDataStateChanged -= this.OnModernizationDataStateChanged;
            Localization.OnLanguageChanged -= OnLanguageChanged;
        }
    }
}