using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SinSity.Core;
using SinSity.Data;
using SinSity.Repo;
using UnityEngine;
using VavilichevGD.Meta;
using VavilichevGD.Monetization;
using VavilichevGD.Tools;
using VavilichevGD.UI;

namespace EcoClickerScripts.Services.SinCityClient {
    public static class FakeServerResponse {
        public static string GetFakeServerResponse(string url) {
            switch (url) {
                case "getBankDataUrl":
                    return GetFakeData<BankCurrencyData>("BANK_REPOSITORY_DATA",new BankCurrencyData());
                case "getDailyRewardDataUrl":
                    return GetFakeData<DailyRewardsData>("DAILY_REWARD_DATA", DailyRewardsData.defaultValue);
                case "getDailyRewardFromCharactersDataUrl":
                    return GetFakeData<DailyRewardsFromCharactersData>("DAILY_REWARDS_FROM_CHARACTERS_DATA", new DailyRewardsFromCharactersData(GameTime.now));
                case "getGameTimeDataUrl1":
                    return GetFakeData<GameSessionTimeData>("PREF_KEY_GAME_TIME_DATA", new GameSessionTimeData());
                case "getIdleObjectsDataUrl":
                    return GetFakeData<IdleObjectStates>("IDLE_OBJECT_STATES", new IdleObjectStates());
                case "getMainQuestDataUrl1":
                    return GetFakeData<MainQuestStatisitcs>("MAIN_QUEST_STATE", new MainQuestStatisitcs());
                case "getMainQuestDataUrl2":
                    return GetFakeData<MainQuestData>("MAIN_QUEST_DATA", new MainQuestData());
                case "urlGetNftCards":
                    return GetOwnedNftCards();
                case "getGameTimeDataUrl2":
                    return GetFakeGameTimeData2();
                case "getMiniQuestsDataUrl1":
                    return GetFakeData<MiniQuestData>("DAILY_QUEST_DATA", null);
                case "getMiniQuestsDataUrl2":
                    return GetFakeData<MiniQuestStatistics>("DAILY_QUEST_STATES", new MiniQuestStatistics());
                case "getProfileExperienceDataUrl":
                    return GetFakeData<ProfileExperienceData>("PROFILE_LEVEL_PREF_KEY", null);
                case "getQuestsDataUrl":
                    return GetFakeData<QuestStates>("QUEST_STATES", QuestStates.empty);
                case "getShopDataUrl":
                    return GetFakeData<ProductStates>("PRODUCTS_STATES", ProductStates.empty);
                case "getTutorialDataUrl":
                    return GetFakeData<TutorialStatistics>("TUTORIAL_STATE", null);
                case "getTutorialUiDataUrl":
                    return GetFakeData<UITutorialState>("REPOSITORY_TUTORIAL_STATE", UITutorialState.GetDefault());
                case "urlGetCryptoCurrency":
                    return GetFakeCryptoAmount();
                default:
                    return "";
            }
        }

        private static string GetOwnedNftCards() {
            var cardsStates = Storage.GetCustom<CardsSavedStates>("CARDS_SAVE_DATA", null);
            if(cardsStates == null) return SinCityEncryptor.GetEncryptedJsonIEnumerable(null);
            var cardsData = cardsStates.GetAllSavedData();
            var ownedCards = cardsData.Where(it => it.amount > 0).ToList();
            return SinCityEncryptor.GetEncryptedJsonIEnumerable(ownedCards);
        }

        private static string GetFakeCryptoAmount() {
            var bankData = Storage.GetCustom<BankCurrencyData>("BANK_REPOSITORY_DATA", new BankCurrencyData());
            var data = bankData.cryptoCurrencyCount;
            return SinCityEncryptor.Encrypt(data.ToString());
        }

        private static string GetFakeGameTimeData2() {
            var data = Storage.GetDateTime("PREF_KEY_FIRST_PLAY_TIME", DateTime.Now).Value;
            return SinCityEncryptor.GetEncryptedJson(data);
        }

        private static string GetFakeData<T>(string key, object defaultValue) {
            var castedDefaultType = (T) defaultValue;
            var data = Storage.GetCustom(key, castedDefaultType);
            return SinCityEncryptor.GetEncryptedJson(data);
        }
    }
}