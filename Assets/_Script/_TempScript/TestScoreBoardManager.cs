using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

namespace BHS.AcidRain.Test
{
    /// <summary>
    /// It actually manages scoreboard.
    /// </summary>
    public class TestScoreBoardManager
    {
        public TestScoreBoardManager(TestWordManager ownerTestWordManager)
        {
            TestWordManager = ownerTestWordManager;
            TestWordManager.OnChangeScore += SendScoreToAllPlayer;

            RankingPositionHolder = TestWordManager.InputField.transform.root.Find("RankingPositionHolder"); //Todo:
            PersonalScoreBoardPrefab = Resources.Load("Prefabs/UIWindow/SingleScoreBoard") as GameObject;

            int positionNumber = 0;
            foreach (var player in PhotonNetwork.PlayerList)
            {
                //Set Each Rank's Position.
                RankingPositions[positionNumber]
                    = RankingPositionHolder.GetChild(positionNumber).position;

                //Instantiate ScoreBoard.
                TestScoreBoardController tempPlayerData
                    = UnityEngine.Object.Instantiate(PersonalScoreBoardPrefab)
                    .GetComponent<TestScoreBoardController>();

                //Set ScoreBoard's Data.
                tempPlayerData.SetTestPlayerData(0, positionNumber, player, player.NickName);

                //Add PlayerData To Dictionary.
                PlayerDataDictionary.Add(player, tempPlayerData);

                //Set ScoreBoard's Position.
                PlayerDataDictionary[player].transform.GetChild(0).GetChild(0)
                    .GetComponent<RectTransform>().position = RankingPositions[positionNumber];
                    //.position = RankingPositions[positionNumber];

                //Move To Next.
                positionNumber++;
            }
        }


        TestWordManager TestWordManager;
        Dictionary<Player, TestScoreBoardController> PlayerDataDictionary = new(4);
        Transform RankingPositionHolder; //Todo:
        GameObject PersonalScoreBoardPrefab; //Todo:
        Vector3[] RankingPositions = new Vector3[4];


        /// <summary>
        /// When score changed, send data of score and sended player to all players.
        /// </summary>
        /// <param name="score">Score to send.</param>
        void SendScoreToAllPlayer(int score)
        {
            TestWordManager.PhotonView.RPC("AdjustScoreOfPlayer", RpcTarget.All, score, PhotonNetwork.LocalPlayer);
        }

        public void AdjustScoreOfPlayer(int score, Player adjustedPlayer)
        {
            PlayerDataDictionary[adjustedPlayer].Score = score;

            RearrangePlayerRanking();
            PlaceScoreBoardByRanking();
        }

        void RearrangePlayerRanking()
        {
            int playerCounts = PlayerDataDictionary.Count;
            List<TestScoreBoardController> scoreList = new(playerCounts);

            foreach (var playerData in PlayerDataDictionary)
            {
                scoreList.Add(playerData.Value);
            }

            scoreList.Sort((a, b) => b.Score.CompareTo(a.Score));

            for (int ix = 0; ix < scoreList.Count; ix++)
            {
                scoreList[ix].Ranking = ix;
            }
        }

        void PlaceScoreBoardByRanking()
        {
            foreach (var playerData in PlayerDataDictionary)
            {
                playerData.Value.transform.GetChild(0).GetChild(0)
                    .GetComponent<RectTransform>().position 
                    = RankingPositions[playerData.Value.Ranking];
                    //.transform.position
                    //= RankingPositions[playerData.Value.Ranking];
            }
        }
    }
}
