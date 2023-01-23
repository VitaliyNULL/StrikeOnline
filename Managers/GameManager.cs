using Photon.Pun;
using StrikeOnline.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StrikeOnline.Managers
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Public Fields

        public static GameManager Instance;

        #endregion

        #region Private Fields

        private GameObject _instance;

        [Tooltip("The prefab to use for representing the player")]
        [SerializeField] private GameObject playerPrefab;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            Instance = this;

            if (!PhotonNetwork.IsConnected)
            {
                SceneManager.LoadScene(0);
                return;
            }

            if (playerPrefab == null)
            {
                Debug.LogError("Set prefab to GameManager");
            }
            else
            {
                if (PlayerManager.LocalPlayerInstance == null)
                {
                    PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 5, 0), Quaternion.identity);
                }
            }
        }

        #endregion

        #region MonobehaviourPunCallbacks Callbacks

        public override void OnPlayerEnteredRoom(global::Photon.Realtime.Player newPlayer)
        {
            Debug.Log(PhotonNetwork.LocalPlayer.NickName + " entered room");
        }

        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        #endregion
    }
}