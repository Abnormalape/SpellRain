using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BHS.AcidRain.Game
{
    public class GameTextInput : MonoBehaviour
    {
        public delegate void EnterText(string Text);
        public event EnterText OnEnterText;

        private TMP_InputField _inputField;

        private void Awake()
        {
            _inputField = GetComponent<TMP_InputField>();
            if( _inputField != null)
            {
                Debug.Log("Input Field exsist");
            }

            _inputField.onEndEdit.AddListener(InputBoxControl);
        }

        private void Start()
        {
            _inputField.ActivateInputField();
        }

        public void InputBoxControl(string input)
        {
            if (input == "")
            {
                _inputField.ActivateInputField();
                return; 
            }

            Debug.Log(input);

            _inputField.text = "";
            OnEnterText(input);

            _inputField.ActivateInputField();
        }
    }
}