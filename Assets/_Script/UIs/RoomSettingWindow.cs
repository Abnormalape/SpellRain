using UnityEngine;

namespace BHS.AcidRain.UI
{
    public class RoomSettingWindow : MonoBehaviour
    {
        public void CloseRoomSettingWindow()
        {
            Destroy(this.gameObject);
        }
    }
}