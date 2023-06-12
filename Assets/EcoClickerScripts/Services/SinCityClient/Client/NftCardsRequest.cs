using System.Collections.Generic;
using Newtonsoft.Json;
using SinSity.Core;
using SinSity.Data;
using SinSity.Domain;
using VavilichevGD.Tools;

namespace EcoClickerScripts.Services {
    public sealed class NftCardsRequest : SinCityRequest {
        protected override object data { get; set; }
        
        public NftCardsRequest() {
            /*var user = User.GetInstance();
            data = new BaseGetRequestPDO {
                login = user.encryptedLogin,
                command = "GET"
            };
            url = "urlGetNftCards";*/
        }
        
        public NftCardsRequest(ICard card) {
            /*var user = User.GetInstance();
            data = new SetNftCardRequestPDO {
                login = user.encryptedLogin,
                command = "SET",
                cardId = card.id,
                amount = card.amount
            };
            url = "urlSetNftCards";*/
        }

        public CardsSavedStates GetOwnedNftCardsAsSavedStates() {
            var decryptedSting = SinCityEncryptor.Decrypt(downloadedData);
            var ownedCards = JsonConvert.DeserializeObject<List<CardSavedData>>(decryptedSting);
            return ownedCards is null ? null : new CardsSavedStates(ownedCards);
        }
    }

    public struct SetNftCardRequestPDO {
        public string login;
        public string command;
        public string cardId;
        public int amount;
    }
}