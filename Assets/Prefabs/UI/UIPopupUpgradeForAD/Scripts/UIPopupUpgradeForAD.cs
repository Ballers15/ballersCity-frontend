using System;
using System.Collections;
using System.Collections.Generic;
using SinSity.Core;
using SinSity.UI;
using UnityEngine;
using VavilichevGD.Audio;
using VavilichevGD.LocalizationFramework;
using VavilichevGD.UI;

public class UIPopupUpgradeForAD : UIPopupAnim<UIPopupUpgradeForADProperties, UIPopupArgs>
{
    #region EVENTS

    public event Action OnGetButtonClickedEvent; 
    public event Action OnCloseClickEvent; 

    #endregion

    [SerializeField] private string descriptionTranslationKey;
    private void OnEnable()
    {
        this.properties.getButton.onClick.AddListener(this.OnButtonClick);
        this.properties.btnClose.onClick.AddListener(this.OnCloseClick);
    }

    private void OnButtonClick()
    {
        this.OnGetButtonClickedEvent?.Invoke();
    }
    
    private void OnCloseClick()
    {
        SFX.PlaySFX(this.properties.audioClipCloseClick);
        this.OnCloseClickEvent?.Invoke();
    }

    public void Setup(IdleObject idle, int levelsCount)
    {
        this.properties.idleObjectImage.sprite = idle.info.spriteIcon;
        var localizingString = "\""+Localization.GetTranslation(idle.info.titleCode)+"\"";
        var localizingDescr = Localization.GetTranslation(this.descriptionTranslationKey);
        this.properties.textDescription.text = String.Format(localizingDescr, levelsCount, localizingString);
    }
}
