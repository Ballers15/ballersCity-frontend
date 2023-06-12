using System;
using System.Collections.Generic;
using SinSity.Core;
using SinSity.Data;
using SinSity.Repo;
using UnityEngine;
using VavilichevGD.Meta;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;
using VavilichevGD.UI;

namespace EcoClickerScripts.Services.SinCityClient {
    public static class FakeServerRequestsProcessor {
        public static void ProcessSaveRequest(string url, string json) {
            switch (url) {
                case "saveBankDataUrl":
                    SaveData<BankCurrencyData>(json);
                    break;
                case "saveCardsDataUrl":
                    SaveData<CardsSavedStates>(json);
                    break;
                case "saveDailyRewardDataUrl":
                    SaveData<DailyRewardsData>(json);
                    break;
                case "saveDailyRewardFromCharactersDataUrl":
                    SaveData<DailyRewardsFromCharactersData>(json);
                    break;
                case "saveGameTimeDataUrl1":
                    SaveData<GameSessionTimeData>(json);
                    break;
                case "saveGameTimeDataUrl2":
                    SaveData<DateTime>(json);
                    break;
                case "saveIdleObjectsDataUrl":
                    SaveData<IdleObjectStates>(json);
                    break;
                case "saveMainQuestDataUrl1":
                    SaveData<MainQuestStatisitcs>(json);
                    break;
                case "saveMainQuestDataUrl2":
                    SaveData<MainQuestData>(json);
                    break;
                case "saveMiniQuestDataUrl1":
                    SaveData<MiniQuestData>(json);
                    break;
                case "saveMiniQuestDataUrl2":
                    SaveData<MiniQuestStatistics>(json);
                    break;
                case "saveProfileExperienceDataUrl":
                    SaveData<ProfileExperienceData>(json);
                    break;
                case "saveQuestsDataUrl":
                    SaveData<QuestStates>(json);
                    break;
                case "saveShopDataUrl":
                    SaveData<ProductStates>(json);
                    break;
                case "saveTutorialDataUrl":
                    SaveData<TutorialStatistics>(json);
                    break;
                case "saveTutorialUiDataUrl":
                    SaveData<UITutorialState>(json);
                    break;
                case "urlSetNftCards":
                    SaveNftCards(json);
                    break;
                default:
                    return;
            }
        }

        public static void SaveNftCards(string json) {
            var decryptedJson = SinCityEncryptor.Decrypt(json);
            var requestPdo = JsonUtility.FromJson<SetNftCardRequestPDO>(decryptedJson);
            var cardSaveData = new CardSavedData(requestPdo.cardId, requestPdo.amount);
            var cardsStates = Storage.GetCustom<CardsSavedStates>("CARDS_SAVE_DATA", null);
            CardsSavedStates savedStates = null;
            
            if (cardsStates == null) {
                savedStates = new CardsSavedStates(new[] {cardSaveData});
            }
            else {
                if (cardsStates.HasState(cardSaveData.id)) {
                    cardsStates.UpdateCardSaveData(cardSaveData.id, cardSaveData.amount);
                }
                else {
                    cardsStates.AddSaveData(cardSaveData);
                }
                
                savedStates = cardsStates.Clone();
            }
            
            Storage.SetCustom("CARDS_SAVE_DATA", savedStates);
        }

        private static void SaveData<T>(string json) {
            /*var decryptedJson = SinCityEncryptor.Decrypt(json);
            var requestDataPdo = JsonUtility.FromJson<SaveGameDataRequestPDO>(decryptedJson);
            var decryptedPrefKey = SinCityEncryptor.Decrypt(requestDataPdo.dataKey);
            var decryptedJsonData = SinCityEncryptor.Decrypt(requestDataPdo.data);
            var data = JsonUtility.FromJson<T>(decryptedJsonData);
            Storage.SetCustom(decryptedPrefKey, data);*/
        }
    }
}