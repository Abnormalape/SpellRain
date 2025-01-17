using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BHS.AcidRain.Game.WordSpawner;

namespace BHS.AcidRain.Game
{
    public class WordSpawner : MonoBehaviour
    {
        //0. Field
        GameObject WordPrefab; //Todo: base Word Prefab.
        WordManager WordManager;
        WordDataManager WordDataManager;
        string WordPrefabPath = "Prefabs/Word/AcidWord"; //Todo: base Word Prefab Path.

        bool IsSpawning = false;
        int currentLevel;

        List<Transform> SpawnPoint = new(10);


        public delegate void WordSpawn(string Word, GameObject spawnedWord, bool isPersonalWord);
        public event WordSpawn OnWordSpawn;


        private void Awake()
        {
            WordPrefab = Resources.Load("Prefabs/Word/AcidWord") as GameObject;

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
                //Debug.Log("WordSpawner Is On Spawn Loop");

                SelectWord(currentLevel, false, out wordToSpawn);
                SpawnWord(isPersonalWord, out spawnedWord);
                AdjustWord(spawnedWord, currentLevel, wordToSpawn, isPersonalWord);
                AddWord(wordToSpawn, spawnedWord, isPersonalWord);
                NextSpawnTimeSpan(currentLevel, out spawnTimeSpan);

                spawnCount++;
                if (spawnCount >= 20)
                {
                    spawnCount = 0;
                    currentLevel++;
                    if (currentLevel > 5)
                    {
                        currentLevel = 5;
                    }
                }

                yield return new WaitForSeconds(spawnTimeSpan);
            }
        }

        public void SpawnOrderedWordRoutine(bool isPublic, int spellLevel, int Loops, bool isSpellWord, int score = 0, float speed = 1)
        {
            for (int ix = 0; ix < Loops; ix++)
            {
                GameObject spawnedWord;
                string wordToSpawn;

                SelectWord(spellLevel, isSpellWord, out wordToSpawn);
                SpawnWord(!isPublic, out spawnedWord);
                AdjustWord(spawnedWord, spellLevel, wordToSpawn, !isPublic, score, speed);
                AddWord(wordToSpawn, spawnedWord, !isPublic);
            }
        }

        //2. SelectWord
        //Todo:
        void SelectWord(int wordLevel, bool isSpellWord, out string selectedWord)
        {
            int _wordLevel = wordLevel;

            if (isSpellWord)
                _wordLevel++;

            SelectWordsRecursively(_wordLevel, isSpellWord, out selectedWord);
        }

        void SelectWordsRecursively(int wordLevel, bool isSpellWord, out string selectedWord)
        {
            int randomWordNum;
            string _word;
            randomWordNum = Random.Range(0, WordData.EachLevelWordCounts[wordLevel]);
            _word = WordDataManager.GetWordListByLevel(wordLevel)[randomWordNum];


            string finalSelectedWord;

            if (WordManager.PersonalSpawnedDictionary.ContainsKey(_word)
                || WordManager.PublicSpawnedDictionary.ContainsKey(_word))
            {
                SelectWordsRecursively(wordLevel, isSpellWord, out finalSelectedWord);
            }
            else
            {
                finalSelectedWord = _word;
            }

            selectedWord = finalSelectedWord;
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

        void AdjustWord(GameObject spawnedWord, int currentLevel, string wordSpell, bool isPersonal, int inputScore = 0, float speed = 1f)
        {
            AcidWordController acidWordController =
                spawnedWord.GetComponent<AcidWordController>();

            acidWordController.AdjustWord(currentLevel, wordSpell, isPersonal, inputScore, speed); //Todo: for what? it works well...
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
            float centerTime = 20f / (9f + 1f * (currentLevel / 1f));
            timeSpan = Random.Range(centerTime * 0.9f, centerTime * 1.1f);
        }

        public void ManagerFoundSpawner(WordManager founder)
        {
            WordManager = founder;
            WordDataManager = FindFirstObjectByType<WordDataManager>();
        }
    }
}
