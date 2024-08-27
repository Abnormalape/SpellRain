using Photon.Pun;
using UnityEngine;

namespace BHS.AcidRain.Game
{
    public class AcidWordEventHandler : MonoBehaviour
    {
        public delegate void EnterOcean();
        public event EnterOcean OnEnterOcean;

        public delegate void ShutDown(); //Typed
        public event ShutDown OnShutDown;

        private void Start()
        {
            OnEnterOcean += DestroyWord;
            OnShutDown += DestroyWord;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ocean"))
            {
                OnEnterOcean();
            }
        }

        public void DestroyWord()
        {
            Debug.Log("Word Destroyed");
            Destroy(this.gameObject);

            // PhotonNetwork.Destroy(this.gameObject); //Todo:
        }

        public void RunShutDownEvent()
        {
            OnShutDown();
        }
    }
}
