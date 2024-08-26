using BHS.AcidRain.UI;
using UnityEngine;
using UnityEngine.UI;

namespace BHS.AcidRain.Buttons
{
    public class NewRoomButton : MonoBehaviour
    {
        [SerializeField] private GameObject _managers;

        private void Awake()
        {
            if (_managers == null)
                _managers = FindFirstObjectByType<GameManager.GameManager>().gameObject;
        }

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => 
            _managers.GetComponent<UIManger>().InstantiateRoomSettingWindow());
        }
    }
}
