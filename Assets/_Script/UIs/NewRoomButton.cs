using Photon.Pun;
using UnityEngine;

namespace BHS.AcidRain.UI
{
    public class NewRoomButton : MonoBehaviour
    {
        public void NewRoomSetting()
        {
            Debug.Log("New Room Setting");
            Debug.Log("Room Name: Room Password: Room Code: ");

            PhotonNetwork.CreateRoom("TestRoom");
        }
    }
}