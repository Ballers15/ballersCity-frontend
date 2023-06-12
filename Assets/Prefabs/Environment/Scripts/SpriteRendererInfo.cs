using UnityEngine;

namespace SinSity.Core {
    public struct SpriteRendererInfo {
        public SpriteRenderer spriteRenderer;
        public Color colorDefault;
        public Color colorTransparent;

        public SpriteRendererInfo(SpriteRenderer spriteRenderer, Color colorDefault, Color colorTransparent) {
            this.spriteRenderer = spriteRenderer;
            this.colorDefault = colorDefault;
            this.colorTransparent = colorTransparent;
        }

        public void SetColor(Color color) {
            spriteRenderer.color = color;
        }
    }
}