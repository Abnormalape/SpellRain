using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BHS.AcidRain.UI
{
    public class UIManger : MonoBehaviour
    {
        [SerializeField] private GameObject SingleRoomUIPrefab;
        [SerializeField] private GameObject SingleRoomWindowPrefab;
        [SerializeField] private GameObject SingleRoomSettingWindowPrefab;

        private Dictionary<string, GameObject> RoomsUI = new();
        private GridLayoutGroup _roomSelectionGrid;

        private List<GameObject> PopUpButtons = new(5);

        public GridLayoutGroup RoomSelectionGrid { get => _roomSelectionGrid; set => _roomSelectionGrid = value; }

        private void Awake()
        {
            if (SingleRoomUIPrefab == null)
                SingleRoomUIPrefab = Resources.Load("Prefabs/RoomPrefab") as GameObject;
            if (SingleRoomWindowPrefab == null)
                SingleRoomWindowPrefab = Resources.Load("Prefabs/UIWindow/RoomWindow") as GameObject;
            if (SingleRoomSettingWindowPrefab == null)
                SingleRoomSettingWindowPrefab = Resources.Load("Prefabs/UIWindow/RoomSettingWindow") as GameObject;
        }

        public void InstantiateRoomListGameObject(List<RoomInfo> changedRoomList)
        {
            List<RoomInfo> roomListBuffer = changedRoomList;

            Debug.Log("I'm Gonna Instantiate Room List GameObjcet!!");
            Debug.Log($"There are {roomListBuffer.Count} Rooms in Lobby!!");

            for (int i = 0; i < roomListBuffer.Count; i++)
            {
                string roomName = roomListBuffer[i].Name;
                GameObject roomMade = Instantiate(SingleRoomUIPrefab, _roomSelectionGrid.gameObject.transform);
                roomMade.GetComponent<SingleRoomUI>().SingleRoomInitializer(roomName);
                RoomsUI.Add(roomName, roomMade);
            }

            if (changedRoomList.Count == 0)
            {
                string roomName = "Empty Room";
                GameObject roomMade = Instantiate(SingleRoomUIPrefab, _roomSelectionGrid.gameObject.transform);
                roomMade.GetComponent<SingleRoomUI>().SingleRoomInitializer(roomName);
                RoomsUI.Add(roomName, roomMade);
            }

            if (changedRoomList.Count == 0)
            {
                string roomName = "Private Room";
                GameObject roomMade = Instantiate(SingleRoomUIPrefab, _roomSelectionGrid.gameObject.transform);
                roomMade.GetComponent<SingleRoomUI>().SingleRoomInitializer(roomName, 1, "Private");
                RoomsUI.Add(roomName, roomMade);
            }
        }

        public void InstantiateRoomWindow()
        {
            Debug.Log("Room Window Opened");
            Instantiate(SingleRoomWindowPrefab);
        }

        public void InstantiateRoomSettingWindow()
        {
            Debug.Log("Room Setting Window Opened");
            Instantiate(SingleRoomSettingWindowPrefab);
        }

        public void AddButtonToButtonList(GameObject toAddButton)
        {
            PopUpButtons.Add(toAddButton);
        }

        public void RemoveButtonToButtonList(GameObject toRemoveButton)
        {
            PopUpButtons.Remove(toRemoveButton);
        }
    }
}