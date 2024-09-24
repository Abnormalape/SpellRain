using BHS.AcidRain.UI;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace BHS.AcidRain.Game
{
    public class WordManager : MonoBehaviour
    {
        public Dictionary<string, GameObject> PersonalSpawnedDictionary = new();
        public Dictionary<string, GameObject> PublicSpawnedDictionary = new();

        private SpellManager _spellController;

        private int _combo = 0;
        public int Combo
        {
            get => _combo;
            set => _combo = value;
        }
        public int Score = 0;

        private readonly int MAX_HP = 10;
        private int _hp = 10;
        public int HP
        {
            get => _hp;
            set => _hp = value;
        }

        [PunRPC]
        public void AdjustHP(int i)
        {
            if (IsDead)
                return;

            HP += i;

            if (HP >= MAX_HP)
                HP = MAX_HP;
            else if (HP <= 0)
            {
                HP = 0;
                MakeDead();
                PhotonView.RPC("SetPlayerDead", RpcTarget.All, PhotonNetwork.LocalPlayer);
            }
            Debug.Log(HP);
        }

        public bool IsDead = false;
        private void MakeDead() { IsDead = true; GameOverSign.enabled = true; }

        private Canvas GameOverSign;
        public TextInput InputField { get; private set; }
        WordSpawner WordSpawner;
        public ScoreBoardManager ScoreBoardManagerInstance;
        private ComboUIController _comboUIcontroller;
        public PhotonView PhotonView;

        public float MAXPASSEDCOMBOTIME { get; private set; } = 3f;
        public float PassedComboTime { get; private set; } = 3f;


        public delegate void ChangeScore(int score);
        public event ChangeScore OnChangeScore;


        private void Awake()
        {
            PhotonView = GetComponent<PhotonView>();
            InputField = FindFirstObjectByType<TextInput>();
            WordSpawner = FindFirstObjectByType<WordSpawner>();
            _comboUIcontroller = FindFirstObjectByType<ComboUIController>();
            ScoreBoardManagerInstance = new(this);
            _spellController = new(this);
            GameOverSign = GameObject.Find("GameOverSign").GetComponentInChildren<Canvas>();
        }

        private void Start()
        {
            InputField.OnEnterText += JudgeInputText;

            WordSpawner.ManagerFoundSpawner(this);
            WordSpawner.OnWordSpawn += AddWord;

            GameOverSign.enabled = false;

            Debug.Log("TestWordManaget Statrs");

            if (PhotonNetwork.IsMasterClient) //Todo: When StartSpawn Should Be Adjusted.
                WordSpawner.StartSpawn();

            Debug.Log("TestWordManaget Start Spawn");

            _comboUIcontroller.SetWordManager(this);
        }

        private void Update()
        {
            UpdateComboTime();
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
            AcidWordController targetController;
            int score;

            if (isPersonalWord)
            {
                targetWord = PersonalSpawnedDictionary[word];
                targetController = targetWord.GetComponent<AcidWordController>();
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

                    if (PhotonNetwork.LocalPlayer == remover)
                    {
                        targetController = targetWord.GetComponent<AcidWordController>();
                        score = targetController.score;
                        AddScore(score);
                        AdjustCombo(1);
                    }
                }
            }
        }

        /// <summary>
        /// When Word Touch Ocean.
        /// </summary>
        [PunRPC]
        public void WordTouchedOcean(string wordSpell)
        {
            bool isPersonalWord = false;
            bool isPublicWord = false;

            isPersonalWord = PersonalSpawnedDictionary.ContainsKey(wordSpell);
            isPublicWord = PublicSpawnedDictionary.ContainsKey(wordSpell);

            if (isPersonalWord)
            {
                Destroy(PersonalSpawnedDictionary[wordSpell]); //Destroy Word.
                PersonalSpawnedDictionary.Remove(wordSpell);
                AdjustHP(-1);
            }
            else if (isPublicWord)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.Destroy(PublicSpawnedDictionary[wordSpell]); //Destroy Word.
                    PublicSpawnedDictionary.Remove(wordSpell);
                    PhotonView.RPC("AdjustHP", RpcTarget.All, -1);
                }
                else
                    return;
            }
        }

        /// <summary>
        /// When other client want to make word to another client, use this as RPC.
        /// </summary>
        /// <param name="wordSpell"></param>
        /// <param name="isPersonal"></param>
        [PunRPC]
        public void OrderSpawnerSpawnWord(bool isPublic, int spellLevel, int Loops, bool isSpellWord, int score = 0, float speed = 1f)
        {
            WordSpawner.SpawnOrderedWordRoutine(isPublic, spellLevel, Loops, isSpellWord, score, speed);
        }

        void JudgeInputText(string inputText)
        {
            if (IsDead)
                return;

            bool isPersonalWord = false;
            bool isPublicWord = false;
            bool isWrongWord = false;
            bool isSpell = false;

            isPersonalWord = PersonalSpawnedDictionary.ContainsKey(inputText);
            isPublicWord = PublicSpawnedDictionary.ContainsKey(inputText);
            isSpell = _spellController.SpellDictionary.ContainsKey(inputText);

            if (isSpell)
            {
                _spellController.SpellDictionary[inputText].TryUseSpell(Combo, PhotonNetwork.LocalPlayer, PhotonView);
                return;
            }

            if (isPersonalWord && isPublicWord)
                isPersonalWord = false;
            else if (!isPersonalWord && !isPublicWord)
                isWrongWord = true;


            if (isWrongWord)
            {
                if (Combo >= 1)
                    AdjustCombo(-1);
            }
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

        public void AdjustCombo(int comboAdjusted)
        {
            if (comboAdjusted == 0)
                return;

            PassedComboTime = MAXPASSEDCOMBOTIME;

            if (Combo >= 10 && comboAdjusted > 0)
            {
                _comboUIcontroller.UpdateCombo();
                Debug.Log("UpdateCombo");
                return;
            }

            Combo += comboAdjusted;
            _comboUIcontroller.SetComboMessage(Combo);
            Debug.Log(Combo + "Combo!!");

            if (comboAdjusted > 0)
                _comboUIcontroller.AddCombo(comboAdjusted);
            if (comboAdjusted < 0)
                _comboUIcontroller.RemoveCombo(comboAdjusted * -1);
        }

        void ResetCombo()
        {
            Combo = 0;
        }

        [PunRPC]
        public void AdjustScoreOfPlayer(int score, Player adjustedPlayer)
        {
            ScoreBoardManagerInstance.AdjustScoreOfPlayer(score, adjustedPlayer);
        }

        [PunRPC]
        public void SetPlayerDead(Player deadPlayer)
        {
            ScoreBoardManagerInstance.SetPlayerDead(deadPlayer);
        }

        private void UpdateComboTime()
        {
            if (Combo > 0)
            {
                PassedComboTime -= Time.deltaTime;
                if (PassedComboTime < 0)
                {
                    AdjustCombo(-1);
                }
            }
        }
    }
}