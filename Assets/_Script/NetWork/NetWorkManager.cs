using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BHS.AcidRain.NetWork
{
    public class NetWorkManager : MonoBehaviourPunCallbacks
    {
        private GameManager.GameManager _gameManager;

        public List<RoomInfo> RoomListInRoom { get; private set; }

        private void Awake()
        {
            _gameManager = GetComponent<GameManager.GameManager>();
            RoomListInRoom = new();
        }

        private void Start()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }

        //Event on "Multi Play" Button.
        public void TryJoinLobby()
        {
            Debug.Log(SceneManager.GetActiveScene().name);
            PhotonNetwork.JoinLobby();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected To Server");
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Joined To Lobby");
        }

        public override void OnLeftLobby()
        {
            Debug.Log("Left From Lobby");
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log($"Connection Fail: {cause}");
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("Room List Changed");
            RoomListInRoom = roomList;
            OnRoomListInRoomChanged(RoomListInRoom);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined Room");
            _gameManager._uIManger.InstantiateRoomWindow();
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log($"Error Code : {returnCode} \nMessage : {message}");
        }

        public delegate void RoomListInRoomChanged(List<RoomInfo> changedRoomList);
        public event RoomListInRoomChanged OnRoomListInRoomChanged;
    }
}