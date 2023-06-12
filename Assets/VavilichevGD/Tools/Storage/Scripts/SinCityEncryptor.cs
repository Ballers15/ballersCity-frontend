using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using SinSity.Data;
using UnityEngine;

namespace VavilichevGD.Tools {
   public static class SinCityEncryptor {
        private static readonly string password = "9b5Q7%gV@tiq/MadDiamondGamesPSWD";
        private static readonly string salt = "THISIS9b5Q7%gV@tiq/MadDiamondSAULT";
        private static readonly string VIKey = "@1B2ccD4e562g7H8";

        private static double VERSION_TRASHOLD = 1.95;
        private static string KEY_DECRYPTION_ENABLED = "DECRYPTION_ENABLED";
        
        public static string GetEncryptedJson(object data) {
            var dataJson = JsonUtility.ToJson(data);
            var encryptData = Encrypt(dataJson);
            return encryptData;
        }
        
        public static string GetEncryptedJsonIEnumerable(IEnumerable list) {
            var dataJson = JsonConvert.SerializeObject(list);
            var encryptData = Encrypt(dataJson);
            return encryptData;
        }

        public static string Encrypt(string plainText) {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            var keyBytes = new Rfc2898DeriveBytes(password, Encoding.ASCII.GetBytes(salt)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
			
            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }
    
        public static string Decrypt(string encryptedText) {
            try {
                var cipherTextBytes = Convert.FromBase64String(encryptedText);

                var keyBytes = new Rfc2898DeriveBytes(password, Encoding.ASCII.GetBytes(salt)).GetBytes(256 / 8);
                var symmetricKey = new RijndaelManaged() {Mode = CipherMode.CBC, Padding = PaddingMode.None};

                var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
                var memoryStream = new MemoryStream(cipherTextBytes);
                var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                var plainTextBytes = new byte[cipherTextBytes.Length];

                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
            }
            catch {
                return encryptedText;
            }
        }
   }
}