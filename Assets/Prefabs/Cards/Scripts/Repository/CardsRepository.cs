using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EcoClickerScripts.Services;
using EcoClickerScripts.Services.SinCityClient;
using Newtonsoft.Json;
using SinSity.Data;
using VavilichevGD.Architecture;
using VavilichevGD.Tools;

namespace SinSity.Repo {
    public sealed class CardsRepository : Repository {
        private const string PREF_KEY = "CARDS_SAVE_DATA";

        private CardsSavedStates _cardsSavedStates;
        

        protected override IEnumerator InitializeRepositoryRoutine() {
            /*using var request = new NftCardsRequest();
            
            yield return request.Send();*/
            
            //_cardsSavedStates = request.GetOwnedNftCardsAsSavedStates();
            _cardsSavedStates = GetSavesStub();
            yield break;
        }

        private CardsSavedStates GetSavesStub() {
            var dataStub = GetDataStub();
            
            var decryptedSting = SinCityEncryptor.Decrypt(dataStub);
            var ownedCards = JsonConvert.DeserializeObject<List<CardSavedData>>(decryptedSting);
            return ownedCards is null ? null : new CardsSavedStates(ownedCards);
        }

        private string GetDataStub() {
            var cardsStates = Storage.GetCustom<CardsSavedStates>("CARDS_SAVE_DATA", null);
            if(cardsStates == null) return SinCityEncryptor.GetEncryptedJsonIEnumerable(null);
            var cardsData = cardsStates.GetAllSavedData();
            var ownedCards = cardsData.Where(it => it.amount > 0).ToList();
            return SinCityEncryptor.GetEncryptedJsonIEnumerable(ownedCards);
        }

        public CardsSavedStates GetSavedData() {
            return _cardsSavedStates;
        }
        
        /*public void Save(CardsSavedStates cardsSavedStates) {
            _cardsSavedStates = cardsSavedStates;
            Save();
        }

        public override void Save() {
            if (isSavingInProcess) return;
            Coroutines.StartRoutine(SaveAsync());
        }*/

        /*private IEnumerator SaveAsync() {
            isSavingInProcess = true;
            using var request = new GameDataRequest("saveCardsDataUrl", PREF_KEY, _cardsSavedStates);
            
            yield return request.Send();
            
            isSavingInProcess = false;
        }*/
    }
}