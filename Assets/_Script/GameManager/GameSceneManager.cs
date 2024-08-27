using BHS.AcidRain.NetWork;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BHS.AcidRain.GameManager
{
    public class GameSceneManager : MonoBehaviour
    {
        private GameManager _gameManager;

        private void Awake()
        {
            _gameManager = GetComponent<GameManager>();
        }

        public void GameSceneChange(string sceneName, RoomManager caller = null)
        {
            StartCoroutine(LoadSceneAndWaitUntilLoadEnds(sceneName, caller));
        }

        private IEnumerator LoadSceneAndWaitUntilLoadEnds(string sceneName, RoomManager caller = null)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            if (sceneName == "MultiLobby")
            {
                _gameManager.NetWorkManager.TryJoinLobby();
            }
            else if (sceneName == "AcidRainBattle")
            {
                if(caller !=null)
                {
                    caller.OnEnterRoomComplete();
                }
            }
        }
    }
}
