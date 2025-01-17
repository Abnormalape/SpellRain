using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BHS.AcidRain.NetWork
{
    public class NetWorkManager : MonoBehaviourPunCallbacks
    {
        private GameManager.GameManager _gameManager;

        private TextMeshProUGUI _buildDebugger; //Todo:

        public Dictionary<string, RoomInfo> RoomDictionaryInLobby { get; private set; }

        private void Awake()
        {
            _gameManager = GetComponent<GameManager.GameManager>();
            _buildDebugger = transform.GetComponentInChildren<TextMeshProUGUI>(); //Todo:

            RoomDictionaryInLobby = new();
        }

        private void Start()
        {
            PhotonNetwork.AutomaticallySyncScene = true; //Link to MasterClient's SceneChange.
            PhotonNetwork.ConnectUsingSettings(); //Todo: Game should Wait unitl Connection end.
        }

        //When "Multi Play" Button pressed.
        public void TryJoinLobby()
        {
            Debug.Log(SceneManager.GetActiveScene().name);
            PhotonNetwork.JoinLobby();
        }

        //Set, Add, Remove Room UI.
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("Room List Changed");

            List<RoomInfo> _tempRoomInfos = roomList;

            foreach (RoomInfo roominfo in _tempRoomInfos)
            {
                if (!roominfo.IsVisible)
                    continue;

                if (roominfo.RemovedFromList)
                {   
                    _gameManager.UIManger.RemoveRoomUI(roominfo);
                    RoomDictionaryInLobby.Remove(roominfo.Name);
                    continue;
                }
                if (RoomDictionaryInLobby.ContainsKey(roominfo.Name))
                {
                    _gameManager.UIManger.UpdateRoomUI(roominfo);
                    RoomDictionaryInLobby[roominfo.Name] = roominfo;
                }
                else if (!RoomDictionaryInLobby.ContainsKey(roominfo.Name))
                {
                    _gameManager.UIManger.AddRoomUI(roominfo);
                    RoomDictionaryInLobby.Add(roominfo.Name, roominfo);
                }
            }
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined Room");

            if(PhotonNetwork.IsMasterClient)
            {
                _gameManager.UIManger.InstantiateRoomWindow();
            }
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log($"Error Code : {returnCode} \nMessage : {message}");
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
    }
}