using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SinSity.Achievements
{
    public abstract class AchievementInfo : ScriptableObject
    {
        [SerializeField] protected string m_id;
        [SerializeField] protected string m_name;
        [SerializeField] protected string m_descr;
        [SerializeField] protected bool m_isHidden;
        [SerializeField] protected Image m_icon;
        
        public string id => m_id;
        public bool isHidden => m_isHidden;
        public virtual string GetName()
        {
            return m_name;
        }
        
        public virtual string GetDescription()
        {
            return m_descr;
        }
        
        public virtual Image GetImage()
        {
            return m_icon;
        }
        
        public virtual AchievementState CreateState(string stateJson)
        {
            return new AchievementState(stateJson);
        }
        
        public abstract AchievementObserver CreateObserver(AchievementEntity achievement);
        public abstract AchievementState CreateStateDefault();
    }
}


