using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace BHS.AcidRain.Game
{
    /// <summary>
    /// It actually manages scoreboard.
    /// </summary>
    public class ScoreBoardManager
    {
        public ScoreBoardManager(WordManager ownerTestWordManager)
        {
            WordManager = ownerTestWordManager;
            WordManager.OnChangeScore += SendScoreToAllPlayer;

            RankingPositionHolder = WordManager.InputField.transform.root.Find("RankingPositionHolder"); //Todo:
            PersonalScoreBoardPrefab = Resources.Load(SCOREBOARDPATH) as GameObject;

            int positionNumber = 0;
            foreach (var player in PhotonNetwork.PlayerList)
            {
                //Set Each Rank's Position.
                RankingPositions[positionNumber]
                    = RankingPositionHolder.GetChild(positionNumber).position;

                //Instantiate ScoreBoard.
                ScoreBoardController tempPlayerData
                    = UnityEngine.Object.Instantiate(PersonalScoreBoardPrefab)
                    .GetComponent<ScoreBoardController>();

                //Set ScoreBoard's Data.
                if (player == PhotonNetwork.MasterClient) //Todo: Remove
                {
                    player.NickName = "MasterClient";
                }
                else
                {
                    player.NickName = "Client";
                }

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


        private readonly string SCOREBOARDPATH = "Prefabs/UIWindow/SingleScoreBoard";
        WordManager WordManager;
        Dictionary<Player, ScoreBoardController> PlayerDataDictionary = new(4);
        Transform RankingPositionHolder; //Todo:
        GameObject PersonalScoreBoardPrefab; //Todo:
        Vector3[] RankingPositions = new Vector3[4];


        /// <summary>
        /// When score changed, send data of score and sended player to all players.
        /// </summary>
        /// <param name="score">Score to send.</param>
        void SendScoreToAllPlayer(int score)
        {
            WordManager.PhotonView.RPC("AdjustScoreOfPlayer", RpcTarget.All, score, PhotonNetwork.LocalPlayer);
        }

        public void AdjustScoreOfPlayer(int score, Player adjustedPlayer)
        {
            PlayerDataDictionary[adjustedPlayer].Score = score;

            RearrangePlayerRanking();
            PlaceScoreBoardByRanking();
        }

        public void SetPlayerDead(Player deadPlayer)
        {
            PlayerDataDictionary[deadPlayer].SetDead();
            Debug.Log("Dead Player Rank : " + PlayerDataDictionary[deadPlayer].Ranking);
        }

        void RearrangePlayerRanking()
        {
            int playerCounts = PlayerDataDictionary.Count;
            List<ScoreBoardController> scoreList = new(playerCounts);

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rank">Use 0 for Find Last</param>
        /// <param name="findLast">Is for repeat. not use</param>
        /// <returns></returns>
        public Player FindRankedPlayer(int rank, bool findLast = false)
        {
            Player tempPlayer;
            int innerRank = rank - 1;
            bool findingLast = findLast;

            if (innerRank == -1) //Use for Find Last Ranked Player.
            {
                innerRank = PlayerDataDictionary.Count - 1;
                findingLast = true;
            }

            foreach (var playerData in PlayerDataDictionary)
            {
                if (playerData.Value.Ranking == innerRank)
                {
                    if (playerData.Value.IsDead == true)
                        continue;

                    tempPlayer = playerData.Key;
                    return tempPlayer;
                }
            }

            if (!findingLast)
            {
                if (rank == 0)
                    return null; //Error
                rank++;
            }
            else
            {
                if (rank == PlayerDataDictionary.Count - 1)
                    return null;

                if (rank == 0)
                    rank = PlayerDataDictionary.Count;

                rank--;
            }

            return FindRankedPlayer(rank, findingLast);
        }
    }
}
