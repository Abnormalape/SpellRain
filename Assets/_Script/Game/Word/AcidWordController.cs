using TMPro;
using Unity.Jobs;
using UnityEngine;

namespace BHS.AcidRain.Game
{
    public class AcidWordController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
        private AcidWordEventHandler _acidWordEventController;
        public int WordScore { get; private set; } = 1; //Todo:

        private string _wordSpell;
        public string WordSpell 
        { 
            get => _wordSpell; 
            set 
            { 
                _wordSpell = value;
                _textMeshProUGUI.text = _wordSpell;
            }
        }

        private void Awake()
        {
            _acidWordEventController = GetComponent<AcidWordEventHandler>();

            if(_textMeshProUGUI == null)
                _textMeshProUGUI = this.GetComponentInChildren<TextMeshProUGUI>();
        }
    }
}