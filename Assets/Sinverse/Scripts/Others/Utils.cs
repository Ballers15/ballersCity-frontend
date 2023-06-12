using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Sinverse
{
    public enum PlotStatus
    {
        EMPTY = 0,
        PURCHASED,
        SOLD,
        RENTED,
        RESTRICTED
    }

    public enum TransactionType
    {
        Debit = 2,
        Credit = 1
    }

    //grey plots - purchased by other users
    //red plots - restricted covered by buildings or decoration
    //green plots - empty and can be purchased
    //blue plots - owned by localPlayer

    public static class AppPath
    {
        //App Path - PC
        public static string APPLICATION_PATH = Application.streamingAssetsPath;

        //App Path - Mobile
        //public static string APPLICATION_PATH = Application.persistentDataPath;

        //Folder Path
        public static string CITYDATA = "City";
        public static string FILE = "File";
        public static string PLOTS_FILE = "Plots";
        public static string CONSTRUCTION_FILE = "Construction";
        public static string BUNDLE = "Bundle";

        //AssetBundle Folder Path
        public static string TERRAIN_SCENE = "TerrainScene";
    }     
    
    public static class SinverseUtils
    {
        public static bool isLoadMiniGame = false;

        public static bool IsValidEmail(string inputEmail)
        {
            return Regex.IsMatch(inputEmail, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
    }      

    [Serializable]
    public class SinverseData
    {
        private string CombinePath(string path, string fileName)
        {
            return Path.Combine(path, fileName);
        }
        
        public string GetData(string folderName)
        {
            string filePath = CombinePath(CombinePath(CombinePath(CombinePath(AppPath.APPLICATION_PATH, AppPath.CITYDATA), folderName), AppPath.FILE), AppPath.PLOTS_FILE + ".txt");
            try
            {
                StreamReader reader = new StreamReader(filePath);
                string data = reader.ReadToEnd();
                reader.Close();
                return data;
            }
            catch (Exception)
            {
                Debug.LogError("File Not Found");
                return null;
            }
        }
        
        public void SetData(string folderName, string data)
        {
            string filePath = CombinePath(CombinePath(CombinePath(CombinePath(AppPath.APPLICATION_PATH, AppPath.CITYDATA), folderName), AppPath.FILE), AppPath.PLOTS_FILE + ".txt");

            Debug.Log(filePath);
            try
            {
                StreamWriter writer = new StreamWriter(filePath, false);
                writer.WriteLine(data);
                writer.Close();
            }
            catch (Exception)
            {
                Debug.LogError("File Not Found");
            }
        }

        public void SaveConstruction(string currentCity, string data)
        {
            string filePath = CombinePath(CombinePath(CombinePath(CombinePath(AppPath.APPLICATION_PATH, AppPath.CITYDATA), currentCity), AppPath.FILE), AppPath.CONSTRUCTION_FILE + ".txt");
            Debug.Log(filePath);
            try
            {
                StreamWriter writer = new StreamWriter(filePath, false);
                writer.WriteLine(data);
                writer.Close();
            }
            catch (Exception)
            {
                Debug.LogError("File Not Found");
            }
        }

        public string LoadConstruction(string folderName)
        {
            string filePath = CombinePath(CombinePath(CombinePath(CombinePath(AppPath.APPLICATION_PATH, AppPath.CITYDATA), folderName), AppPath.FILE), AppPath.CONSTRUCTION_FILE + ".txt");
            try
            {
                StreamReader reader = new StreamReader(filePath);
                string data = reader.ReadToEnd();
                reader.Close();
                return data;
            }
            catch (Exception)
            {
                Debug.LogError("File Not Found");
                return null;
            }
        }
    }
}

