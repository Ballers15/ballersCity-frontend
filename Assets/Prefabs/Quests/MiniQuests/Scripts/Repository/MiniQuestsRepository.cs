using System.Collections;
using System.Collections.Generic;
using EcoClickerScripts.Services;
using EcoClickerScripts.Services.SinCityClient;
using Orego.Util;
using UnityEngine;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace SinSity.Repo
{
    public sealed class MiniQuestsRepository : Repository, IUpdateVersionRepository
    {
        #region Const

        private const string PREF_KEY_QUESTS = "DAILY_QUEST_STATES";
        private const string PREF_KEY_DATA = "DAILY_QUEST_DATA";

        #endregion

        private readonly IVersionUpdater<MiniQuestData> internalVersionUpdater;
        private readonly IVersionUpdater<MiniQuestData> externalVersionUpdater;
        private MiniQuestData miniQuestData;
        private readonly Dictionary<string, MiniQuestEntity> entityMap;
        
        public MiniQuestStatistics statistic;

        public MiniQuestsRepository() {
            entityMap = new Dictionary<string, MiniQuestEntity>();
            internalVersionUpdater = ScriptableObject.Instantiate(Resources.Load<InternalMiniQuestVersionUpdater>(
                "InternalMiniQuestVersionUpdater"
            ));
            externalVersionUpdater = ScriptableObject.Instantiate(Resources.Load<ExternalMiniQuestVersionUpdater>(
                "ExternalMiniQuestVersionUpdater"
            ));
        }

        #region Initialize

        protected override IEnumerator InitializeRepositoryRoutine() {
            using var request = new GameDataRequest(PREF_KEY_DATA);
            
            yield return request.Send(RequestType.GET);
            
            miniQuestData = request.GetGameData<MiniQuestData>(null);
            internalVersionUpdater.UpdateVersion(ref miniQuestData);
            
            yield return LoadQuests();
        }
        
        private IEnumerator LoadQuests() {
            using var request = new GameDataRequest(PREF_KEY_QUESTS);
            
            yield return request.Send(RequestType.GET);
            
            statistic = request.GetGameData(new MiniQuestStatistics());
            if (statistic.entities.Count != 3) {
                statistic.entities = new List<MiniQuestEntity>();
                Save();
            }
            else {
                var questEntities = statistic.entities;
                foreach (var entity in questEntities) {
                    var entityId = entity.id;
                    entityMap.Add(entityId, entity);
                }
            }
        }

        #endregion

        public bool GetIsUnlocked() {
            return miniQuestData.isUnlocked;
        }

        public void SetIsUnlocked(bool isUnlocked) {
            miniQuestData.isUnlocked = isUnlocked;
            Coroutines.StartRoutine(SaveQuestDataRoutine());
        }
        
        private IEnumerator SaveQuestDataRoutine() {
            using var request = new GameDataRequest(PREF_KEY_DATA, miniQuestData);
            yield return request.Send();
        }

        public List<MiniQuestEntity> GetEntities() {
            var activeQuests = new List<MiniQuestEntity>();
            foreach (var questEntity in entityMap.Values) {
                var clone = questEntity.Clone();
                activeQuests.Add(clone);
            }

            return activeQuests;
        }

        public void UpdateEntity(MiniQuestEntity questEntity) {
            var clone = questEntity.Clone();
            entityMap[questEntity.id] = clone;
            Coroutines.StartRoutine(SaveQuests());
        }

        public void DeleteEntity(string questId) {
            entityMap.Remove(questId);
            Coroutines.StartRoutine(SaveQuests());
        }

        private IEnumerator SaveQuests() {
            var statistics = new MiniQuestStatistics {
                lastResetTimeSerialized = statistic.lastResetTimeSerialized, resetWasUsed = statistic.resetWasUsed
            };
            var entities = statistics.entities;
            entities.AddRange(entityMap.Values);
            using var request = new GameDataRequest(PREF_KEY_QUESTS, statistics);

            yield return request.Send();
        }

        public IEnumerator OnUpdateVersion(Reference<bool> isUpdated) {
            var isVersionUpdated = externalVersionUpdater.UpdateVersion(ref miniQuestData);
            if (isVersionUpdated) {
                Coroutines.StartRoutine(SaveQuestDataRoutine());
            }

            isUpdated.value = isVersionUpdated;
            yield break;
        }
    }
}