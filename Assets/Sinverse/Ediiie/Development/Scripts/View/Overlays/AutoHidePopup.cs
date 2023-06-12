using DG.Tweening;
using Ediiie.Screens;
using UnityEngine;

public class AutoHidePopup : GameUI
{
    [SerializeField] private float time = 2f;

    protected virtual void OnEnable()
    {
        PlayAnimation();
    }

    private void PlayAnimation()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
        {
            AfterPopupVisible();
            Invoke("Deactivate", time);
        });
    }

    public void Deactivate()
    {
        transform.DOScale(transform.localScale, 0.8f).OnComplete(() =>
        {
            transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
            {
                this.gameObject.SetActive(false);
            });
        });
    }

    protected virtual void AfterPopupVisible() { }
}
