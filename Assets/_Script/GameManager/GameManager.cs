using BHS.AcidRain.NetWork;
using BHS.AcidRain.UI;
using UnityEngine;

namespace BHS.AcidRain.GameManager
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject RPCTESTPREFAB;

        public NetWorkManager NetWorkManager { get; private set; }
        public UIManger UIManger { get; private set; }
        public GameSceneManager GameSceneManager { get; private set; }
        public bool IsNetWorkConnected { get; private set; }


        private void Awake()
        {
            NetWorkManager = GetComponent<NetWorkManager>();
            UIManger = GetComponent<UIManger>();
            GameSceneManager = GetComponent<GameSceneManager>();
        }

        /// <summary>
        /// Enter Title.
        /// </summary>
        public void StartGame()
        {
            IsNetWorkConnected = true;
        }

        public void StopGame()
        {

        }

        /// <summary>
        /// Show "Disconnect" UI, with Quit Button and Retry Button.
        /// If Press Quit, Game End.
        /// </summary>
        public void EndGame()
        {
            IsNetWorkConnected = false;
        }
    }
}
