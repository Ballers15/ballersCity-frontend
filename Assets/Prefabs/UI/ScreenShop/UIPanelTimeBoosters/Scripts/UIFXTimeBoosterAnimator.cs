using UnityEngine;
using VavilichevGD;

namespace SinSity.UI {
    public class UIFXTimeBoosterAnimator : AnimObject {

        [SerializeField] private GameObject rootGO;

        private void Handle_AnimationIsOver() {
            rootGO.SetActive(false);
        }
    }
}