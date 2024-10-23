using BHS.AcidRain.NetWork;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BHS.AcidRain.UI
{
    public class SingleRoomController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TextMeshProUGUI _currentMaxPlayer;
        [SerializeField] private TextMeshProUGUI _readyCurrentPlayer;
        private int _currentPlayer = 0;
        private int _readyPlayer = 0;

        [SerializeField] private Button _gameStartReadyButton;
        private TextMeshProUGUI _buttonText;

        private PhotonView _photonView;
        private RoomManager _roomManager;

        private bool _isReady = false;

        public RoomManager RoomManager;

        public int CurrentPlayer
        {
            get => _currentPlayer;
            private set
            {
                if (_currentPlayer != value)
                {
                    _currentPlayer = value;
                    OnPlayerCountsInRoonChanged();
                    ReadyPlayerCountUpdated();
                }
            }
        }

        private void Awake()
        {
            _buttonText = _gameStartReadyButton.GetComponentInChildren<TextMeshProUGUI>();
            _photonView = GetComponent<PhotonView>();
        }

        private void Start()
        {
            JudgeMasterClient();

            CurrentPlayer = PhotonNetwork.CurrentRoom.PlayerCount;
            Debug.Log($"Current Players : {_currentPlayer}");
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
            _currentMaxPlayer.text = $"{_currentPlayer}/4";
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log("Player Enter Room");
            CurrentPlayer++;

            if (PhotonNetwork.IsMasterClient)
            {
                _photonView.RPC("ReadyPlayerCountUpdated", newPlayer, _readyPlayer);
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log("Player Exit Room");
            CurrentPlayer--;
        }

        private void TryStartGame()
        {
            if (_readyPlayer == CurrentPlayer)
            {
                PhotonNetwork.CurrentRoom.IsVisible = false;
                this.RoomManager.SceneChangeOrder(CurrentPlayer);
            }
            else
            {
                Debug.Log("Not All Player Ready");
            }
        }

        private void GameReady()
        {
            if (!_isReady)
            {
                _readyPlayer++;
                _isReady = true;
            }
            else
            {
                _readyPlayer--;
                _isReady = false;
            }
            
            _photonView.RPC("ReadyPlayerCountUpdated", RpcTarget.All, _readyPlayer);
        }

        [PunRPC]
        private void ReadyPlayerCountUpdated(int readyPlayer = -1)
        {
            if (readyPlayer != -1)
                _readyPlayer = readyPlayer;

            _readyCurrentPlayer.text = $"{_readyPlayer}/{CurrentPlayer}";
        }

        private void JudgeMasterClient()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("You Are Master Client!");
                PhotonNetwork.Instantiate("Prefabs/Room/RoomManager", Vector3.zero,Quaternion.identity);

                _buttonText.text = "Start Game";

                _readyPlayer++;
                CurrentPlayer++;
                _photonView.RPC("ReadyPlayerCountUpdated", RpcTarget.All, _readyPlayer);

                _gameStartReadyButton.onClick.AddListener(TryStartGame);
            }
            else if (!PhotonNetwork.IsMasterClient)
            {
                Debug.Log("You Are Not Master Client!");
                _buttonText.text = "Ready";
                _gameStartReadyButton.onClick.AddListener(GameReady);
            }
        }

        public void CloseRoomExpandWindow()
        {
            PhotonNetwork.LeaveRoom();
        }
    }
}
