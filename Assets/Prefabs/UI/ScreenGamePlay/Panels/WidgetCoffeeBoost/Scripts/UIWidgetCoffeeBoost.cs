using System;
using System.Collections;
using System.Security.Cryptography;
using SinSity.Services;
using IdleClicker.Gameplay;
using Orego.Util;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Audio;
using VavilichevGD.UI;
using Random = System.Random;

namespace Prefabs.UI.ScreenGamePlay.Panels.WidgetCoffeeBoost {
    public class UIWidgetCoffeeBoost : UIWidgetAnim<UIWidgetCoffeeBoost.Properties> {

        #region DELEGATES

        public delegate void UIWidgetCoffeeBoostHandler(UIWidgetCoffeeBoost widget);
        public event UIWidgetCoffeeBoostHandler OnClickedEvent;

        #endregion

        private Coroutine visibleTimeRoutine;

        
        #region MONO

        protected void OnEnable() {
            this.properties.buttonActivate.AddListener(this.OnClick);
            this.visibleTimeRoutine = this.StartCoroutine(this.VisibleTimeRoutine());
        }

        protected void OnDisable() {
            this.properties.buttonActivate.RemoveListener(this.OnClick);
        }

        #endregion

        public void Setup(GameObject cupPrefab) {
            CleanCupsContainer();

            Instantiate(cupPrefab, properties.widgetCupContainer);
        }

        private void CleanCupsContainer() {
            var childCount = properties.widgetCupContainer.childCount;
            for (int i = 0; i < childCount; i++)
                Destroy(properties.widgetCupContainer.GetChild(i).gameObject);
        }


        protected override void OnPostShow() {
            SFX.PlaySFX(this.properties.sfxAppear);
            var interactor = GetInteractor<CoffeeBoostInteractor>();
            CoffeeBoostAnalytics.LogCoffeeBoostShown(interactor.coffeeBoost.level);
        }


        private IEnumerator VisibleTimeRoutine() {
            yield return new WaitForSecondsRealtime(this.properties.visibleLifeTime);
            this.Hide();
        }

        public override void Hide() {
            if (this.visibleTimeRoutine != null) {
                this.StopCoroutine(this.visibleTimeRoutine);
                this.visibleTimeRoutine = null;
            }
            
            base.Hide();
        }

        #region EVENTS

        private void OnClick() {
            this.OnClickedEvent?.Invoke(this);
            this.Hide();
        }

        #endregion
        
        
        [Serializable]
        public class Properties : UIProperties {
            public Button buttonActivate;
            public float visibleLifeTime = 5f;
            public AudioClip sfxAppear;
            public Transform widgetCupContainer;
        }
    }
}