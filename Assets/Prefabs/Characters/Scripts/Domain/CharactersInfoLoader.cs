using System.Linq;
using SinSity.Core;
using SinSity.Scripts;

namespace SinSity.Repo {
    public class CharactersInfoLoader : InfoLoader {
        #region Const

        private const string CHARS_INFO_FOLDER_PATH = "SinSityCharacters";

        #endregion

        public override string infosPath => CHARS_INFO_FOLDER_PATH;

        public ICharacterInfo[] Load() {
            var infos = LoadAs<CharacterInfo>();
            var castedInfos = infos.Cast<ICharacterInfo>();
            return castedInfos.ToArray();
        }
    }
}