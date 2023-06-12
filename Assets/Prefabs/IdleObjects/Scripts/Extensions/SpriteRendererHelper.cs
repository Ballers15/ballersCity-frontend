using UnityEngine;

namespace SinSity.Extensions {
    public class SpriteRendererHelper : MonoBehaviour {

        [SerializeField] private int addOrderValue = 100;
        [SerializeField] private Transform transformRoot;

        [ContextMenu("Apply")]
        public void AddOrder() {
            SpriteRenderer[] spriteRenderers = transformRoot.GetComponentsInChildren<SpriteRenderer>(true);
            foreach (SpriteRenderer spriteRenderer in spriteRenderers) {
                spriteRenderer.sortingOrder += addOrderValue;
            }
        }
    }
}