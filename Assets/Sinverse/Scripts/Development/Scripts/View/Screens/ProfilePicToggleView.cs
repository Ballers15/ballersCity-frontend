using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePicToggleView : MonoBehaviour
{
    [SerializeField] private Image picHolder;

    private Toggle myToggle;
    private int avatarIndex;

    private void Awake()
    {
        myToggle = this.GetComponent<Toggle>();
        myToggle.onValueChanged.AddListener((bool on) => UpdatePlayerSprite(on));
    }

    public void SetData(Sprite sprite, int index, ToggleGroup toggleGroup)
    {
        picHolder.sprite = sprite;
        avatarIndex = index;
        myToggle.group = toggleGroup;
    }

    private void UpdatePlayerSprite(bool on)
    {
        if (!on) return;
     
        PlayerData.SaveSprite(picHolder.sprite, avatarIndex);
    }
}
