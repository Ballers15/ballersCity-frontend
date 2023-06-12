using System.Collections;
using Orego.Util;
using SinSity.Domain;
using SinSity.Services;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Core.Content.UpgradePerson
{
    public class UIHintInspectorReceivePersonForUpgrade : UIHintCertainInspector<HintInspectorReceivePersonForUpgrade>
    {
        [SerializeField]
        private UIHintReceivePersonForUpgrade uiHintReceivePerson;

        //TODO: OLD CARDS SYSTEM DELETED
        /*private void Awake()
        {
            this.uiHintReceivePerson.SetInvisible();
        }

        public override void OnInitialized()
        {
            base.OnInitialized();
            if (this.hintInspector.IsViewed())
            {
                return;
            }

            BluePrint.OnBluePrintStateChanged += this.OnBlueprntStateChanged;
        }

        private void OnBlueprntStateChanged(bool isActive)
        {
            if (!isActive)
            {
                return;
            }
            
            var cardObjectUpgradeInteractor = Game.GetInteractor<CardsInteractor>();
            bool anyCardReadyForUpgrade = cardObjectUpgradeInteractor.HasAnyCardReadyForUpgrade();

            if (anyCardReadyForUpgrade && !this.hintInspector.IsViewed())
            {
                this.hintInspector.NotifyAboutPersonReceivedViewed();
                this.uiHintReceivePerson.SetVisible();
                StartCoroutine(InitializeRoutine());
                TutorialAnalytics.LogEventInMin("hint_second_staff_hint");
            }
        }
        
        private IEnumerator InitializeRoutine() {
            yield return new WaitForSeconds(1f);
            this.uiHintReceivePerson.OnClickedEvent.AddListener(() =>
            {
                this.uiHintReceivePerson.SetInvisible();
                TutorialAnalytics.LogEventInMin("hint_second_staff_hint_close");
            });
        }*/
    }
}