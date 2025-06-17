using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UJ.Data
{
    public static class ExcelConverter
    {
        [MenuItem("Tools/Convert Excel to JSON")]
        public static void ConvertExcelToJson()
        {
            // ExcelData 폴더에서 Excel 파일 찾기
            string[] excelPaths = AssetDatabase.FindAssets("t:Spreadsheet", new[] { "Assets/ExcelData" });
            if (excelPaths.Length == 0)
            {
                excelPaths = AssetDatabase.FindAssets("t:TextAsset", new[] { "Assets/ExcelData" });
                if (excelPaths.Length == 0)
                {
                    Debug.LogError("Excel file not found in ExcelData folder");
                    return;
                }
            }

            string excelPath = AssetDatabase.GUIDToAssetPath(excelPaths[0]);
            string jsonPath = Path.Combine(
                Path.GetDirectoryName(excelPath),
                Path.GetFileNameWithoutExtension(excelPath) + ".json"
            );

            try
            {
                string[] lines = File.ReadAllLines(excelPath);
                TargetDataList targetDataList = new TargetDataList();

                // 헤더를 스킵하고 데이터 파싱
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] values = lines[i].Split(',');
                    if (values.Length >= 8) // 필드 개수 확인
                    {
                        TargetData targetData = new TargetData
                        {
                            prefabName = values[0].Trim(),
                            pointValue = int.Parse(values[1].Trim()),
                            tag = values[2].Trim(),
                            minSpeed = float.Parse(values[3].Trim()),
                            maxSpeed = float.Parse(values[4].Trim()),
                            maxTorque = float.Parse(values[5].Trim()),
                            xRange = float.Parse(values[6].Trim()),
                            ySpawnPos = float.Parse(values[7].Trim())
                        };
                        targetDataList.targets.Add(targetData);
                    }
                }

                // JSON으로 변환
                string json = JsonUtility.ToJson(targetDataList, true);
                File.WriteAllText(jsonPath, json);

                // Unity 에디터에서 AssetDatabase 갱신
                AssetDatabase.Refresh();
                Debug.Log($"Conversion complete: {jsonPath}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Conversion failed: {e.Message}");
            }
        }
    }
}
