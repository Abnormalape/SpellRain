using UnityEngine;
using BHS.AcidRain.Function;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;

namespace BHS.AcidRain.Game
{
    public static class WordData
    {
        private static TextAsset WordDataCSV = Resources.Load("Data/WordsData") as TextAsset;
        private static string WordDataString = WordDataCSV.text;
        private static Dictionary<int, List<string>> _wordLevelDictionary = new();
        private static Dictionary<int, int> _eachLevelWordCounts = new();

        //private static TextAsset SpellDataCSV = Resources.Load("Data/SpellData") as TextAsset;
        //private static string SpellString = SpellDataCSV.text;


        public static Dictionary<int, List<string>> WordLevelDictionary { get => _wordLevelDictionary; }
        public static Dictionary<int, int> EachLevelWordCounts { get => _eachLevelWordCounts; }



        public static void ParseWordDataToApropriateLevel()
        {
            List<Dictionary<string, string>> AllWordData;

            Parsing.ParseInputGetDictionaryList(WordDataString, out AllWordData);

            Dictionary<string, string>.KeyCollection aa = AllWordData[0].Keys;


            string level = "Level";
            string word = "Word";

            foreach (Dictionary<string, string> level_word_dictionary in AllWordData)
            {
                int wordLevel = Convert.ToInt32(level_word_dictionary[level]);

                if (!_wordLevelDictionary.ContainsKey(wordLevel))
                {
                    _wordLevelDictionary.Add(wordLevel, new List<string>());
                }

                _wordLevelDictionary[wordLevel].Add(level_word_dictionary[word]);
            }
        }

        public static void DebugLevelsWordList()
        {
            foreach (var aa in _wordLevelDictionary)
            {
                int count = 0;
                foreach (var bb in aa.Value)
                {
                    count++;
                    Debug.Log($"Level:{aa.Key}. {count}th word = {bb}");
                }
            }
        }

        public static void CountEachLevelWords()
        {
            foreach (var aa in _wordLevelDictionary)
            {
                _eachLevelWordCounts.Add(aa.Key,aa.Value.Count);
            }
        }
    }
}