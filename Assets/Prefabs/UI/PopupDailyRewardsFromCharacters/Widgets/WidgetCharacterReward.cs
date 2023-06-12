using System.Collections;
using System.Collections.Generic;
using SinSity.Core;
using SinSity.Scripts;
using SinSity.UI;
using UnityEngine;
using UnityEngine.UI;

public class WidgetCharacterReward : UISceneElement {
    [SerializeField] private WidgetCharacterCard widgetCharacterCard;
    [SerializeField] private Image imageReward;
    [SerializeField] private Text textFieldRewardAmount;

    private CharacterReward reward;

    public void Setup(CharacterReward reward) {
        this.reward = reward;
        widgetCharacterCard.Setup(reward.character);
    }

    protected override void OnElementEnable() {
        base.OnElementEnable();
        UpdateVisual();
    }

    private void UpdateVisual() {
        var rewardInfo = reward.rewardInfo;
        imageReward.sprite = rewardInfo.GetSpriteIcon();
        textFieldRewardAmount.text = rewardInfo.GetCountToString();
    }
}
