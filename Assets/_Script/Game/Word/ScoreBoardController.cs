using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace BHS.AcidRain.Game
{
    /// <summary>
    /// This Controlls Each ScoreBoard
    /// </summary>
    public class ScoreBoardController : MonoBehaviour
    {
        public void SetTestPlayerData(int score, int ranking, Player player, string name)
        {
            Score = score;
            Ranking = ranking;
            Player = player;
            Name = name;
            IsDead = false;
        }


        public bool IsDead { get; private set; }
        public void SetDead() { if (!IsDead) IsDead = true; }

        public int Score;
        Player Player;
        string Name;
        Transform textHolder;
        TextMeshProUGUI RankingText;
        TextMeshProUGUI ScoreText;
        TextMeshProUGUI NameText;

        private int _ranking;
        public int Ranking
        {
            get => _ranking;
            set
            {
                _ranking = value;
                ChangeText();
            }
        }


        private void Awake()
        {
            textHolder = transform.GetChild(0).GetChild(0);
            RankingText = textHolder.Find("Rank").GetComponent<TextMeshProUGUI>();
            ScoreText = textHolder.Find("Score").GetComponent<TextMeshProUGUI>();
            NameText = textHolder.Find("UserName").GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            ChangeText();
        }

        private void ChangeText()
        {
            RankingText.text = (Ranking + 1).ToString();
            ScoreText.text = Score.ToString();

            if (Name == "" || Name == null)
            {
                Name = "null";
            }

            NameText.text = Name;
        }
    }
}
