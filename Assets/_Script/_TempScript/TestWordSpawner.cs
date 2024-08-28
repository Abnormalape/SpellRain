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
        GameObject WordPrefab; //Todo: base Word Prefab.
        TestWordManager WordManager;
        string WordPrefabPath = "Prefabs/Word/TestAcidWord"; //Todo: base Word Prefab Path.

        bool IsSpawning = false;
        int currentLevel;

        List<Transform> SpawnPoint = new(10);


        public delegate void WordSpawn(string Word, GameObject spawnedWord, bool isPersonalWord);
        public event WordSpawn OnWordSpawn;


        private void Awake()
        {
            WordPrefab = Resources.Load("Prefabs/Word/TestAcidWord") as GameObject;

            Transform[] tempTransform = GetComponentsInChildren<Transform>();
            for (int ix = 0; ix < tempTransform.Length; ix++)
            {
                if (tempTransform[ix] == transform)
                    continue;

                SpawnPoint.Add(tempTransform[ix]);
            }
        }

        //1. StartSpawn
        //isMasterClient
        public void StartSpawn()
        {
            StartCoroutine(PublicWordSpawnRoutine());
        }

        /// <summary>
        /// Word made in this method must be Public Word.
        /// </summary>
        /// <returns></returns>
        IEnumerator PublicWordSpawnRoutine(int startLevel = 1)
        {
            GameObject spawnedWord;
            string wordToSpawn;
            bool isPersonalWord = false;
            float spawnTimeSpan;
            int spawnCount = 0;
            currentLevel = startLevel;

            IsSpawning = true;

            while (IsSpawning)
            {
                Debug.Log("WordSpawner Is On Spawn Loop");

                SelectWord(currentLevel, out wordToSpawn);
                SpawnWord(isPersonalWord, out spawnedWord);
                AdjustWord(spawnedWord, currentLevel, wordToSpawn, isPersonalWord);
                AddWord(wordToSpawn, spawnedWord, isPersonalWord);
                NextSpawnTimeSpan(currentLevel, out spawnTimeSpan);

                spawnCount++;
                if (spawnCount >= 20)
                    currentLevel++;

                yield return new WaitForSeconds(spawnTimeSpan);
                yield return new WaitForSeconds(10000f); //Todo:
            }
        }

        public void SpawnOrderedWordRoutine(string orderedWord, bool isPersonalWord = true)
        {
            GameObject spawnedWord;

            SpawnWord(isPersonalWord, out spawnedWord);
            AdjustWord(spawnedWord, currentLevel, orderedWord, isPersonalWord);
            AddWord(orderedWord, spawnedWord, isPersonalWord);
        }

        //2. SelectWord
        //Todo:
        void SelectWord(int wordLevel, out string selectedWord)
        {
            //0. Set member
            int _wordLevel = wordLevel;
            string _word;

            //1. Get Word Data
            _word = TempWordData.Level_1_Words;
            _word = "AAAAAAAA";

            //2. Select Word From Data.
            selectedWord = _word;
        }

        void SpawnWord(bool isPersonal, out GameObject spawnedWord)
        {
            int spawnPointNum = Random.Range(0, SpawnPoint.Count);
            Vector3 spawnPosition = SpawnPoint[spawnPointNum].position;

            if (isPersonal)
            {
                spawnedWord = Instantiate(
                    WordPrefab,
                    spawnPosition,
                    Quaternion.identity,
                    null);
            }
            else
            {
                spawnedWord = PhotonNetwork.Instantiate(
                    WordPrefabPath,
                    spawnPosition,
                    Quaternion.identity);
            }
        }

        void AdjustWord(GameObject spawnedWord, int currentLevel, string wordSpell, bool isPersonal, float speed = 1f)
        {
            TestAcidWordController testAcidWordController =
                spawnedWord.GetComponent<TestAcidWordController>();

            testAcidWordController.AdjustWord(currentLevel, wordSpell, isPersonal, speed);
        }

        void AddWord(string Word, GameObject spawnedWord, bool isPersonalWord)
        {
            OnWordSpawn(Word, spawnedWord, isPersonalWord);
        }

        void StopSpawn()
        {
            IsSpawning = false;
        }

        void NextSpawnTimeSpan(int currentLevel, out float timeSpan)
        {
            float centerTime = 2f / currentLevel;
            timeSpan = Random.Range(centerTime * 0.9f, centerTime * 1.1f);
        }

        public void ManagerFoundSpawner(TestWordManager founder)
        {
            WordManager = founder;
        }
    }
}
