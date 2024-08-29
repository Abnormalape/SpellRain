using BHS.AcidRain.Game;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

namespace BHS.AcidRain.UI
{
    //Todo: This Class Must handle Only UI.
    public class ComboUIController : MonoBehaviour
    {
        [SerializeField] GameObject ComboBar;
        [SerializeField] List<GameObject> ComboBars = new();
        [SerializeField] private TextMeshProUGUI _comboText;
        [SerializeField] private GameObject _currentComboBar;
        private WordManager _wordManager;
        


        public GameObject CurrentComboBar
        {
            get => _currentComboBar;
            set
            {
                if (_currentComboBar != null)
                    _currentComboBar.GetComponent<Image>().color = Color.red;

                _currentComboBar = value;
            }
        }

        private void Awake()
        {

        }

        private void Start()
        {

        }

        private void Update()
        {
            if (CurrentComboBar != null)
            {
                CurrentComboBarDisappears();
            }
        }

        public void AddCombo(int inputCombos = 1)
        {
            for (int ix = 0; ix < inputCombos; ix++)
            {
                if (ComboBars.Count > 10)
                    return;

                CurrentComboBar = Instantiate(ComboBar, this.transform);
                ComboBars.Add(CurrentComboBar);
            }
        }

        public void RemoveCombo(int removedCombos = 1)
        {
            for (int ix = 0; ix < removedCombos; ix++)
            {
                if (ComboBars.Count <= 0)
                    return;

                GameObject previousComboBar = CurrentComboBar;

                if (ComboBars.Count - 2 < 0)
                    CurrentComboBar = null;
                else
                    CurrentComboBar = ComboBars[ComboBars.Count - 2];

                Debug.Log(ComboBars.Count);
                if (CurrentComboBar != null)
                    Debug.Log(CurrentComboBar.name);

                ComboBars.Remove(previousComboBar);
                Destroy(previousComboBar);

                if (ComboBars.Count <= 0)
                    return;
            }
        }

        public void CurrentComboBarDisappears() //Todo: use WordManager's passed Combo Time
        {
            Image image = CurrentComboBar.GetComponent<Image>();
            Color imagecolor = image.color;
            imagecolor.a = _wordManager.PassedComboTime / _wordManager.MAXPASSEDCOMBOTIME;
            image.color = imagecolor;
        }

        public void UpdateCombo()
        {
            CurrentComboBar.GetComponent<Image>().color = Color.red;
        }

        public void SetWordManager(WordManager wordManager)
        {
            _wordManager = wordManager;
        }

        public void SetComboMessage(int combo)
        {
            _comboText.text = $"{combo.ToString()}COMBO!!";
        }
    }
}
