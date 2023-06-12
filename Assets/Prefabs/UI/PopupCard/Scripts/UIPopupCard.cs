using System;
using SinSity.Core;
using SinSity.Domain;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.UI;

public class UIPopupCard : UIDialogue<UIPopupCardProperties, UIPopupArgs> {
    [SerializeField] private Button btnGet;
    
    private ICard card;
    
    public void Setup(ICard card) {
        this.card = card;
    }

    private void OnEnable() {
        if(card == null) return;

        UpdateVisual();
        properties.btnClose.onClick.AddListener(OnCloseBtnClick);
        btnGet.onClick.AddListener(OnBtnGetCicked);
    }

    private void OnDisable() {
        properties.btnClose.onClick.RemoveListener(OnCloseBtnClick);
        btnGet.onClick.RemoveListener(OnBtnGetCicked);
    }
    
    private void UpdateVisual() {
        properties.imageCard.sprite = card.GetActiveSprite();
        properties.textName.text = card.GetName();
        properties.textDescription.text = card.GetDesription();
    }
    
    private void OnCloseBtnClick() {
        NotifyAboutResults(new UIPopupArgs(this, UIPopupResult.Close));
        Hide();
    }
    
    private void OnBtnGetCicked() {
        var cardsInteractor = GetInteractor<CardsInteractor>();
        cardsInteractor.IncreaseCardAmount(card.id, 1);
    }
}

[Serializable]
public class UIPopupCardProperties : UIProperties {
    [SerializeField] private Image _imageCard;
    [SerializeField] private Text _textName;
    [SerializeField] private Text _textDescription;
    [SerializeField] private Button _btnClose;

    public Image imageCard => _imageCard;
    public Text textName => _textName;
    public Text textDescription => _textDescription;
    public Button btnClose => _btnClose;
}
