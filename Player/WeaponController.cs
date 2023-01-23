using ExitGames.Client.Photon;
using Photon.Pun;
using StrikeOnline.Core;
using StrikeOnline.Weapon;
using UnityEngine;

namespace StrikeOnline.Player
{
    public class WeaponController : MonoBehaviour
    {
        #region Private Fields

        private IReloadingWeapon _iReloadingWeapon;
        private Transform _cameraTransform;
        private PickUpGunController pickUpGunController;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            _cameraTransform = UnityEngine.Camera.main.transform;
            Hashtable hash = new Hashtable();
            hash.Add("weaponController", transform);
            hash.Add("reloadingWeapon", _iReloadingWeapon);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.E))
            {
                FindWeapon();
            }

            if (Input.GetMouseButton(0))
            {
                _iReloadingWeapon?.Damage();
            }
        }

        #endregion

        #region Private Methods

        private void FindWeapon()
        {
            RaycastHit hit;
            if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out hit, 5f))
            {
                Debug.Log("Ray is catch something");
                if (hit.transform.TryGetComponent(out pickUpGunController))
                {
                    pickUpGunController.PickUpGun(transform);
                    Debug.Log("Gun is taken");
                    _iReloadingWeapon = hit.transform.GetComponent<IReloadingWeapon>();
                    _iReloadingWeapon.TakeWeaponDirection(_cameraTransform);
                }
            }
        }

        #endregion
        
    }
}