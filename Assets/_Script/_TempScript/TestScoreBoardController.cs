using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace BHS.AcidRain.Test
{
    /// <summary>
    /// This Controlls Each ScoreBoard
    /// </summary>
    public class TestScoreBoardController : MonoBehaviour
    {
        public void SetTestPlayerData(int score, int ranking, Player player, string Name)
        {
            Score = score;
            Ranking = ranking;
            Player = player;
            Name = name;
        }


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

            if(Name == "" || Name == null)
            {
                int RI = Random.Range(0, 100000);
                Name = RI.ToString();
            }

            NameText.text = Name;
        }
    }
}
