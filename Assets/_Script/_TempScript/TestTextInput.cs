using TMPro;
using UnityEngine;

namespace BHS.AcidRain.Test
{
    public class TestTextInput : MonoBehaviour
    {
        public delegate void EnterText(string Text);
        public event EnterText OnEnterText;

        private TMP_InputField _inputField;

        private void Awake()
        {
            _inputField = GetComponent<TMP_InputField>();
        }

        private void Start()
        {
            _inputField.onEndEdit.AddListener(InputBoxControl);
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
