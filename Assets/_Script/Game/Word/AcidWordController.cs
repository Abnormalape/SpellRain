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
        WordManager wordManager;

        private void Awake()
        {
            textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
            photonView = GetComponent<PhotonView>();
            this.wordManager = FindFirstObjectByType<WordManager>();
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

        public void AdjustWord(int currentLevel, string wordSpell, bool isPublicWord, int inputScore = 0, float speed = 1f)
        {
            if (isPublicWord)
            {
                if (inputScore == 0)
                    score = currentLevel * 100;
                else
                    score = inputScore;

                this.wordSpell = wordSpell;
                textMeshProUGUI.text = wordSpell;
                textMeshProUGUI.color = Color.red;
                this.speed = speed;
                if (score >= 1000)
                {
                    textMeshProUGUI.color = Color.blue;
                }
            }
            else
            {
                photonView.RPC("AdjustPublicWord", RpcTarget.All, currentLevel, wordSpell, inputScore, speed);
            }
        }

        [PunRPC]
        public void AdjustPublicWord(int currentLevel, string wordSpell, int inputScore = 0, float speed = 1f)
        {
            if (inputScore == 0)
                score = currentLevel * 100;
            else
                score = inputScore;

            this.wordSpell = wordSpell;
            textMeshProUGUI.text = wordSpell;
            textMeshProUGUI.color = Color.black;
            this.speed = speed;

            if (score >= 1000)
            {
                textMeshProUGUI.color = Color.blue;
            }
        }

        void MoveDown()
        {
            float randonMovement = Random.Range(0.9f, 1.1f);
            transform.position += Vector3.down * randonMovement * 0.5f;

            if (transform.position.y <= -2.75f)
            {
                wordManager.WordTouchedOcean(wordSpell);
            }
        }

        [PunRPC]
        public void AddToPublicDictionary(string Word)
        {
            FindFirstObjectByType<WordManager>().AddPublicWord(Word, this.gameObject); ;
        }
    }
}
