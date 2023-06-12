using System;
using SinSity.Domain;
using SinSity.UI;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD.Architecture;
using VavilichevGD.Audio;
using VavilichevGD.UI;

public class UIPopupCardsCollection : UIDialogue<UIPopupCardsCollectionProperties, UIPopupArgs> {
    protected override void OnGameInitialized() {
        base.OnGameInitialized();
        var charactersInteractor = Game.GetInteractor<CharactersInteractor>();
        var characters = charactersInteractor.GetAllCharacters();
        foreach (var character in characters) {
            var widget = Instantiate(properties.widgetCharacterCard, properties.cardsContainer);
            widget.Setup(character);
        }
    }
    
    private void OnEnable() {
        properties.btnClose.onClick.AddListener(OnCloseBtnClick);
    }
    
    private void OnDisable() {
        properties.btnClose.onClick.RemoveListener(OnCloseBtnClick);
    }

    private void OnCloseBtnClick() {
        NotifyAboutResults(new UIPopupArgs(this, UIPopupResult.Close));
        Hide();
        SFX.PlaySFX(properties.audioClipClose);
    }
}

[Serializable]
public class UIPopupCardsCollectionProperties : UIProperties {
    [SerializeField] private WidgetCharacterCard _widgetCharacterCard;
    [SerializeField] private Transform _cardsContainer;
    [SerializeField] private Button _btnClose;
    [SerializeField] private AudioClip _audioClipClose;

    public WidgetCharacterCard widgetCharacterCard => _widgetCharacterCard;
    public Transform cardsContainer => _cardsContainer;
    public Button btnClose => _btnClose;
    public AudioClip audioClipClose => _audioClipClose;
}
