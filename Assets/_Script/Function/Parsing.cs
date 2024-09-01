using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WebSocketSharp;

namespace BHS.AcidRain.Function
{
    public static class Parsing
    {
        public static void ParseInputGetDictionaryList(string inputText, out List<Dictionary<string, string>> getList)
        {
            List<Dictionary<string, string>> dictionaryList = new();
            ParseString(inputText, out dictionaryList);
            getList = dictionaryList;
        }

        public static void ParseInputGetDictionaryList(TextAsset inputTextAsset, out List<Dictionary<string, string>> getList)
        {
            string inputText = inputTextAsset.text;
            List<Dictionary<string, string>> dictionaryList = new();
            ParseString(inputText, out dictionaryList);
            getList = dictionaryList;
        }

        /// <summary>
        /// This method can only parse csv with no 'Enter' in certain section.
        /// </summary>
        /// <param name="inputText">String</param>
        private static void ParseString(string inputText, out List<Dictionary<string, string>> listWantToAdjust)
        {
            List<Dictionary<string, string>> dictionaryList = new();

            string[] lineArray = inputText.Split('\n');

            string[] keys = lineArray[0].Split(',');

            for (int i = 0; i < keys.Length; i++)
            {
                keys[i] = keys[i].Replace("\r", "");
            }


            for (int ix = 1; ix < lineArray.Length; ix++)
            {
                Dictionary<string, string> tempDictionary = new();

                string[] values = lineArray[ix].Split(',');

                for (int iy = 0; iy < keys.Length; iy++)
                {
                    values[iy].Trim();

                    if (values[iy].IsNullOrEmpty())
                        tempDictionary.Add(keys[iy], "");
                    else
                        tempDictionary.Add(keys[iy], values[iy]);
                }

                dictionaryList.Add(tempDictionary);
            }
            listWantToAdjust = dictionaryList;
        }
    }
}


//private static List<Dictionary<string, string>> ParseCsvFile(string csvContent)
//{
//    List<Dictionary<string, string>> outdata = new List<Dictionary<string, string>>();

//    // 줄 단위로 분리.
//    string[] lines = csvContent.Split('\n');

//    // 첫 열을 키로 사용.
//    if (lines.Length <= 1) { return null; } // 데이터가 없을 경우 빈 리스트를 반환.

//    // 첫 열을 쉼표로 나누어 키로 저장.
//    string[] headers = lines[0].Split(',');

//    // 각 줄마다 데이터를 쪼개고 저장.
//    for (int i = 1; i < lines.Length; i++)
//    {
//        if (string.IsNullOrWhiteSpace(lines[i])) continue; // 빈 줄은 건너뜁니다.

//        // 쉼표로 필드를 구분. 큰따옴표 안의 쉼표는 무시.
//        string[] fields = Regex.Split(lines[i], ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

//        Dictionary<string, string> entry = new Dictionary<string, string>();

//        for (int j = 0; j < headers.Length; j++)
//        {
//            // 키는 헤더 이름, 값은 해당 필드의 값.
//            // 각 값의 큰따옴표를 제거하고, 공백을 제거.
//            if (j == fields.Length)
//            {
//                break;
//            }
//            entry[headers[j].Trim()] = fields[j].Trim('"').Trim();
//        }
//        outdata.Add(entry);
//    }
//    return outdata;
//}
