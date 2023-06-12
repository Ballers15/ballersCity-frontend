using VavilichevGD.LocalizationFramework;

namespace VavilichevGD.Tools {
    public static class BigNumberLocalizator {

        private static IBigNumberDictionary simpleDictionary;
        private static string currentLanguageCode;
        
        private static void DefineSimpleDictionary(string languageCode) {
            currentLanguageCode = languageCode;
            switch (languageCode) {
                
                case "ENG":
                    simpleDictionary = new BigNumberDictionaryEN();
                    break;
                
                case "RUS":
                    simpleDictionary = new BigNumberDictionaryRU();
                    break;
                
                case "DEU":
                    simpleDictionary = new BigNumberDictionaryDEU();
                    break;
                
                case "FRA":
                    simpleDictionary = new BigNumberDictionaryFRA();
                    break;
                
                case "CHN (Land)":
                    simpleDictionary = new BigNumberDictionaryCHN_Land();
                    break;
                
                case "CHN (T)":
                    simpleDictionary = new BigNumberDictionaryCHN_T();
                    break;
                
                case "ESP (LatAm)":
                    simpleDictionary = new BigNumberDictionaryESP_LA();
                    break;
                
                case "ESP (EU)":
                    simpleDictionary = new BigNumberDictionaryESP_EU();
                    break;
                
                case "HIN":
                    simpleDictionary = new BigNumberDictionaryHIN();
                    break;
                
                case "IND":
                    simpleDictionary = new BigNumberDictionaryIND();
                    break;
                
                case "ITA":
                    simpleDictionary = new BigNumberDictionaryITA();
                    break;
                
                case "JPN":
                    simpleDictionary = new BigNumberDictionaryJPN();
                    break;
                
                case "KOR":
                    simpleDictionary = new BigNumberDictionaryKOR();
                    break;
                
                case "MSA":
                    simpleDictionary = new BigNumberDictionaryMSA();
                    break;
                
                case "POL":
                    simpleDictionary = new BigNumberDictionaryPOL();
                    break;
                
                case "POR (Bra)":
                    simpleDictionary = new BigNumberDictionaryPOR_Bra();
                    break;
                
                case "POR (EU)":
                    simpleDictionary = new BigNumberDictionaryPOR_EU();
                    break;
                
                case "THA":
                    simpleDictionary = new BigNumberDictionaryTHA();
                    break;
                
                case "TUR":
                    simpleDictionary = new BigNumberDictionaryTUR();
                    break;
                
                case "VIE":
                    simpleDictionary = new BigNumberDictionaryVIE();
                    break;
                
                default:
                    simpleDictionary = new BigNumberDictionaryEN();
                    break;
                
            }
        }

        public static IBigNumberDictionary GetSimpleDictionary(string languageCode) {
            if (simpleDictionary == null || languageCode != currentLanguageCode)
                DefineSimpleDictionary(languageCode);
            return simpleDictionary;
        }

        public static IBigNumberDictionary GetSimpleDictionary() {
            var languageCode = Localization.GetCurrentLanguageCode();
            if (simpleDictionary == null || languageCode != currentLanguageCode)
                DefineSimpleDictionary(languageCode);
            return simpleDictionary;
        }
    }
}