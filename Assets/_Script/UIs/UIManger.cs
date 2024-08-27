using Photon.Pun;
using Photon.Realtime;
using System;
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

        #region RoomUIControl
        //Instantiate Room GameObject.
        public void AddRoomUI(RoomInfo roomInfo)
        {
            RoomInfo _tempRoomInfo = roomInfo;
            GameObject roomMade = 
                Instantiate(SingleRoomUIPrefab, _roomSelectionGrid.gameObject.transform);
            SingleRoomUI _tempSingleRoom = roomMade.GetComponent<SingleRoomUI>();

            _tempSingleRoom.SingleRoomComponentUpdate(_tempRoomInfo.Name, _tempRoomInfo.PlayerCount);
            RoomsUI.Add(_tempRoomInfo.Name, roomMade);
        }

        //Adjust Room GameObject.
        public void UpdateRoomUI(RoomInfo roomInfo)
        {
            RoomInfo _tempRoomInfo = roomInfo;
            SingleRoomUI _tempSingleRoom =
                RoomsUI[_tempRoomInfo.Name].GetComponent<SingleRoomUI>();

            _tempSingleRoom.SingleRoomComponentUpdate(_tempRoomInfo.Name, _tempRoomInfo.PlayerCount);
        }

        //Remove Room GameObject.
        public void RemoveRoomUI(RoomInfo roomInfo)
        {
            RoomInfo _tempRoomInfo = roomInfo;

            Destroy(RoomsUI[roomInfo.Name]);
            RoomsUI.Remove(roomInfo.Name);
        }
        #endregion

        #region RoomWindow
        public void InstantiateRoomWindow()
        {
            Debug.Log("Room Window Opened");
            PhotonNetwork.Instantiate("Prefabs/UIWindow/RoomWindow",Vector3.zero,Quaternion.identity);
            // Instantiate(SingleRoomWindowPrefab);
        }

        public void InstantiateRoomSettingWindow()
        {
            Debug.Log("Room Setting Window Opened");
            Instantiate(SingleRoomSettingWindowPrefab);
        }
        #endregion

        #region Button
        public void AddButtonToButtonList(GameObject toAddButton)
        {
            PopUpButtons.Add(toAddButton);
        }

        public void RemoveButtonToButtonList(GameObject toRemoveButton)
        {
            PopUpButtons.Remove(toRemoveButton);
        }
        #endregion
    }
}