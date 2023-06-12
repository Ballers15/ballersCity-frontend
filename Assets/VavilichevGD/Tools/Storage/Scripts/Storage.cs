using System;
using UnityEngine;
using VavilichevGD.Architecture;

namespace VavilichevGD.Tools {
    public static class Storage {

        public static bool HasObject(string prefKey) {
            return PlayerPrefs.HasKey(prefKey);
        }
        
        public static void ClearKey(string prefKey) {
            PlayerPrefs.DeleteKey(prefKey);
            Debug.Log($"Key \"{prefKey}\" was cleaned.");
        }

        public static void ClearAll() {
            PlayerPrefs.DeleteAll();
        }
        
        
        #region SetData
        
        [Obsolete("This method to save is not secure. Better use custom serialized classes with SetCustom(CustomClass customClass) method")]
        public static void SetFloat(string prefKey, float value) {
            PlayerPrefs.SetFloat(prefKey, value);
        }
        
        [Obsolete("This method to save is not secure. Better use custom serialized classes with SetCustom(CustomClass customClass) method")]
        public static void SetInteger(string prefKey, int value) {
            PlayerPrefs.SetInt(prefKey, value);
        }
        
        [Obsolete("This method to save is not secure. Better use custom serialized classes with SetCustom(CustomClass customClass) method")]
        public static void SetBool(string prefKey, bool value) {
            var convertedValue = BoolToInteger(value);
            PlayerPrefs.SetInt(prefKey, convertedValue);
        }
        
        public static void SetString(string prefKey, string value) {
            var encryptedValue = SinCityEncryptor.Encrypt(value);
            PlayerPrefs.SetString(prefKey, encryptedValue);
        }
        
        public static void SetEnum(string prefKey, Enum value) {
            var encryptedStrValue = SinCityEncryptor.Encrypt(value.ToString());
            PlayerPrefs.SetString(prefKey, encryptedStrValue);
        }
        
        public static void SetDateTime(string prefKey, DateTime? value) {
            var encryptedDateTime = SinCityEncryptor.Encrypt(value.ToString());
            PlayerPrefs.SetString(prefKey, encryptedDateTime);
        }
        
        public static void SetCustom<T>(string prefKey, T value) {
            var json = JsonUtility.ToJson(value);
            var encryptedJson = SinCityEncryptor.Encrypt(json);
            PlayerPrefs.SetString(prefKey, encryptedJson);
        }
        
        #endregion

        
        #region GetData

        [Obsolete("This method to save is not secure. Better use custom serialized classes with SetCustom(CustomClass customClass) method")]
        public static float GetFloat(string prefKey, float defaultValue = 0f) {
            if (!PlayerPrefs.HasKey(prefKey))
                SetFloat(prefKey, defaultValue);
            return PlayerPrefs.GetFloat(prefKey);
        }
        
        [Obsolete("This method to save is not secure. Better use custom serialized classes with SetCustom(CustomClass customClass) method")]
        public static int GetInteger(string prefKey, int defaultValue = 0) {
            if (!PlayerPrefs.HasKey(prefKey))
                SetInteger(prefKey, defaultValue);
            return PlayerPrefs.GetInt(prefKey);
        }
        
        [Obsolete("This method to save is not secure. Better use custom serialized classes with SetCustom(CustomClass customClass) method")]
        public static bool GetBool(string prefKey, bool defaultValue = false) {
            if (!PlayerPrefs.HasKey(prefKey))
                SetBool(prefKey, defaultValue);
            var intValue = PlayerPrefs.GetInt(prefKey);
            return IntToBool(intValue);
        }
        
        public static string GetString(string prefKey, string defaultValue = "") {
            var encryptedValue = "";
            if (!PlayerPrefs.HasKey(prefKey))
                SetString(prefKey, defaultValue);

            encryptedValue = PlayerPrefs.GetString(prefKey);
            return SinCityEncryptor.Decrypt(encryptedValue);
        }
        
        public static T GetEnum<T>(string prefKey, T defaultValue) where T : Enum{
            if (!PlayerPrefs.HasKey(prefKey))
                SetEnum(prefKey, defaultValue);
           
            var encryptedStrValue = PlayerPrefs.GetString(prefKey);
            var strValue = SinCityEncryptor.Decrypt(encryptedStrValue);
            var value = Enum.Parse(typeof(T), strValue);
            return (T) value;
        }
        
        public static DateTime? GetDateTime(string prefKey, DateTime? defaultValue = null) {
            if (!PlayerPrefs.HasKey(prefKey))
                SetDateTime(prefKey, defaultValue);
            
            var encryptedDateTimeString = PlayerPrefs.GetString(prefKey);
            var dateTimeString = SinCityEncryptor.Decrypt(encryptedDateTimeString);
            DateTime? loadedDateTime;
            try {
                loadedDateTime = DateTime.Parse(dateTimeString);
            }
            catch {
                loadedDateTime = null;
            }

            return loadedDateTime;
        }

        public static T GetCustom<T>(string prefKey, T defaultValue) {
            if (!PlayerPrefs.HasKey(prefKey))
                SetCustom(prefKey, defaultValue);
            var encryptedJson = PlayerPrefs.GetString(prefKey);
            var json = SinCityEncryptor.Decrypt(encryptedJson);
            var result = JsonUtility.FromJson<T>(json);
            return result;
        }

        #endregion
        
        #region Convertion

        private static int BoolToInteger(bool value) {
            if (value)
                return 1;
            return 0;
        }

        private static bool IntToBool(int value) {
            return value != 0;
        }

        #endregion
    }
}