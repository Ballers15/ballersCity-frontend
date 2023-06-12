using SinSity.Meta;
using UnityEngine;

namespace SinSity.UI {
	public class IdleObjectLevelImprovementInfoPanel : MonoBehaviour {

		[SerializeField] private ProgressBarMasked progressBarMasked; 

		public void UpdateView(LevelImprovementBlock levelImprovementBlock, int level) {
			if (levelImprovementBlock == null) {
				progressBarMasked.SetValue(1f);
				return;
			}

			float progress = levelImprovementBlock.GetProgressNormalized(level);
			progressBarMasked.SetValue(progress);
		}
	}
}