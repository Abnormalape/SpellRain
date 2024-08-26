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

        public void GameSceneChange(string sceneName)
        {
            StartCoroutine(LoadSceneAndWaitUntilLoadEnds(sceneName));
        }
        private IEnumerator LoadSceneAndWaitUntilLoadEnds(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            _gameManager._netWorkManager.TryJoinLobby();
        }
    }
}
