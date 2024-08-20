using Photon.Pun;
using UnityEngine;

namespace BHS.AcidRain.Game
{
    /// <summary>
    /// Attached to GameObject which palaced on GameInstanceScene.
    /// </summary>
    public class GameInstanceController : MonoBehaviour
    {
        private WordSpawner _wordSpawner;

        private void Awake()
        {
            _wordSpawner = transform.root.Find("WordSpawner").GetComponent<WordSpawner>();
        }

        /// <summary>
        /// Start Method Must runs when all client load Scene.
        /// </summary>
        private void Start() //Todo: wait until all player load game
        {
#if !UNITY_EDITOR
            if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(_wordSpawner.WordSpawn());
            }
#endif

#if UNITY_EDITOR
            StartCoroutine(_wordSpawner.WordSpawn());
#endif

        }
    }
}
