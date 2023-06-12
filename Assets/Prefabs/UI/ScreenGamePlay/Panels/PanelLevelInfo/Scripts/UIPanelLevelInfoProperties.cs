using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SinSity.UI {
    [Serializable]
    public class UIPanelLevelInfoProperties : UIProperties {
        [SerializeField] private Text textLevelValue;
        [SerializeField] private ProgressBarMaskedWithTMPro progressBar;
        [SerializeField] private UIPanelLevelInfoAnimator m_animator;

        public UIPanelLevelInfoAnimator animator => m_animator;
        
        public bool visualSetuppedAsMax { get; private set; }
        
        public void SetProgress(float progressNotmalized, string progressTextValue) {
            this.progressBar.SetValue(progressNotmalized);
            this.progressBar.SetTextValue(progressTextValue);
        }

        public void SetLevelValue(int level) {
            this.textLevelValue.text = $"LEVEL {level}";
        }

        public void PlayBounce() {
            animator.PlayBounce();
        }

        public void PlayNextLevel() {
            animator.PlayNextLevel();
        }

        public void SetVisualAsMax(int maxLevelNumber) {
            this.textLevelValue.text = $"LEVEL {maxLevelNumber.ToString()}";
            this.progressBar.SetValue(1f);
            this.progressBar.SetTextValue("MAX");
            this.visualSetuppedAsMax = true;
        }
    }
}