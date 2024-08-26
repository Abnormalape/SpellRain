using BHS.AcidRain.NetWork;
using BHS.AcidRain.UI;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

namespace BHS.AcidRain.GameManager
{
    public class GameManager : MonoBehaviour
    {
        public NetWorkManager _netWorkManager { get; private set; }
        public UIManger _uIManger { get; private set; }
        public GameSceneManager _gameSceneManager { get; private set; }
        public bool IsNetWorkConnected { get; private set; }

        private void Awake()
        {
            _netWorkManager = GetComponent<NetWorkManager>();
            _uIManger = GetComponent<UIManger>();
            _gameSceneManager = GetComponent<GameSceneManager>();
        }

        private void Start()
        {
            _netWorkManager.OnRoomListInRoomChanged += ClientRoomListChanged;
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

        private void ClientRoomListChanged(List<RoomInfo> changedRoomList)
        {
            _uIManger.InstantiateRoomListGameObject(changedRoomList);
        }
    }
}
