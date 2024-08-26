using Photon.Pun;
using System.Collections;
using UnityEngine;

namespace BHS.AcidRain.Game
{
    public class WordSpawner : MonoBehaviour
    {
        public delegate void SummonWord(string wordName, AcidWordController controller);
        public event SummonWord OnSummonWord;

        private Transform[] _spawnPoints;
        private GameObject _acidWord;

        private void Awake()
        {
            _spawnPoints = this.GetComponentsInChildren<Transform>();
            _acidWord = Resources.Load("Prefabs/Word/AcidWord") as GameObject;
        }

        private void Start()
        {
            OnSummonWord += TempEventMethod;//Todo: Delete
        }

        public IEnumerator WordSpawn() //Todo: For Multi Player, Use Instantiate
        {
            //PhotonNetwork.Instantiate();

            int _spawnPointLength = _spawnPoints.Length;
            int _spawnCount = Random.Range(1, 10); //Todo: Word Number
            Debug.Log($"Counts:{_spawnCount}");

            for (int ix = 0; ix < _spawnCount; ix++)
            {
                int _tempSpawnPoint = Random.Range(1, _spawnPointLength);

                string wordSpell = SelectWord(); //Todo:

                //GameObject SummonedWord =
                //    Instantiate(_acidWord, _spawnPoints[_tempSpawnPoint].position, Quaternion.identity, null); //Todo:

                GameObject SummonedWord =
                    PhotonNetwork.Instantiate("Prefabs/Word/AcidWord", _spawnPoints[_tempSpawnPoint].position, Quaternion.identity);

                SummonedWord.GetComponent<AcidWordController>().WordSpell = wordSpell; //Todo:

                OnSummonWord(
                    wordSpell,
                    SummonedWord.GetComponent<AcidWordController>()); //Todo:

                float _waitTime = Random.Range(0f,1f); //Todo:
                yield return new WaitForSeconds(_waitTime); //Todo:
            }
        }


        int tempint = 0;
        string[] tempstring = {"afs","dsa","asdf","faee","eeass","etqqe","ple","uenjf","emean","eetgha" };
        private string SelectWord() //Todo:
        {
            string tempString = tempstring[tempint];
            tempint++;

            return tempString;
        }

        private void TempEventMethod(string aa, AcidWordController bb) //Todo: Delete
        {
            Debug.Log("OnSummonWord Runs");
            Debug.Log($"{aa}: {bb.name}");
        }
    }
}