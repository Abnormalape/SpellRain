using BHS.AcidRain.GameManager;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace BHS.AcidRain.UI
{
    public class SingleRoomExpand : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TextMeshProUGUI CurrentMaxPlayer;
        private int _currentPlayer = 0;

        [SerializeField] private Button _gameStartButton; //Todo:

        public int CurrentPlayer 
        { 
            get => _currentPlayer;
            private set
            {
                if (_currentPlayer != value)
                {
                    _currentPlayer = value;
                    OnPlayerCountsInRoonChanged();
                }
            }
        }

        private void Awake()
        {
            
        }

        private void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("You Are Master Client!");
            }

            CurrentPlayer = PhotonNetwork.CurrentRoom.PlayerCount;
            Debug.Log($"Current Players : {_currentPlayer}");

            _gameStartButton.onClick.AddListener(MoveToGameScene);
        }

        private void Update()
        {
            
        }

        private void OnPlayerCountsInRoonChanged()
        {
            ChangeCurrentPlayerText();
        }

        private void ChangeCurrentPlayerText()
        {
            CurrentMaxPlayer.text = $"{_currentPlayer}/4";
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log("New Player Enter Room");
            CurrentPlayer++;
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log("New Player Exit Room");
            CurrentPlayer--;
        }

        private void MoveToGameScene()
        {
            FindFirstObjectByType<GameSceneManager>().GameSceneChange("AcidRainBattle");
        }
    }
}
