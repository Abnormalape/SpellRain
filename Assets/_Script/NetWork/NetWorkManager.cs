using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BHS.AcidRain.NetWork
{
    public class NetWorkManager : MonoBehaviourPunCallbacks
    {
        private void Start()
        {
            PhotonNetwork.AutomaticallySyncScene = true; //Todo:
            PhotonNetwork.ConnectUsingSettings();
        }

        public void TryJoinLobby()
        {
            PhotonNetwork.JoinLobby();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected To Server");
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Joined To Lobby");
            SceneManager.LoadScene("MultiLobby");
        }

        public override void OnLeftLobby()
        {
            Debug.Log("Left From Lobby");
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log($"Connection Fail: {cause}");
        }

        public delegate void ServerConnected();
        public event ServerConnected OnServerConnected;

        public delegate void ServerDisconnected();
        public event ServerDisconnected OnServerDisconnected;
    }
}