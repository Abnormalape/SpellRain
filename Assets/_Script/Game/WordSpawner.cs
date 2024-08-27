using BHS.AcidRain.NetWork;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BHS.AcidRain.Game
{
    public class WordSpawner : MonoBehaviour
    {
        public delegate void SummonWord(string wordName, AcidWordController controller);
        public event SummonWord OnSummonWord;

        private Transform[] _spawnPoints;
        private RoomManager _roomManager;
        //private GameObject _acidWord;

        private Dictionary<string, GameObject> _personalWordDictionary = new();
        private Dictionary<string, GameObject> _publicWordDictionary = new();

        private void Awake()
        {
            _spawnPoints = this.GetComponentsInChildren<Transform>();
            _roomManager = FindFirstObjectByType<RoomManager>();
            //_acidWord = Resources.Load("Prefabs/Word/AcidWord") as GameObject;
        }

        private void Start()
        {
            _roomManager.SetWordSpawner(this);
            _roomManager.PhotonView.RPC("WordSpawnerSpawnComplete", RpcTarget.MasterClient);
        }

        public IEnumerator WordSpawn() //Todo: For Multi Player, Use Instantiate
        {
            //PhotonNetwork.Instantiate();

            int _spawnPointLength = _spawnPoints.Length;
            int _spawnCount = Random.Range(8, 10); //Todo: Word Number
            Debug.Log($"Counts:{_spawnCount}");

            for (int ix = 0; ix < _spawnCount; ix++)
            {
                int _tempSpawnPoint = Random.Range(1, _spawnPointLength);

                string wordSpell = SelectWord(); //Todo:

                //GameObject SummonedWord =
                //    Instantiate(_acidWord, _spawnPoints[_tempSpawnPoint].position, Quaternion.identity, null); //Todo:

                if (PhotonNetwork.IsMasterClient)
                {
                    GameObject SummonedWord =
                        PhotonNetwork.Instantiate("Prefabs/Word/AcidWord", _spawnPoints[_tempSpawnPoint].position, Quaternion.identity);

                    SummonedWord.GetComponent<PhotonView>().RPC("SetWordSpell", RpcTarget.All, wordSpell); //Todo:

                    // SummonedWord.GetComponent<AcidWordController>().WordSpell = wordSpell; //Todo:

                    OnSummonWord(
                        wordSpell,
                        SummonedWord.GetComponent<AcidWordController>()); //Todo:

                    float _waitTime = Random.Range(0f, 1f); //Todo:
                    yield return new WaitForSeconds(_waitTime); //Todo:
                }
            }
        }


        int tempint = 0;
        string[] tempstring = { "afs", "dsa", "asdf", "faee", "eeass", "etqqe", "ple", "uenjf", "emean", "eetgha" };
        private string SelectWord() //Todo:
        {
            string tempString = tempstring[tempint];
            tempint++;

            return tempString;
        }

        [PunRPC]
        private void SummonWordToSinglePlayer() //Todo:
        {
            GameObject gameObject = new GameObject();
            Instantiate(gameObject);

            _personalWordDictionary.Add(gameObject.name, gameObject);
        }

        [PunRPC]
        public void SummonWordToAllPlayer() //Todo:
        {
            string prefabPath = "AA";
            PhotonNetwork.Instantiate(prefabPath, Vector3.zero, Quaternion.identity);

            _publicWordDictionary.Add(gameObject.name, gameObject);
        }
    }
}