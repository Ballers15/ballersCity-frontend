using SinSity.UI;
using UnityEngine;
using UnityEngine.UI;
using VavilichevGD;
using VavilichevGD.Architecture;
using VavilichevGD.Audio;
using VavilichevGD.UI;

public abstract class UIWidgetBtnNavigate : AnimObject
{
    #region Const

    protected static readonly int openedId = Animator.StringToHash(BOOL_OPENED);

    protected const string BOOL_OPENED = "opened";

    #endregion
    
    [SerializeField] private AudioClip audioClipClick;
    [SerializeField] protected Button btn;

    protected UIInteractor uiInteractor;

    protected UIScreenGamePlay uiScreenGameplay;
    protected Color colorDefault = Color.white;

    protected virtual void Awake()
    {
        uiInteractor = Game.GetInteractor<UIInteractor>();
        uiScreenGameplay = GetComponentInParent<UIScreenGamePlay>();
    }

    protected virtual void OnEnable()
    {
        btn.onClick.AddListener(OnClick);
    }

    public abstract bool IsPopupAlreadyOpened();

    protected abstract void OpenPopup();

    public abstract void ClosePopup();

    public void OnClick()
    {
        if (this.IsPopupAlreadyOpened())
        {
            this.ClosePopup();
        }
        else
        {
            this.OpenPopup();
        }

        SFX.PlaySFX(this.audioClipClick);
    }

    protected virtual void OnDisable()
    {
        this.btn.onClick.RemoveListener(OnClick);
    }

    protected override void Reset()
    {
        base.Reset();
        if (!this.btn)
        {
            this.btn = this.gameObject.GetComponentInChildren<Button>();
        }
    }

    protected virtual void SetVisualAsOpened()
    {
        this.uiScreenGameplay.HideAllBtnNavigatesExcept(this);
        this.SetBoolTrue(openedId);
    }

    protected virtual void SetVisualAsHidden()
    {
        this.SetBoolFalse(openedId);
    }

   
}