using Spine;
using Spine.Unity;
using UnityEngine;
using Event = Spine.Event;

namespace SinSity.UI {
    public class UIGemTreeAnimator : MonoBehaviour {

        [SerializeField] private SkeletonGraphic skeletonGraphic;


        private void OnEnable() {
            //skeletonGraphic.AnimationState.Event += OnEventHandled;
        }

        private void OnEventHandled(TrackEntry trackentry, Event e) {
            Debug.Log("WTFFFFFFFFFFFFFFFFFFFFFFF " + e.Data.Name);
        }

        private void Handle_OnGrowingComplete() {
            Debug.Log("WTFFFFFFFFFFFFFFFFFFFFFFF");
        }

        private void OnDisable() {
            if (skeletonGraphic == null)
                return;

            //skeletonGraphic.AnimationState.Event -= OnEventHandled;
        }
    }
}