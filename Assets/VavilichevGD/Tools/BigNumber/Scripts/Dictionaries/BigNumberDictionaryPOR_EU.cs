namespace VavilichevGD.Tools {
    public class BigNumberDictionaryPOR_EU : IBigNumberDictionary {
        private string[] dictionary = {
            "",
            "K",
            "Mi",
            "Bi",
            "Tri",
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