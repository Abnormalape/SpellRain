using BHS.AcidRain.GameManager;
using UnityEngine;
using UnityEngine.UI;

namespace BHS.AcidRain.Buttons
{
    public class MultiPlayButton : MonoBehaviour
    {
        [SerializeField] private GameObject _managers;
        [SerializeField] private string _sceneToGo;
        private GameSceneManager _gameSceneManager;

        private void Awake()
        {
            if (_managers == null)
                _managers = FindFirstObjectByType<GameManager.GameManager>().gameObject;
            if (_gameSceneManager == null)
                    _gameSceneManager = _managers.GetComponent<GameSceneManager>();
            _sceneToGo = "MultiLobby";
        }

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => _gameSceneManager.GameSceneChange($"{_sceneToGo}"));
        }
    }
}
