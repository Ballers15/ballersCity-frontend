using System;
using SinSity.Tools;
using SinSity.Core;
using SinSity.Domain;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture;
using VavilichevGD.Audio;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;
using VavilichevGD.UI;

namespace SinSity.UI
{
    public class IdleObjectUI : MonoBehaviour
    {
        [SerializeField] private IdleObjectBtnBuild btnBuild;
        [SerializeField] private IdleObjectBtnUpgrade btnUpgrade;
        [SerializeField] private Text textPrice;
        [SerializeField] private Text textCaptionNotBuilt;
        [SerializeField] private Text textCaptionBuilt;
        [SerializeField] private Text textRank;
        [SerializeField] private GameObject panelStateGO;
        [Space]
        [SerializeField] private GameObject panelActiveStateInfo;
        [SerializeField] private Sounds sounds;

        private IdleObject idleObject;
        private string defaultPriceText;

        private void Awake() {
            defaultPriceText = textPrice.text;
            Initialize();
        }

        private void Initialize()
        {
            idleObject = gameObject.GetComponentInParent<IdleObject>();
            idleObject.OnInitialized += OnInitialized;
            idleObject.OnResetEvent += this.OnIdleObjectReset;
            BluePrint.OnBluePrintStateChanged += OnBluePrintStateChanged;
        }

        private void OnInitialized()
        {
            IdleObject.OnIdleObjectBuilt += IdleObjectOnIdleObjectBuilt;

            if (!idleObject.isBuilt)
                Bank.uiBank.OnStateChangedEvent += OnBankStateChanged;
            
            UpdateState();
            UpdatePrice();
        }

        private void IdleObjectOnIdleObjectBuilt(IdleObject idleObject, IdleObjectState state)
        {
            if (idleObject == this.idleObject)
            {
                UpdateState();
                Bank.uiBank.OnStateChangedEvent -= OnBankStateChanged;
            }
        }

        private void OnBankStateChanged(object sender)
        {
            if (!Bank.isInitialized)
                return;

            UpdateState();
        }

        private void UpdateState() {
            var bluePrintEnabled = BluePrint.bluePrintModeEnabled;
            var canBuildObject = !idleObject.isBuilt && !bluePrintEnabled;
            btnBuild.gameObject.SetActive(canBuildObject);
            btnUpgrade.gameObject.SetActive(idleObject.isBuilt && bluePrintEnabled);
            panelActiveStateInfo.SetActive(idleObject.isBuilt && !bluePrintEnabled);
            UpdateCaptionText();
            UpdateRankText();
        }

        private void UpdateCaptionText() {
            if (idleObject == null) return;
            
            var bluePrintEnabled = BluePrint.bluePrintModeEnabled;
            textCaptionBuilt.text = Localization.GetTranslation(idleObject.info.GetTitle());
            textCaptionBuilt.gameObject.SetActive(idleObject.isBuilt && !bluePrintEnabled);
            textCaptionNotBuilt.text = Localization.GetTranslation(idleObject.info.GetTitle());
            textCaptionNotBuilt.gameObject.SetActive(!idleObject.isBuilt || bluePrintEnabled);
        }
        
        private void UpdateRankText() {
            var bluePrintEnabled = BluePrint.bluePrintModeEnabled;
            textRank.gameObject.SetActive(idleObject.isBuilt && bluePrintEnabled);
            textRank.text = $"rank: {idleObject.levelImprovementBlock.blockLevel}";
        }

        private void OnDestroy()
        {
            this.idleObject.OnInitialized -= this.OnInitialized;
            this.idleObject.OnResetEvent -= this.OnIdleObjectReset;
            BluePrint.OnBluePrintStateChanged -= OnBluePrintStateChanged;
        }


        private void OnEnable()
        {
            btnBuild.AddListener(OnBuildBtnClick);
            btnUpgrade.OnButtonClicked += OnUpgradeClicked;
            Localization.OnLanguageChanged += OnLanguageChanged;
        }

        public void OnUpgradeClicked() {
            var uiInteractor = Game.GetInteractor<UIInteractor>();
            var popup = uiInteractor.GetUIElement<UIPopupUpgrade>();
            popup.Setup(this.idleObject);
            popup.Show();
        }

        private void OnLanguageChanged() {
            UpdatePrice();
        }

        private void UpdatePrice() {
            var dictionary = BigNumberLocalizator.GetSimpleDictionary();
            textPrice.text = string.Format(defaultPriceText,idleObject.info.priceToBuild.ToString(BigNumber.FORMAT_XXX_XC,dictionary));
        }

        private void OnBluePrintStateChanged(bool isActive)
        {
            UpdateState();
        }

        public void OnBuildBtnClick()
        {
            if (!Bank.isEnoughtSoftCurrency(idleObject.info.priceToBuild))
            {
                SFX.PlaySFX(this.sounds.m_audioClipNotEnoughCurrency);
                return;
            }

            var uiInteractor = Game.GetInteractor<UIInteractor>();
            var popup = uiInteractor.ShowElement<UIPopupBuild>();
            popup.Setup(this.idleObject);
            popup.OnDialogueResults += this.OnDialogueResults;
        }

        private void OnDialogueResults(UIPopupArgs e) {
            UIPopupBuild popup = e.uiElement as UIPopupBuild;
            popup.OnDialogueResults -= OnDialogueResults;
            
            if (e.result == UIPopupResult.Apply)
            {
                this.idleObject.Build();
                UpdateState();
                SFX.PlaySFX(this.sounds.m_audioClipBuild);
            }
        }


        private void OnIdleObjectReset()
        {
            this.UpdateState();
        }

        private void OnDisable()
        {
            btnBuild.RemoveListener(OnBuildBtnClick);
            btnUpgrade.OnButtonClicked -= OnUpgradeClicked;
            Localization.OnLanguageChanged -= OnLanguageChanged;
        }

        public void SetActive(bool isAcrive)
        {
            gameObject.SetActive(isAcrive);
        }

        [Serializable]
        public sealed class Sounds
        {
            [SerializeField] 
            public AudioClip m_audioClipNotEnoughCurrency;

            [SerializeField] 
            public AudioClip m_audioClipBuild;
        }
    }
}