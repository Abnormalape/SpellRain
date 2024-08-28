using BHS.AcidRain.Game;
using BHS.AcidRain.GameManager;
using BHS.AcidRain.UI;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BHS.AcidRain.NetWork
{
    public class RoomManager : MonoBehaviour
    {
        public PhotonView PhotonView { get; private set; }
        private GameSceneManager _gameSceneManager; //Todo: Please Use Singleton Pattern.
        private SingleRoomController _singleRoomController;
        private WordSpawner _wordSpawner;

        private int _playersOnGame;

        private int _wordSpawnerSpawnCompleteCount = 0;
        public int WordSpawnerSpawnCompleteCount
        {
            get => _wordSpawnerSpawnCompleteCount;
            set
            {
                _wordSpawnerSpawnCompleteCount = value;

                if (_wordSpawnerSpawnCompleteCount >= _playersOnGame)
                {
                    StartSpawnWord();
                }
            }
        }

        private int _sceneLoadCompleteCount = 0;
        public int SceneLoadCompleteCount
        {
            get
            {
                return _sceneLoadCompleteCount;
            }
            set
            {
                _sceneLoadCompleteCount = value;
                OnSceneLoadCompleteCountChanged(_sceneLoadCompleteCount);
            }
        }

        private void Awake()
        {
            PhotonView = GetComponent<PhotonView>();
            _gameSceneManager = FindFirstObjectByType<GameSceneManager>();
            _singleRoomController = FindFirstObjectByType<SingleRoomController>();
        }

        private void Start()
        {
            _singleRoomController.RoomManager = this;
        }

        public void SceneChangeOrder(int playerCount)
        {
            _playersOnGame = playerCount;
            PhotonView.RPC("SceneChange", RpcTarget.All);
        }

        [PunRPC]
        private void SceneChange()
        {
            _gameSceneManager.GameSceneChange("AcidRainBattle", this);
        }

        public void OnEnterRoomComplete()
        {
            PhotonView.RPC("AddSceneLoadCompleteCount", RpcTarget.MasterClient);
        }

        [PunRPC]
        public void AddSceneLoadCompleteCount()
        {
            Debug.Log("One Client Load Complete!!");
            SceneLoadCompleteCount++;
        }

        private void OnSceneLoadCompleteCountChanged(int loadCompleteCount)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            if (loadCompleteCount >= _playersOnGame)
            {
                Debug.Log("All Player Load Complete!!");
                Debug.Log($"{loadCompleteCount}Players On Game!!");
                Debug.Log($"Current Scene Name is : {SceneManager.GetActiveScene().name}!!");
                Debug.Log("Ready To Start Spawn Word!!");
                //Todo: Make Masterclient's Wordspawner Spawn Words.

                //1. Summon WordSpawner To All Client
                //Todo:
                PhotonNetwork.Instantiate("Prefabs/GameFunction/TestWordManager", Vector3.zero, Quaternion.identity);

                //2. Use RPC to Make Word To All Client
            }
        }

        public void SetWordSpawner(WordSpawner wordSpawner)
        {
            _wordSpawner = wordSpawner;
        }

        [PunRPC]
        public void WordSpawnerSpawnComplete()
        {
            WordSpawnerSpawnCompleteCount++;
        }

        private void StartSpawnWord()
        {
            Debug.Log("All Spawner Spawn Complete!!");
            StartCoroutine(_wordSpawner.WordSpawn());
        }
    }
}