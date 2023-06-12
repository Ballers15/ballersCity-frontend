using SinSity.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI {
    public class UIPanelNotificationLevelRaiseIncome : UIPanelNotificationLevelRaise {
        [SerializeField] private Text textIncomeMultiplicatorValue;

        public override void Setup(IdleObject idleObject) {
            base.Setup(idleObject);
            
            IdleObjectState state = idleObject.state;
            textIncomeMultiplicatorValue.text = $"x{state.localMultiplicatorDynamic}";
        }
    }
}