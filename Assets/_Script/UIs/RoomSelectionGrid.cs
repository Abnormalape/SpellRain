using UnityEngine;
using UnityEngine.UI;

namespace BHS.AcidRain.UI
{
    public class RoomSelectionGrid : MonoBehaviour
    {
        private UIManger _uIManager;

        private void Awake()
        {
            _uIManager = FindFirstObjectByType<UIManger>();
            _uIManager.RoomSelectionGrid = this.GetComponent<GridLayoutGroup>();
        }

        private void Start()
        {
            
        }
    }
}
