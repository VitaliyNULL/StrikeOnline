using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using StrikeOnline.Weapon;
using UnityEngine;

namespace StrikeOnline.UpdatedPlayer
{
    public class WeaponController : MonoBehaviourPunCallbacks
    {
        #region Private Fields

        [SerializeField] private GunItem[] guns;
        private PlayerManager _playerManager;
        private int _gunIndex;
        private int _prevGunIndex = -1;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _playerManager = PhotonView.Find((int)photonView.InstantiationData[0]).GetComponent<PlayerManager>();
        }

        #endregion
        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.ContainsKey("gunIndex") && !photonView.IsMine && targetPlayer.Equals(photonView.Owner))
            {
                EquipItem((int)changedProps["gunIndex"]);
            }
        }

        #endregion

        #region Private Methods

        private void EquipItem(int index)
        {
            if (index == _prevGunIndex) return;
            _gunIndex = index;
            guns[_gunIndex].gunGameObject.SetActive(true);
            if (_prevGunIndex != -1)
            {
                guns[_prevGunIndex].gunGameObject.SetActive(false);
            }

            _prevGunIndex = _gunIndex;
            if (photonView.IsMine)
            {
                Hashtable hash = new Hashtable();
                hash.Add("gunIndex", _gunIndex);
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            }
        }

        private void SwitchGun()
        {
            for (int i = 0; i < guns.Length; i++)
            {
                if (Input.GetKey((i + 1).ToString()))
                {
                    EquipItem(i);
                    break;
                }
            }

            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
            {
                if (_gunIndex >= guns.Length - 1)
                {
                    EquipItem(0);
                }
                else
                {
                    EquipItem(_gunIndex + 1);
                }
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
            {
                if (_gunIndex <= 0)
                {
                    EquipItem(guns.Length - 1);
                }
                else
                {
                    EquipItem(_gunIndex - 1);
                }
            }
        }

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            EquipItem(photonView.IsMine ? 0 : _gunIndex);
        }

        private void Update()
        {
            if (!photonView.IsMine)
                return;
            if (_playerManager.GetExitMenuBool()) return;
            SwitchGun();
            if (Input.GetMouseButton(0))
            {
                guns[_gunIndex].GunReloadingWeapon?.Damage();
            }

            if (Input.GetKey(KeyCode.R))
            {
                guns[_gunIndex].GunReloadingWeapon?.Reload();
            }
        }

        #endregion
    }
}