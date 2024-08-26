using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BHS.AcidRain.UI
{
    public class SingleRoomUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TextMeshProUGUI _roomName;
        [SerializeField] private TextMeshProUGUI _roomMembers;
        [SerializeField] private TextMeshProUGUI _roomState;

        private Transform _parentCanvasTransform;
        private bool isDragging = false;
        private bool isPointerOn = false;
        private Vector2 pressedPosition;
        private Vector2 dragStartPosition;

        private void Awake()
        {
            if (_roomName == null)
                _roomName = transform.Find("RoomName").GetComponent<TextMeshProUGUI>();
            if (_roomMembers == null)
                _roomMembers = transform.Find("RoomMembers").GetComponent<TextMeshProUGUI>();
            if (_roomState == null)
                _roomState = transform.Find("RoomState").GetComponent<TextMeshProUGUI>();

            _parentCanvasTransform = transform.parent;
        }

        public void SingleRoomInitializer(string roomName)
        {
            Debug.Log($"{roomName} has made!");

            _roomName.text = roomName;
        }

        public void SingleRoomInitializer(string roomName, int roomMembers, string roomState = "")
        {
            Debug.Log($"{roomName} has made!");

            _roomName.text = roomName;
            _roomMembers.text = $"{roomMembers}/4";

            if (roomState == "") { _roomState.text = "Waiting"; }
            else { _roomState.text = roomState; }
        }

        private void OnDestroy()
        {
            Debug.Log("Room Has Destroyed!!!");
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isDragging = false;
            pressedPosition = eventData.position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (isPointerOn && !isDragging)
                TryJoinRoom();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isDragging)
            {
                if (Vector2.Distance(pressedPosition, eventData.position) > 10f)
                {
                    isDragging = true;
                    Debug.Log("Start Dragging");
                    dragStartPosition = eventData.position;
                }
            }
            else
            {
                Debug.Log("Now Dragging");
                float yMoved = dragStartPosition.y - eventData.position.y;
                _parentCanvasTransform.position += Vector3.down * yMoved;
                dragStartPosition = eventData.position;
            }

            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("End Drag");
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isPointerOn = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isPointerOn = false;
        }

        private void TryJoinRoom()
        {
            Debug.Log("Room Clicked");
            PhotonNetwork.JoinRoom("aa");
        }
    }
}