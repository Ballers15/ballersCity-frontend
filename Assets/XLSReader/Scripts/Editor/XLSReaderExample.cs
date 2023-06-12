﻿#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace XLSReader.Example {
    public static class XLSReaderExample {
        [MenuItem("XLSReader/Test ReadWrite")]
        static void ReadWrite() {
            Excel xls = new Excel();
            ExcelTable table = new ExcelTable();
            table.TableName = "test";
            string outputPath = Application.dataPath + "/XLSReader/ExampleTables/Test2.xlsx";
            xls.Tables.Add(table);
            xls.Tables[0].SetValue(1, 1, "1");
            xls.Tables[0].SetValue(1, 2, "2");
            xls.Tables[0].SetValue(2, 1, "3");
            xls.Tables[0].SetValue(2, 2, "4");
            xls.ShowLog();
            ExcelHelper.SaveExcel(xls, outputPath);
        }

        [MenuItem("XLSReader/Test Read Test4")]
        static void Read() {
            string path = Application.dataPath + "/XLSReader/ExampleTables/Test4.xlsx";
            Excel xls = ExcelHelper.LoadExcel(path);
            xls.ShowLog();
        }

        [MenuItem("XLSReader/Test Read read5")]
        static void Read5() {
            string path = Application.dataPath + "/XLSReader//ExampleTables/Test5.xlsx";
            Excel xls = ExcelHelper.LoadExcel(path);
            xls.ShowLog();
        }
    }
}
#endif