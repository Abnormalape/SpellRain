using System.Collections.Generic;
using UnityEngine;

namespace BHS.AcidRain.Game
{
    public class WordDataManager : MonoBehaviour
    {
        private void Start()
        {
            WordData.ParseWordDataToApropriateLevel();
            WordData.DebugLevelsWordList();
            WordData.CountEachLevelWords();
        }

        public List<string> GetWordListByLevel(int level)
        {
            List<string> words = new();

            WordData.WordLevelDictionary.TryGetValue(level, out words);

            return words;
        }
    }
}
