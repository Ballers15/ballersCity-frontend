using SinSity;
using SinSity.Core;
using SinSity.Domain;
using UnityEngine;
using VavilichevGD.Architecture;

public class UIWidgetBtnNavigateUpgrade : UIWidgetBtnNavigate
{
    [SerializeField]
    private GameObject exclamationMarker;

    public delegate void ButtonBluePrintHandler();

    public static event ButtonBluePrintHandler OnClicked;

    public override bool IsPopupAlreadyOpened()
    {
        return BluePrint.bluePrintModeEnabled;
    }

    protected override void OpenPopup()
    {
        if (IsPopupAlreadyOpened())
            return;
        OnClicked?.Invoke();
    }

    public override void ClosePopup()
    {
        if (!IsPopupAlreadyOpened())
            return;
        OnClicked?.Invoke();
    }

    protected override void OnEnable()
    {
        base.OnEnable();


        if (Game.isInitialized)
        {
            SubscribeOnCardObjectsInteractor();
            UpdateExclamationMarkerState();
        }
        else
            Game.OnGameInitialized += OnGameInitialized;

        SubscribeOnDifferentEvents();
    }

    private void SubscribeOnDifferentEvents() {
        IdleObject.OnIdleObjectBuilt += OnIdleObjectBuilt;
        BluePrint.OnBluePrintStateChanged += OnBluePrintStateChanged;
    }

    private void OnIdleObjectBuilt(IdleObject idleobject, IdleObjectState state) {
        UpdateExclamationMarkerState();
    }
    
    private void OnBluePrintStateChanged(bool isactive) {
        if (!isactive)
            SetVisualAsHidden();
        else
            SetVisualAsOpened();
    }

    private void SubscribeOnCardObjectsInteractor() {

    }

    private void OnCardReadyForUpgradeEvent(object o, ICard cardObject) {
        SetActiveExclamationMarker(true);
    }

    private void SetActiveExclamationMarker(bool isActive) {
        exclamationMarker.SetActive(isActive);
    }

    private void OnCardUpgradedEvent(ICard obj) {
        UpdateExclamationMarkerState();
    }

    private void UpdateExclamationMarkerState() {
        SetActiveExclamationMarker(false);
        //var cardObjectDataInteractor = Game.GetInteractor<CardObjectUpgradeInteractor>();
        //var anyCardReadyForUpgrade = cardObjectDataInteractor.HasAnyCardReadyForUpgrade();
        //this.SetActiveExclamationMarker(anyCardReadyForUpgrade);
    }

    private void OnGameInitialized(Game game)
    {
        Game.OnGameInitialized -= OnGameInitialized;

        /*cardObjectCollectInteractor = Game.GetInteractor<CardObjectCollectInteractor>();
        cardObjectUpgradeInteractor = Game.GetInteractor<CardObjectUpgradeInteractor>();*/

        SubscribeOnCardObjectsInteractor();
        UpdateExclamationMarkerState();
    }


    protected override void OnDisable()
    {
        base.OnDisable();

        if (Game.isInitialized)
        {
            UnsubscribeOnEvents();
        }
    }

    private void UnsubscribeOnEvents() {
        IdleObject.OnIdleObjectBuilt -= OnIdleObjectBuilt;
        BluePrint.OnBluePrintStateChanged -= OnBluePrintStateChanged;
    }
}