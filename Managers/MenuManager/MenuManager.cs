using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace StrikeOnline.Managers.MenuManager
{
    public class MenuManager : MonoBehaviour
    {
        #region Public Fields

        public static MenuManager Instance;

        #endregion

        #region Private Fields

        [SerializeField] private Menu[] menus;
        [SerializeField] private TMP_InputField roomInputField;
        [SerializeField] private TMP_Text roomName;
        [SerializeField] private TMP_Text errorText;
        [SerializeField] private Transform roomListContent;
        [SerializeField] private Transform playerListContent;
        [SerializeField] private GameObject roomListItem;
        [SerializeField] private GameObject playerListItem;
        [SerializeField] private GameObject startButton;

        #endregion

        #region Public Methods

        public void OpenMenu(MenuEnum menuName)
        {
            foreach (var varMenu in menus)
            {
                if (varMenu.menuEnum == menuName)
                {
                    OpenMenu(varMenu);
                    return;
                }
            }

            throw new Exception($"No Menu was opened.\n Initialize {menuName} on Menu object");
        }

        public void OpenMenu(Menu menu)
        {
            foreach (var varMenu in menus)
            {
                CloseMenu(varMenu);
            }

            menu.Open();
        }

        public void ShowError(string text)
        {
            errorText.text = text;
        }

        public void GetCurrentRoomName()
        {
            roomName.text = PhotonNetwork.CurrentRoom.Name;
        }

        public void SetStartButtonActive(bool isActive)
        {
            startButton.SetActive(isActive);
        }

        public void InstantiateRoomListItem(RoomInfo info)
        {
            Instantiate(roomListItem, roomListContent).GetComponent<RoomListItem>().Setup(info);
        }

        public Transform GetPLayerListContent() => playerListContent;
        public Transform GetRoomListContent() => roomListContent;
        public string GetCreationRoomName() => roomInputField.text;

        public void InstantiatePlayerListItem(Player player)
        {
            Instantiate(playerListItem, playerListContent).GetComponent<PlayerListItem>().Setup(player);
        }

        #endregion

        #region Private Methods

        private void CloseMenu(Menu menu)
        {
            menu.Close();
        }

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                Debug.LogWarning("More than one Menu Manager on scene. Object was destroyed");
            }
        }

        #endregion
    }
}