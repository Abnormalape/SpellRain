using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

namespace BHS.AcidRain.Test
{
    public class TestWordManager : MonoBehaviour
    {
        Dictionary<string, GameObject> PersonalSpawnedDictionary = new();
        Dictionary<string, GameObject> PublicSpawnedDictionary = new();

        int Combo = 0;
        int Score = 0;

        public TestTextInput InputField { get; private set; }
        TestWordSpawner TestWordSpawner;
        TestScoreBoardManager TestScoreBoardManagerInstance;
        public PhotonView PhotonView;


        public delegate void ChangeScore(int score);
        public event ChangeScore OnChangeScore;


        private void Awake()
        {
            PhotonView = GetComponent<PhotonView>();
            InputField = FindFirstObjectByType<TestTextInput>();
            TestWordSpawner = FindFirstObjectByType<TestWordSpawner>();
            TestScoreBoardManagerInstance = new TestScoreBoardManager(this);
        }

        private void Start()
        {
            InputField.OnEnterText += JudgeInputText;

            TestWordSpawner.ManagerFoundSpawner(this);
            TestWordSpawner.OnWordSpawn += AddWord;

            Debug.Log("TestWordManaget Statrs");

            if (PhotonNetwork.IsMasterClient) //Todo: When StartSpawn Should Be Adjusted.
                TestWordSpawner.StartSpawn();

            Debug.Log("TestWordManaget Start Spawn");

        }

        /// <summary>
        /// This method is used for removing words in two ways.
        /// 1. Use In RPC / 2. Use as method.
        /// </summary>
        /// <param name="word">Word which want to remove.</param>
        /// <param name="isPersonalWord">If on RPC this should false. Else, true.</param>
        [PunRPC]
        void TryRemoveWord(string word, bool isPersonalWord, Player remover)
        {
            GameObject targetWord;
            TestAcidWordController targetController;
            int score;

            if (isPersonalWord)
            {
                targetWord = PersonalSpawnedDictionary[word];
                targetController = targetWord.GetComponent<TestAcidWordController>();
                score = targetController.score;

                Destroy(PersonalSpawnedDictionary[word]);
                PersonalSpawnedDictionary.Remove(word);
                AddScore(score);
                AdjustCombo(1);
            }
            else
            {
                PublicSpawnedDictionary.TryGetValue(word, out targetWord);
                if (targetWord != null)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        PhotonNetwork.Destroy(PublicSpawnedDictionary[word]);
                    }

                    PublicSpawnedDictionary.Remove(word);

                    if(PhotonNetwork.LocalPlayer == remover)
                    {
                        targetController = targetWord.GetComponent<TestAcidWordController>();
                        score = targetController.score;
                        AddScore(score);
                        AdjustCombo(1);
                    }
                }
            }
        }

        /// <summary>
        /// When other client want to make word to another client, use this as RPC.
        /// </summary>
        /// <param name="wordSpell"></param>
        /// <param name="isPersonal"></param>
        [PunRPC]
        public void OrderSpawnerSpawnWord(string wordSpell, bool isPersonal = true)
        {
            TestWordSpawner.SpawnOrderedWordRoutine(wordSpell, isPersonal);
        }

        void JudgeInputText(string inputText)
        {
            bool isPersonalWord = false;
            bool isPublicWord = false;
            bool isWrongWord = false;


            isPersonalWord = PersonalSpawnedDictionary.ContainsKey(inputText);
            isPublicWord = PublicSpawnedDictionary.ContainsKey(inputText);


            if (isPersonalWord && isPublicWord)
                isPersonalWord = false;
            else if (!isPersonalWord && !isPublicWord)
                isWrongWord = true;


            if (isWrongWord)
                ResetCombo();
            else if (isPersonalWord)
                TryRemoveWord(inputText, isPersonalWord, null);
            else
                PhotonView.RPC("TryRemoveWord", RpcTarget.All, inputText, isPersonalWord, PhotonNetwork.LocalPlayer);
        }

        void AddWord(string Word, GameObject spawnedWord, bool isPersonalWord)
        {
            if (isPersonalWord)
                PersonalSpawnedDictionary.Add(Word, spawnedWord);
            else
            {
                spawnedWord.GetComponent<PhotonView>().RPC("AddToPublicDictionary", RpcTarget.All, Word);

                //PhotonView.RPC(
                //    "AddPublicWord",
                //    RpcTarget.All,
                //    Word,
                //    spawnedWord);
            }
        }

        [PunRPC]
        public void AddPublicWord(string Word, GameObject spawnedWord)
        {
            PublicSpawnedDictionary.Add(Word, spawnedWord);
        }

        void AddScore(int scoreInput)
        {
            Score += scoreInput;
            OnChangeScore(Score);
        }

        void AdjustCombo(int comboAdjusted)
        {
            Combo += comboAdjusted;
        }

        void ResetCombo()
        {
            Combo = 0;
        }

        [PunRPC]
        public void AdjustScoreOfPlayer(int score, Player adjustedPlayer)
        {
            TestScoreBoardManagerInstance.AdjustScoreOfPlayer(score, adjustedPlayer);
        }
    }
}