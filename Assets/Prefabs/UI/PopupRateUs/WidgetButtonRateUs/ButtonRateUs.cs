using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonRateUs : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image highlightImage;
    [SerializeField] private Sprite disabledBackSprite;
    [SerializeField] private Sprite disabledHighlightSprite;
    [SerializeField] private Sprite activeBackSprite;
    [SerializeField] private Sprite activeHighlightSprite;
    [SerializeField] private Button m_button;

    public Button button => this.m_button;

    public void SetVisualActive()
    {
        this.backgroundImage.sprite = this.activeBackSprite;
        this.highlightImage.sprite = this.activeHighlightSprite;
    }

    public void SetVisualDisabled()
    {
        this.backgroundImage.sprite = this.disabledBackSprite;
        this.highlightImage.sprite = this.disabledHighlightSprite;
    }

    public void ResetVisual()
    {
        this.SetVisualDisabled();
    }
}
