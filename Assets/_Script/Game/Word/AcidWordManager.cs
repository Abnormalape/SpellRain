using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BHS.AcidRain.Game
{
    public class AcidWordManager : MonoBehaviour
    {
        private WordSpawner _wordSpawner;
        [SerializeField] private GameTextInput _textInput;
        [SerializeField] private Dictionary<string, AcidWordController> _acidWords;

        private void Awake()
        {
            if (_textInput == null) { Debug.Log("No Text Input"); }
            _wordSpawner = GetComponent<WordSpawner>();
            _acidWords = new(50);
        }

        private void Start()
        {
            _wordSpawner.OnSummonWord += AddWord;
            _textInput.OnEnterText += JudgeText;
        }

        private void AddWord(string wordName, AcidWordController controller)
        {
            _acidWords.TryAdd(wordName, controller); //Todo: Delete
        }

        private void JudgeText(string inputText)
        {
            if (inputText.StartsWith("spell")) //Todo:
            {
                AttackPlayer();
                return;
            }

            AcidWordController tempController;

            _acidWords.TryGetValue(inputText, out tempController);

            if (tempController == null)
            {
                Debug.Log("No Word Found");
            }
            else if (tempController != null)
            {
                Debug.Log("Word Found");

                AddScore(tempController); //Todo: 
                RemoveText(tempController); //Todo: If "Black" Word => Other Player's Word Also Remove
            }
        }

        private void RemoveText(AcidWordController tempController) //Todo:
        {
            PhotonNetwork.Destroy(tempController.gameObject);
        }

        private void AddScore(AcidWordController tempController) //Todo:
        {
            Debug.Log(tempController.WordScore);
        }

        private void AttackPlayer() //Todo:
        {
            Debug.Log("Spell Attack!!!");
        }
    }
}
