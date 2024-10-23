using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

namespace BHS.AcidRain.Buttons
{
    public class CreateNewRoomButton : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _roomNameHolder;
        [SerializeField] private TMP_InputField _passWordHolder;
        [SerializeField] private Toggle _isLockedHolder;


        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(Clicked);
        }

        public void Clicked()
        {
            Debug.Log("Create New Room Clicked");

            string roomName = _roomNameHolder.text;
            string passWord = _passWordHolder.text;
            bool isLocked = _isLockedHolder.isOn;

            int roonNameLength = roomName.Length;
            int emptylength = "".Length;
            bool isRoomNameEmpty = roomName == "";

            if (roomName == null || roomName == "")
            {
                Debug.Log("Please Enter Room Name");
                return;
            }

            if (isLocked && (passWord == null || passWord == ""))
            {
                Debug.Log("Please Enter Password When Room Locked");
                return;
            }

            MakeRoom(roomName, passWord, isLocked);
        }

        private void MakeRoom(string roomName, string passWord, bool isLocked)
        {
            RoomOptions roomOptions = new RoomOptions();

            roomOptions.MaxPlayers = 4;
            roomOptions.CleanupCacheOnLeave = true;

            roomOptions.IsOpen = true; //Can join room.
            roomOptions.IsVisible = !isLocked; //Can find room in lobby.

            if(!isLocked)
                PhotonNetwork.CreateRoom(roomName, roomOptions);
            else if(isLocked)
            {
                string timeString = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                roomName = $"{roomName}{passWord}{timeString}";
                Debug.Log(roomName);
                PhotonNetwork.CreateRoom(roomName,roomOptions);
            }

            Destroy(transform.root.gameObject);
        }
    }
}
