using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace BHS.AcidRain.Buttons
{
    public class CreateNewRoomButton : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(Clicked);
        }

        public void Clicked()
        {
            Debug.Log("Create New Room Clicked");
            PhotonNetwork.CreateRoom("TempRoom");
            Destroy(transform.root.gameObject);
        }
    }
}
