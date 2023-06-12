using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class WidgetRateUsStars : MonoBehaviour
{
    #region EVENT'S
        public event Action OnValueChangedEvent;
    #endregion
    
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite disabledSprite;
    private StarToggle[] toggles;

    private void OnEnable()
    {
        this.toggles = this.toggleGroup.GetComponentsInChildren<StarToggle>();
        foreach (var toggleStar in this.toggles)
        {
            toggleStar.toggle.onValueChanged.AddListener(this.OnToggleChanged);
        }
    }

    private void OnDisable()
    {
        foreach (var toggleStar in this.toggles)
        {
            toggleStar.toggle.onValueChanged.RemoveListener(this.OnToggleChanged);
        }
    }

    private void OnToggleChanged(bool state)
    {
        this.OnValueChangedEvent?.Invoke();
        var isActiveTogglePassed = (!this.HasToggledStar());
        foreach (var toggleStar in this.toggles)
        {
            if (!toggleStar.toggle.isOn)
            {
                toggleStar.image.sprite = (isActiveTogglePassed) ? this.disabledSprite : this.activeSprite;
            }
            else
            {
                isActiveTogglePassed = true;
            }
        }
    }

    public int GetToggledStarsCount()
    {
        var isActiveTogglePassed = (!this.HasToggledStar());
        var count = 0;

        if (isActiveTogglePassed) return count;
        
        foreach (var toggleStar in this.toggles)
        {
            if (!toggleStar.toggle.isOn)
            {
                count++;
            }
            else
            {
                count++;
                break;
            }
        }
        
        return count;
    }

    public bool HasToggledStar()
    {
        return toggleGroup.AnyTogglesOn();
    }

    public void ResetStars()
    {
        this.toggleGroup.SetAllTogglesOff();
    }
}
