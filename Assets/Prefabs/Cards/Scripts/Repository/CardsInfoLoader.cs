using System.Linq;
using SinSity.Core;
using SinSity.Scripts;

namespace SinSity.Repo {
    public class CardsInfoLoader : InfoLoader {
        #region Const

        private const string CARDS_FOLDER_PATH = "SinSityCards";

        #endregion

        public override string infosPath => CARDS_FOLDER_PATH;

        public ICardInfo[] Load() {
            var infos = LoadAs<CardInfo>();
            var castedInfos = infos.Cast<ICardInfo>();
            return castedInfos.ToArray();
        }
    }
}