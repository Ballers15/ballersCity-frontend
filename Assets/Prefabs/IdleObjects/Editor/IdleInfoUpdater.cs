#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using SinSity.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using VavilichevGD.Tools;


namespace EcoClickerScripts.Tools {
    public class IdleInfoUpdater {

        #region CONSTANTS

        private const string SHEETS_ID = "1qbEF5LSlxlU_v6K-y3eak3YW1E4BZLstmT01I6-RP9E";
        private const string PATH_IDLE_INFO = "IdleObjectsInfo";
        private const string TABLE_NAME = "IdleInfo";
        private static readonly string pathIdlesExcel = $"{Application.dataPath}/Prefabs/IdleObjects/Balance/IdleObjectsBalanceTable.xlsx";
        private static Excel xls;
        private static int TITLE_ROWS_COUNT = 1;

        #endregion

        private static UnityWebRequest request;

        [MenuItem("Database/IdleObjects/Update from google table")]
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
            //var upgradesMap = InitUpgradesMap();
            var idlesMap = InitIdleMap();
            xls = ExcelHelper.LoadExcel(pathIdlesExcel);
            
            if (IsEcxelTableExist(TABLE_NAME))
            {
                var table = GetEcxelTablebyName(TABLE_NAME);
                var startDataRowNumber = TITLE_ROWS_COUNT + 1;
                //var endDataRowNumber = table.NumberOfRows;
                var endDataRowNumber = 29;
                var counter = 0;

                for (int rowNumber = startDataRowNumber; rowNumber <= endDataRowNumber; rowNumber++)
                {
                    var newIdleInfo = GetIdleInfoFromTable(table, rowNumber);
                    if (newIdleInfo != null)
                    {
                        idlesMap[newIdleInfo.id].SetupConfig(newIdleInfo);
                        Debug.Log($"Idle Id: {newIdleInfo.id}, " +
                                  $"priceToBuild: {newIdleInfo.priceToBuild}, " +
                                  $"improvePrice: {newIdleInfo.priceImproveDefault}, " +
                                  $"priceStep: {newIdleInfo.priceStep}, " +
                                  $"incomeDefault: {newIdleInfo.incomeDefault}, " +
                                  $"incomePeriodDefault: {newIdleInfo.incomePeriodDefault}");
                    }
                    
                    var progressNormalized = counter / (float) endDataRowNumber;
                    ShowProgress(progressNormalized, newIdleInfo.id);
                }
                AssetDatabase.SaveAssets();

                SetDirtyIdlesMap(idlesMap);

            }

            Resources.UnloadUnusedAssets();
            
            Debug.Log($"IdleObjectsInfo assets data updated SUCCESSFULLY!!");
            ShowProgress(100,"COMP");
            
        }

        private static void ShowProgress(float progressNormalized, string id) {
            if (progressNormalized < 1f)
                EditorUtility.DisplayProgressBar("Updating upgrades Balans", id, progressNormalized);
            else
                EditorUtility.ClearProgressBar();
        }
        
        private static Dictionary<string, IdleObjectInfo> InitIdleMap()
        {
            var idles = Resources.LoadAll<IdleObjectInfo>(PATH_IDLE_INFO);
            var createdIdlesMap = new Dictionary<string, IdleObjectInfo>();
            foreach (var info in idles)
                createdIdlesMap[info.id] = info;
            return createdIdlesMap;
        }

        private static IdleObjectInfo GetIdleInfoFromTable(ExcelTable table, int rowNumber)
        {
            var idleId = GetIdleIdFromTable(table, rowNumber);
            if (string.IsNullOrEmpty(idleId))
                return null;
            
            var idDataCol = 1;
            var priceToBuildDataCol = 2;
            var priceImproveDataCol = 3;
            var incomeDefaultDataCol = 5;
            var titleCodeDataCol = 7;
            var descrCodeDataCol = 8;
            
            var id = GetStringFromTable(table, rowNumber, idDataCol);
            var priceToBuild = GetBigNumberFromTable(table, rowNumber, priceToBuildDataCol);
            var priceImproveDefault = GetBigNumberFromTable(table, rowNumber, priceImproveDataCol);
            var priceStep = GetPriceStepFromTable(table, rowNumber);
            var incomeDefault = GetBigNumberFromTable(table, rowNumber, incomeDefaultDataCol);
            var incomePeriodDefault = GetIncomePeriodFromTable(table, rowNumber);
            var titleCode = GetStringFromTable(table, rowNumber, titleCodeDataCol);
            var descrCode = GetStringFromTable(table, rowNumber, descrCodeDataCol);
            
            var newIdleInfo = new IdleObjectInfo();
            newIdleInfo.id = id;
            newIdleInfo.priceToBuild = priceToBuild;
            newIdleInfo.priceImproveDefault = priceImproveDefault;
            newIdleInfo.priceStep = priceStep;
            newIdleInfo.incomeDefault = incomeDefault;
            newIdleInfo.incomePeriodDefault = incomePeriodDefault;
            newIdleInfo.titleCode = titleCode;
            newIdleInfo.descriptionCode = descrCode;

            return newIdleInfo;
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

        private static string GetIdleIdFromTable(ExcelTable table, int rowNumber)
        {
            var idleIdDataCol = 1;
            var idleId = (string) table.GetValue(rowNumber, idleIdDataCol);
            return idleId;
        }

        private static BigNumber GetBigNumberFromTable(ExcelTable table, int rowNumber, int colNumber)
        {
            var numberStr = (string) table.GetValue(rowNumber, colNumber);
            var numberDouble = Convert.ToDouble(numberStr);
            var numberBigInteger = new BigInteger(numberDouble);
            return new BigNumber(numberBigInteger);
        }

        private static float GetPriceStepFromTable(ExcelTable table, int rowNumber)
        {
            var priceStepDataCol = 4;
            var multiplierStr = (string) table.GetValue(rowNumber, priceStepDataCol);
            return float.Parse(multiplierStr);
        }
        
        private static float GetIncomePeriodFromTable(ExcelTable table, int rowNumber) {
            var incomePeriodDataCol = 6;
            var incomePeriodStr = (string) table.GetValue(rowNumber, incomePeriodDataCol);
            return float.Parse(incomePeriodStr);
        }

        private static string GetStringFromTable(ExcelTable table, int rowNumber, int colNumber)
        {
            var str = (string) table.GetValue(rowNumber, colNumber);
            return str;
        }

        private static void SetDirtyIdlesMap(Dictionary<string, IdleObjectInfo> map)
        {
            foreach (var idleInfo in map)
            {
                EditorUtility.SetDirty(idleInfo.Value);
            }
        }

    }
}
#endif