using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using StrikeOnline.Managers.MenuManager;
using UnityEngine;

namespace StrikeOnline.Managers
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Public Fields

        public static Launcher Instance;

        #endregion

        #region Monobehaviour Callbacks

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                Debug.LogWarning("More than one Launcher on scene. Object was destroyed");
            }

            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        #endregion

        #region MonobehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("In Master");
            PhotonNetwork.JoinLobby();
            MenuManager.MenuManager.Instance.OpenMenu(MenuEnum.LoadingMenu);
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("Joined Lobby");
            MenuManager.MenuManager.Instance.OpenMenu(MenuEnum.MainMenu);
        }


        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            MenuManager.MenuManager.Instance.ShowError("Room Creation failed: " + message);
            MenuManager.MenuManager.Instance.OpenMenu(MenuEnum.ErrorMenu);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined Room");
            MenuManager.MenuManager.Instance.OpenMenu(MenuEnum.RoomMenu);
            MenuManager.MenuManager.Instance.GetCurrentRoomName();
            Player[] players = PhotonNetwork.PlayerList;
            foreach (Transform child in MenuManager.MenuManager.Instance.GetPLayerListContent())
            {
                Destroy(child.gameObject);
            }

            foreach (var varPlayer in players)
            {
                // Instantiate(playerListItem,playerListContent).GetComponent<PlayerListItem>().Setup(players[i]);

                MenuManager.MenuManager.Instance.InstantiatePlayerListItem(varPlayer);
            }

            MenuManager.MenuManager.Instance.SetStartButtonActive(PhotonNetwork.IsMasterClient);
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            MenuManager.MenuManager.Instance.SetStartButtonActive(PhotonNetwork.IsMasterClient);
        }


        public override void OnLeftRoom()
        {
            MenuManager.MenuManager.Instance.OpenMenu(MenuEnum.MainMenu);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            foreach (Transform tr in MenuManager.MenuManager.Instance.GetRoomListContent())
            {
                Destroy(tr.gameObject);
            }

            foreach (var roomInfo in roomList)
            {
                if (roomInfo.RemovedFromList) continue;
                // Instantiate(roomListItem,roomListContent).GetComponent<RoomListItem>().Setup(roomList[i]);
                MenuManager.MenuManager.Instance.InstantiateRoomListItem(roomInfo);
            }
        }


        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            // Instantiate(playerListItem,playerListContent).GetComponent<PlayerListItem>().Setup(newPlayer);
            MenuManager.MenuManager.Instance.InstantiatePlayerListItem(newPlayer);
        }

        #endregion

        #region Public Methods

        public void Quit()
        {
            Application.Quit();
        }

        public void CreateRoom()
        {
            string roomName = MenuManager.MenuManager.Instance.GetCreationRoomName();
            if (string.IsNullOrEmpty(roomName)) return;
            PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 20 });
            MenuManager.MenuManager.Instance.OpenMenu(MenuEnum.LoadingMenu);
        }

        public void JoinRoom(RoomInfo info)
        {
            PhotonNetwork.JoinRoom(info.Name);
            MenuManager.MenuManager.Instance.OpenMenu(MenuEnum.LoadingMenu);
        }

        public void StartGame()
        {
            PhotonNetwork.LoadLevel(1);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
            MenuManager.MenuManager.Instance.OpenMenu(MenuEnum.LoadingMenu);
        }

        #endregion
    }
}