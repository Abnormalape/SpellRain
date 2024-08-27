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
    }
}
