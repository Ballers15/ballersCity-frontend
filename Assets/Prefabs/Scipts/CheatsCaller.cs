using System;
using System.Collections;
using System.Collections.Generic;
using SinSity.UI;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.UI;

public class CheatsCaller : MonoBehaviour
{
    private void FixedUpdate() {
        OnKeyUp(KeyCode.BackQuote, () => {
            var uiInteractor = Game.GetInteractor<UIInteractor>();
            var uiController = uiInteractor.uiController;
            uiController.Show<UIPopupCheats>();
        });
    }

    private static void OnKeyUp(KeyCode keyCode, Action action) {
        if (Input.GetKeyUp(keyCode)) {
            action?.Invoke();
        }
    }
}
