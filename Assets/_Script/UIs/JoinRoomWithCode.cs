using BHS.AcidRain.NetWork;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BHS.AcidRain.UI
{
    public class JoinRoomWithCode : MonoBehaviour
    {
        [SerializeField] private GameObject warnWindow;
        [SerializeField] private Button JoinRoomButton;
        [SerializeField] private TMP_InputField JoinRoomText;
        private NetWorkManager netWorkManager;


        private void Awake()
        {
            netWorkManager = FindFirstObjectByType<NetWorkManager>();
        }

        private void Start()
        {
            JoinRoomButton.onClick.AddListener(TryJoinRoom);
        }

        private void TryJoinRoom()
        {
            JudgeEnteredCode();
        }

        private void JudgeEnteredCode()
        {
            if (netWorkManager.RoomDictionaryInLobby.ContainsKey(JoinRoomText.text))
            {
                JoinRoom(JoinRoomText.text);
            }
            else
            {
                JoinRoomFailed();
            }
        }
        private void JoinRoom(string roomCode)
        {
            PhotonNetwork.JoinRoom(roomCode);
        }

        private void JoinRoomFailed()
        {
            warnWindow.SetActive(true);
        }

        public void CloseWarnWindow()
        {
            warnWindow.SetActive(false);
        }
    }
}
