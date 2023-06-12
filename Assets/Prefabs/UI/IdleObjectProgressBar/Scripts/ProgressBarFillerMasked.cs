using UnityEngine;
using UnityEngine.UI;

namespace SinSity.UI
{
    public class ProgressBarFillerMasked : ProgressBarMasked
    {
        [SerializeField] 
        protected Image m_imageFiller;
        
        public Image imageFiller
        {
            get { return this.m_imageFiller; }
        }
    }
}