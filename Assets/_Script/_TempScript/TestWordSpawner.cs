using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BHS.AcidRain.Test
{
    public class TestWordSpawner : MonoBehaviour
    {
        //0. Field
        GameObject WordPrefab = new();
        string WordPrefabPath = "AA";

        //1. StartSpawn
        void StartSpawn()
        {
            StartCoroutine(SpawnRoutine());
        }

        IEnumerator SpawnRoutine()
        {
            //0. Field
            string wordToSpawn;
            GameObject spawnedWord;

            SelectWord(1, out wordToSpawn);
            SpawnWord(true, out spawnedWord);

            yield return null;
        }

        //2. SelectWord
        void SelectWord(int wordLevel, out string selectedWord)
        {
            //0. Set member
            int _wordLevel = wordLevel;
            string _word;

            //1. Get Word Data
            _word = TempWordData.Level_1_Words;

            //2. Select Word From Data.
            selectedWord = _word;
        }

        //3. SpawnWord
        void SpawnWord(bool isPersonal, out GameObject spawnedWord)
        {
            if (isPersonal)
            {
                spawnedWord = Instantiate(WordPrefab);
            }
            else
            {
                spawnedWord = PhotonNetwork.Instantiate(
                    WordPrefabPath, 
                    Vector3.zero, 
                    Quaternion.identity);
            }
        }

        //4. AdjustWord
        void AdjustWord(GameObject spawnedWord)
        {
            spawnedWord.GetComponent<TextMeshProUGUI>().text
                = "";
        }

        //5. AddWord

        //6. RemoveWord

        //7. StopSpawn
    }
}
