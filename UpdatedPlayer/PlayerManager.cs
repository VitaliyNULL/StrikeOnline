using Photon.Pun;
using Photon.Realtime;
using StrikeOnline.Spawn;
using UnityEngine;
using System.Linq;
using StrikeOnline.Managers;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace StrikeOnline.UpdatedPlayer
{
    public class PlayerManager : MonoBehaviourPunCallbacks
    {
        #region Private Fields

        [SerializeField] private Canvas canvas;
        private GameObject _controller;
        private int _killCount;
        private int _deathCount;
        private bool _exitMenuOpened;


        #endregion
        
        #region MonoBehaviour Callbacks

        private void Awake()
        {
            if (!photonView.IsMine)
            {
                Destroy(canvas.gameObject);
            }
            
        }

        private void Start()
        {
            if (photonView.IsMine)
            {
                CreateController();
            }
        }

        private void Update()
        {
            if(!photonView.IsMine) return;
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                CallExitMenu(true);
            }
        }

        #endregion

        #region Private Methods

        private void CreateController()
        {
            Transform spawnPoint = SpawnManager.Instance.GetSpawnPoint();
            _controller = PhotonNetwork.Instantiate("Player Controller", spawnPoint.position, spawnPoint.rotation, 0,
                new object[] { photonView.ViewID });
        }

        #endregion

        #region Public Methods

        public void Die()
        {
            PhotonNetwork.Destroy(_controller);
            CreateController();
            _deathCount++;

            Hashtable hash = new Hashtable();
            hash.Add("deathCount", _deathCount);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

        public void GetKill()
        {
            photonView.RPC(nameof(RpcGetKill), photonView.Owner);
        }

        public static PlayerManager Find(Player player)
        {
            return FindObjectsOfType<PlayerManager>().SingleOrDefault(x => x.photonView.Owner.Equals(player));
        }

        private void CallExitMenu(bool call)
        {
            if (call)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                canvas.gameObject.SetActive(true);
                if (photonView.IsMine)
                {
                    _exitMenuOpened = true;
                }
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                canvas.gameObject.SetActive(false);
                if (photonView.IsMine)
                {
                    _exitMenuOpened = false;
                }
            }
        }

        public void OnClickExitRoom()
        {
            GameManager.Instance.LeaveRoom();
            if (photonView.IsMine)
            {
                _exitMenuOpened = false;
            }
        }

        public bool GetExitMenuBool() => _exitMenuOpened;
        public void OnClickBackButton()
        {
            CallExitMenu(false);
        }
        #endregion

        #region RPC

        [PunRPC]
        public void RpcGetKill()
        {
            _killCount++;
            Hashtable hash = new Hashtable();
            hash.Add("killCount", _killCount);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

        #endregion
    }
}