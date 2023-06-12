using System;
using UnityEngine;
using VavilichevGD.Utils;

namespace SinSity.Repo
{
    [Serializable]
    public sealed class ProfileExperienceData : ICloneable<ProfileExperienceData>
    {
        [SerializeField] 
        public int version;
        
        [SerializeField]
        public ulong currentExperience;
        
        public ProfileExperienceData Clone()
        {
            return new ProfileExperienceData
            {
                currentExperience = this.currentExperience,
                version = this.version
            };
        }
    }
}