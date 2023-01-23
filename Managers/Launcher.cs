using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace StrikeOnline.Managers
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Fields

        [SerializeField] private GameObject controlPanel;
        [SerializeField] private TMP_Text connecting;
        private bool isConnecting;

        #endregion

        #region Public Fields

        #endregion

        #region Monobehaviour Callbacks

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            connecting.enabled = false;
            controlPanel.SetActive(true);
        }

        #endregion

        #region MonobehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
                Debug.Log("Connected to master");
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 20 });
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel(1);
        }


        public override void OnDisconnected(DisconnectCause cause)
        {
            isConnecting = false;
            connecting.enabled = false;
            controlPanel.SetActive(true);
        }

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        public void Connect()
        {
            isConnecting = true;
            connecting.enabled = true;
            controlPanel.SetActive(false);

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        #endregion
    }
}