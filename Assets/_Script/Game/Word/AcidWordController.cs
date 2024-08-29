using TMPro;
using UnityEngine;
using Photon.Pun;

namespace BHS.AcidRain.Game
{
    /// <summary>
    /// This class contains Word's Data, like Score.
    /// And makes Word fall down.
    /// And ETC.
    /// </summary>
    public class AcidWordController : MonoBehaviour
    {
        public int score { get; private set; }
        string wordSpell;
        float passedTime;
        float speed;
        TextMeshProUGUI textMeshProUGUI;
        PhotonView photonView;


        private void Awake()
        {
            textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
            photonView = GetComponent<PhotonView>();
        }

        private void Update()
        {
            passedTime += Time.deltaTime;
            if (passedTime >= 2f / (1f + (speed / 1f)))
            {
                passedTime = 0f;
                MoveDown();
            }
        }

        public void AdjustWord(int currentLevel, string wordSpell, bool isPublicWord, float speed = 1f)
        {
            if (isPublicWord)
            {
                score = currentLevel * 100;
                this.wordSpell = wordSpell;
                textMeshProUGUI.text = wordSpell;
                this.speed = speed;
            }
            else
            {
                photonView.RPC("AdjustPublicWord", RpcTarget.All, currentLevel, wordSpell, speed);
            }
        }

        [PunRPC]
        public void AdjustPublicWord(int currentLevel, string wordSpell, float speed = 1f)
        {
            score = currentLevel * 100;
            this.wordSpell = wordSpell;
            textMeshProUGUI.text = wordSpell;
            this.speed = speed;
        }

        void MoveDown()
        {
            float randonMovement = Random.Range(0.9f, 1.1f);
            transform.position += Vector3.down * randonMovement * 0.5f;
        }

        [PunRPC]
        public void AddToPublicDictionary(string Word)
        {
            FindFirstObjectByType<WordManager>().AddPublicWord(Word, this.gameObject); ;
        }
    }
}
