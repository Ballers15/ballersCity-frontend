using System.Collections;
using SinSity.Domain;
using UnityEngine;

namespace SinSity.Core
{
    public class CleanSlot : MonoBehaviour
    {
        [SerializeField]
        private string m_idleObjectId = "io_";

        [SerializeField]
        private string m_zoneName = "Zone";

        [Space]
        [SerializeField]
        private SpriteRenderer[] spriteRenderersFading;

        [SerializeField]
        private float fadeDuration;

        public string idleObjectId => m_idleObjectId;
        public string zoneName => m_zoneName;
        public bool isActive => this.gameObject.activeInHierarchy;

        private SpriteRendererInfo[] spriteRenderersInfo;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            spriteRenderersInfo = new SpriteRendererInfo[spriteRenderersFading.Length];
            int count = spriteRenderersFading.Length;
            for (int i = 0; i < count; i++)
            {
                SpriteRenderer spriteRenderer = spriteRenderersFading[i];
                spriteRenderersInfo[i].spriteRenderer = spriteRenderer;
                spriteRenderersInfo[i].colorDefault = spriteRenderer.color;
                Color colorTransparent = spriteRenderer.color;
                colorTransparent.a = 0f;
                spriteRenderersInfo[i].colorTransparent = colorTransparent;
            }
        }

        public void ShowCreatingWithAnimation()
        {
            this.StartCoroutine(this.ShowCreatingAnimationRoutine());
        }

        private IEnumerator ShowCreatingAnimationRoutine()
        {
            var timer = 0f;
            while (timer < 1f)
            {
                timer = Mathf.Min(timer + Time.deltaTime / this.fadeDuration, 1f);
                foreach (var spriteRendererInfo in this.spriteRenderersInfo)
                {
                    var color = Color.Lerp(spriteRendererInfo.colorTransparent, spriteRendererInfo.colorDefault, timer);
                    spriteRendererInfo.SetColor(color);
                }

                yield return null;
            }
        }

        public void ShowCreatingWithoutAnimations()
        {
            foreach (var spriteRendererInfo in this.spriteRenderersInfo)
            {
                var color = spriteRendererInfo.colorDefault;
                spriteRendererInfo.SetColor(color);
            }
        }

        public IEnumerator DestroyWithoutAnimations()
        {
            Destroy(this.gameObject);
            yield return new WaitForEndOfFrame();
        }

        public void Activate(bool instantly = false) {
            this.gameObject.SetActive(true);
            if (!instantly)
                this.ShowCreatingWithAnimation();
        }

        public void Deactivate() {
            this.gameObject.SetActive(false);
        }
    }
}