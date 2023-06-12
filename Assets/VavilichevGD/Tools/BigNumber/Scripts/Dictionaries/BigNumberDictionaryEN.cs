using VavilichevGD.Tools;

namespace VavilichevGD.Tools {
    public class BigNumberDictionaryEN : IBigNumberDictionary {
        private string[] dictionary = new[] {
            "",
            "K",
            "M",
            "B",
            "T",
            "a",
            "b",
            "c",
            "d",
            "e",
            "f",
            "g",
            "h",
            "i",
            "j",
            "k",
            "l",
            "m",
            "n",
            "o",
            "p",
            "q",
            "r",
            "s",
            "t",
            "u",
            "v",
            "w",
            "x",
            "y",
            "z"
        };


        public string GetTranslatedOrder(BigNumberOrder order) {
            return this.dictionary[(int) order];
        }
    }
}