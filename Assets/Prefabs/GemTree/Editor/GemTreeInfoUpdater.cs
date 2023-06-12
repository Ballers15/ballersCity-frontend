#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using SinSity.Core;
using IdleClicker.Upgrades;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using VavilichevGD.Tools;


namespace EcoClickerScripts.Tools {
    public class GemTreeInfoUpdater {

        #region CONSTANTS

        private const string SHEETS_ID = "1qbEF5LSlxlU_v6K-y3eak3YW1E4BZLstmT01I6-RP9E";
        private const string PATH_GEM_TREE_INFO = "GemTreeInfo_KIRILL";
        private const string EXCEL_SHEET_NAME = "GemTreeUpdater";
        private static readonly string pathIdlesExcel = $"{Application.dataPath}/Prefabs/GemTree/Balance/GemTreeInfoBalanceTable.xlsx";
        private static Excel xls;
        private static int TITLE_ROWS_COUNT = 1;

        #endregion

        private static UnityWebRequest request;

        [MenuItem("Database/GemTree/Update levels price from google table")]
        public static void UpdateInfo() {
            var url = $"https://docs.google.com/spreadsheets/u/0/d/{SHEETS_ID}/export?format=xlsx";
            request = UnityWebRequest.Get(url);
            request.SendWebRequest();

            EditorApplication.update += EditorUpdate;
        }

        
        private static void EditorUpdate() {
            if (!request.isDone)
                return;

            ShowProgress(0f, "Downloading");
            EditorApplication.update -= EditorUpdate;
            TryToSaveDownloadedFile();
            UpdateAssets();
        }
        
        
        private static void TryToSaveDownloadedFile() {
            if (request.isNetworkError || request.isHttpError) {
                ShowProgress(1f, "Downloading");
                throw new Exception($"Download ERROR: {request.error}");
            }

            using (var fileWriter = new FileStream(pathIdlesExcel, FileMode.OpenOrCreate, FileAccess.Write)) {
                var byteArray = request.downloadHandler.data;
                fileWriter.Write(byteArray, 0, byteArray.Length);
            }
        }

        private static void UpdateAssets() {
            var gemTreeInfo = Resources.Load<GemTreeInfo>(PATH_GEM_TREE_INFO);;
            xls = ExcelHelper.LoadExcel(pathIdlesExcel);
            
            if (IsEcxelTableExist(EXCEL_SHEET_NAME))
            {
                var table = GetEcxelTablebyName(EXCEL_SHEET_NAME);
                var startDataRowNumber = TITLE_ROWS_COUNT + 1;
                var endDataRowNumber = 21;
                var gemTreePriceDataCol = 2;
                var counter = 0;

                for (int rowNumber = startDataRowNumber; rowNumber <= endDataRowNumber; rowNumber++)
                {
                    var gemTreeLevelIndex = GetLevelIndexFromTable(table, rowNumber);
                   
                    if (gemTreeLevelIndex < 0) continue;

                    var gemTreeLevelPrice = GetBigNumberFromTable(table, rowNumber, gemTreePriceDataCol);
                    gemTreeInfo.treeLevels[gemTreeLevelIndex].SetPrice(gemTreeLevelPrice);

                    Debug.Log($"GemTree Level: {gemTreeLevelIndex}, " + $"LevelPrice: {gemTreeInfo.treeLevels[gemTreeLevelIndex].GetBasePrice()}");
                    
                    var progressNormalized = counter / (float) endDataRowNumber;
                    ShowProgress(progressNormalized, gemTreeLevelIndex.ToString());
                }
                AssetDatabase.SaveAssets();

                EditorUtility.SetDirty(gemTreeInfo);

            }

            Resources.UnloadUnusedAssets();
            
            Debug.Log($"GemTreeInfo assets data updated SUCCESSFULLY!!");
            ShowProgress(100,"COMP");
            
        }

        private static void ShowProgress(float progressNormalized, string id) {
            if (progressNormalized < 1f)
                EditorUtility.DisplayProgressBar("Updating Balans", id, progressNormalized);
            else
                EditorUtility.ClearProgressBar();
        }

        private static bool IsEcxelTableExist(string name)
        {
            foreach (var table in xls.Tables)
            {
                if(table.TableName != name) continue;
                return true;
            }
            return false;
        }
        
        private static ExcelTable GetEcxelTablebyName(string name)
        {
            foreach (var table in xls.Tables)
            {
                if(table.TableName != name) continue;
                return table;
            }
            return new ExcelTable();
        }

        private static BigNumber GetBigNumberFromTable(ExcelTable table, int rowNumber, int colNumber)
        {
            var numberStr = (string) table.GetValue(rowNumber, colNumber);
            var numberDouble = Convert.ToDouble(numberStr);
            var numberBigInteger = new BigInteger(numberDouble);
            return new BigNumber(numberBigInteger);
        }

        private static string GetStringFromTable(ExcelTable table, int rowNumber, int colNumber)
        {
            var str = (string) table.GetValue(rowNumber, colNumber);
            return str;
        }

        private static int GetLevelIndexFromTable(ExcelTable table, int rowNumber)
        {
            int levelIndexDataCol = 1;
            var strNum = (string) table.GetValue(rowNumber, levelIndexDataCol);
            return Convert.ToInt32(strNum);
        }

    }
}
#endif