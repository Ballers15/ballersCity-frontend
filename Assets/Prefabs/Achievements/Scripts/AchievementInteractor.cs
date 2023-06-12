using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VavilichevGD.Architecture;

namespace SinSity.Achievements
{
    public sealed class AchievementInteractor : Interactor
    {
        #region CONST

        private const string CONFIG_PATH = "Config/Achievements";

        private const string ACHIEVEMENTS_FOLDER_PATH = "Achievements";

        #endregion
        
        #region EVENTS

        //TODO: Make Achievements events

        #endregion
        
        private Dictionary<string, AchievementEntity> _achievementsList;
        private AchievementInfo[] _achievementInfo;
        private AchievementRepository _repository;
        
        private AchievementProvider _provider;
        //TODO: Achievements config
        
        public override bool onCreateInstantly { get; } = true;
        
        #region INITIALIZE

        protected override void Initialize() {
            base.Initialize();
            
#if UNITY_ANDROID     
            _achievementsList = new Dictionary<string, AchievementEntity>();
            _repository = this.GetRepository<AchievementRepository>();
            _achievementInfo = Resources.LoadAll<AchievementInfo>(ACHIEVEMENTS_FOLDER_PATH);
            _provider = new AchievementProviderGoogle();
            Achievement.Initialize(this);
           // _achievementInfoList = allAchievementsInfo.ToDictionary(it => it.id);

           LoadFromStorage();
           
           if (_achievementsList.Count != _achievementInfo.Length)
           {
               CreateNewAchievementsState();
               SaveAllAchievements();
           }

           InitializeAllObservers();
           Resources.UnloadUnusedAssets();
#endif
        }
        
        #endregion

        private void CreateNewAchievementsState()
        {
            foreach (var info in _achievementInfo)
            {
                if(_achievementsList.ContainsKey(info.id)) continue;
                var state = info.CreateStateDefault();
                var achievement = new AchievementEntity(info, state);
                _achievementsList.Add(info.id, achievement);
            }
        }

        private void InitializeAllObservers()
        {
            foreach (var achievement in _achievementsList)
            {
                if(achievement.Value.isCompleted) continue;
                achievement.Value.StartObserve();
            }
        }

        #region STORAGE

        private void LoadFromStorage()
        {
            string[] stateJsons = _repository.stateJsons;
            
            foreach (var stateJson in stateJsons) {
                var state = JsonUtility.FromJson<AchievementState>(stateJson);

                foreach (var info in _achievementInfo) {
                    if (info.id == state.id) {
                        var specialState = info.CreateState(stateJson);
                        var achievement = new AchievementEntity(info, specialState);
                        _achievementsList.Add(info.id, achievement);
                        break;
                    }
                }
            }
        }
        
        public void SaveAllAchievements() {
            var states  = new List<AchievementState>();
            foreach (var achievement in _achievementsList.Values)
                states.Add(achievement.state);
            
            _repository.Save(states.ToArray());
        }

        #endregion
        
        #region PROVIDER
        
        public void ShowAchievement(AchievementEntity achievement)
        {
            _provider.ShowAchievement(achievement);
        }

        public void CompleteAchievement(AchievementEntity achievement)
        {
            _provider.CompleteAchievement(achievement);
        }

        public void ShowAchievementsList()
        {
            _provider.ShowAchievementsList();
        }
        #endregion
    }
}