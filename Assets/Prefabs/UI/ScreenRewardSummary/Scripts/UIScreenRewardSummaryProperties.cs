using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Tools;

namespace SinSity.UI
{
    [Serializable]
    public sealed class UIScreenRewardSummaryProperties : UIProperties
    {
        [SerializeField] private UICardRewardSummary m_cardRewardSummaryPref;
        [SerializeField] private Button m_buttonOk;
        [SerializeField] private Button m_buttonOpenNext;
        [SerializeField] private Transform m_cardLayout;
        [SerializeField] private Button m_btnHurryUp;
        [SerializeField] public AudioClip audioClipOpenNextClick;
        
        
        public UICardRewardSummary cardRewardSummaryPref => this.m_cardRewardSummaryPref;
        public Button buttonOk => this.m_buttonOk;
        public Button buttonOpenNext => this.m_buttonOpenNext;
        public Transform cardLayout => this.m_cardLayout;
        public Button btnHurryUp => this.m_btnHurryUp;

        private Coroutine activateBtnsRoutine;

        public void ActivateBtns() {
            this.StopAllCoroutines();
            this.activateBtnsRoutine = Coroutines.StartRoutine(ActivateBtnsRoutine());
        }
        
        public void DeactivateBtns() {
            buttonOk.gameObject.SetActive(false);
            buttonOpenNext.gameObject.SetActive(false);
        }

        public void StopAllCoroutines() {
            if (this.activateBtnsRoutine != null) {
                Coroutines.StopRoutine(this.activateBtnsRoutine);
                this.activateBtnsRoutine = null;
            }
        }

        private IEnumerator ActivateBtnsRoutine() {
            buttonOpenNext.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(0.3f);
            buttonOk.gameObject.SetActive(true);
        }
    }
}