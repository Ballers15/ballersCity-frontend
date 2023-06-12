using System;
using SinSity.Core;
using IdleClicker.UI;
using UnityEngine;
using UnityEngine.UI;


namespace SinSity.UI {
	public class ProgressBarCollecting : ProgressBarMasked {

		[SerializeField] private Image imgFiller;
		[SerializeField] private Sprite defaultFiller;
		[SerializeField] private float speed = -0.15f;

		private const string SPRITE_FILLER_DEFAULT_PATH = "SpriteFillerDefault";
		private const string SPRITE_FILLER_TOOFAST_PATH = "SpriteFillerTooFast";
		private const string MATERIAL_FILLER_TOOFAST_PATH = "MaterialFillerTooFast";

		private static Sprite spriteFillerDefault;
		private static Sprite spriteFillerTooFast;
		private static Material materialFillerTooFast;

		private bool fillerTooFastModeEnabled => imgFiller.material == materialFillerTooFast;

		private void Awake() {
			Initialize();
		}

		private void Initialize() {
			/*if (!spriteFillerDefault)
				spriteFillerDefault = Resources.Load<Sprite>(SPRITE_FILLER_DEFAULT_PATH);*/
			if (!spriteFillerTooFast)
				spriteFillerTooFast = Resources.Load<Sprite>(SPRITE_FILLER_TOOFAST_PATH);
			if (!materialFillerTooFast)
				materialFillerTooFast = Resources.Load<Material>(MATERIAL_FILLER_TOOFAST_PATH);
		}

		public override void SetValue(float newNormalizedValue) {
			if (fillerTooFastModeEnabled)
				return;

			base.SetValue(newNormalizedValue);
		}

		public void ActivateFillerDefault() {
			imgFiller.material = null;
			imgFiller.sprite = defaultFiller;
			CycleProgressBarTooFastAnimation animations = imgFiller.GetComponent<CycleProgressBarTooFastAnimation>();
			if (animations)
				Destroy(animations);
		}

		public void ActivateTooFastFiller() {
			imgFiller.material = materialFillerTooFast;
			imgFiller.sprite = spriteFillerTooFast;
			CycleProgressBarTooFastAnimation animations = imgFiller.GetComponent<CycleProgressBarTooFastAnimation>();
			if (!animations) {
				animations = imgFiller.gameObject.AddComponent<CycleProgressBarTooFastAnimation>();
				animations.speed = speed;
			}
		}
	}
}