using DG.Tweening;
using Ediiie.Screens;
using UnityEngine;
using System.Collections;

public class SimpleAutoHidePopup : GameScreen
{
    protected virtual void OnEnable()
    {
        StartCoroutine(AutoHideTimer());
    }

    private IEnumerator AutoHideTimer()
    {
        yield return new WaitForSeconds(1f);
        Show(false);
    }

    protected void Show(bool on)
    {
        this.gameObject.SetActive(on);
    }
}
