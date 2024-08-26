using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace BHS.AcidRain.Test
{
    public class ForRPCTest : MonoBehaviour
    {
        private PhotonView _photonView;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }

        private void Start()
        {
            _photonView.RPC("AADD", RpcTarget.All);
        }


        [PunRPC]
        public void AADD()
        {
            Debug.Log("RPC Test");
        }
    }
}
