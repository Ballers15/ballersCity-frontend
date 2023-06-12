using System.Collections;
using UnityEngine;

namespace SinSity.Core {
    public class NotBuildedSlot : MonoBehaviour {
        [SerializeField] private string m_idleObjectId;

        [SerializeField] private string m_zoneName;

        [Space] [SerializeField] private SpriteRenderer[] spriteRenderersFading;

        [SerializeField] private float fadeDuration = 1f;

        public string idleObjectId => m_idleObjectId;

        public string zoneName => m_zoneName;


        private SpriteRendererInfo[] spriteRenderersInfo;


        private void Awake() {
            LateInitialize();
        }

        private void LateInitialize() {
            int count = spriteRenderersFading.Length;
            spriteRenderersInfo = new SpriteRendererInfo[count];
            for (int i = 0; i < count; i++) {
                SpriteRenderer sr = spriteRenderersFading[i];
                Color colorDefault = sr.color;
                Color colorTransparent = colorDefault;
                colorTransparent.a = 0f;

                SpriteRendererInfo info = new SpriteRendererInfo(sr, colorDefault, colorTransparent);
                spriteRenderersInfo[i] = info;
            }
        }

        #region Destroy

        public void DestroyWithAnimation() {
            this.StartCoroutine(this.DestroyWithAnimationRoutine());
        }

        private IEnumerator DestroyWithAnimationRoutine() {
            var timer = 0f;
            while (timer < 1f) {
                timer = Mathf.Min(timer + Time.deltaTime / this.fadeDuration, 1f);
                foreach (var info in this.spriteRenderersInfo) {
                    var color = Color.Lerp(info.colorDefault, info.colorTransparent, timer);
                    info.SetColor(color);
                }

                yield return null;
            }

            Destroy(this.gameObject);
        }

        #endregion
    }
}