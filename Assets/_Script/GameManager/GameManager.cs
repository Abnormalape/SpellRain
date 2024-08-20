using BHS.AcidRain.NetWork;
using BHS.AcidRain.UI;
using UnityEngine;

namespace BHS.AcidRain.GameManager
{
    public class GameManager : MonoBehaviour
    {
        private NetWorkManager _netWorkManager;
        private UIManger _UIManger;

        public bool IsNetWorkConnected { get; private set; }

        private void Awake()
        {
            _netWorkManager = GetComponent<NetWorkManager>();
            _UIManger = GetComponent<UIManger>();
        }

        private void Start()
        {
            _netWorkManager.OnServerConnected += StartGame;
            _netWorkManager.OnServerDisconnected += EndGame;
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
