using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI {
    public class IdleObjectLevelPanel : MonoBehaviour {
        
        [SerializeField] private Text textLevelValue;

        public void SetValue(int level) {
            textLevelValue.text = level.ToString();
        }
    }
}