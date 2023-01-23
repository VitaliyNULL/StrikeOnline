using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

namespace StrikeOnline.Weapon
{
    public class PickUpGunController: MonoBehaviourPunCallbacks
    {
        #region Private Fields

        private StandartGun _standartGun;
        private Rigidbody _gunRigidbody;
        private Collider _gunCollider;
        private bool _isGunEquip =false;
        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            _standartGun = GetComponent<StandartGun>();
            _gunCollider = GetComponentInChildren<Collider>();
            _gunRigidbody = GetComponent<Rigidbody>();
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnPlayerPropertiesUpdate(global::Photon.Realtime.Player targetPlayer, Hashtable changedProps)
        {
            if (!photonView.IsMine && targetPlayer == photonView.Owner)
            {
                PickUpGun((UnityEngine.Transform)changedProps["weaponController"]);
            }
        }

        #endregion
        #region Public Methods

        public void PickUpGun(Transform positionForWeapon)
        {
            if (!_isGunEquip)
            {
                Debug.Log("Gun is picked up ");
                _standartGun.enabled = true;
                _gunRigidbody.useGravity = false;
                _gunCollider.isTrigger = true;
                _isGunEquip = true;
                transform.SetParent(positionForWeapon);
                transform.localPosition = Vector3.zero;
                transform.localRotation =Quaternion.Euler(Vector3.zero);
                transform.localScale = Vector3.one;
                
            }
            
           
        }

        public void DropDownGun()
        {
            _standartGun.enabled = false;
            _gunRigidbody.useGravity = true;
            _gunCollider.isTrigger = false;
            _isGunEquip = false;
            transform.SetParent(null);
            _gunRigidbody.AddForce(_gunRigidbody.transform.forward * 6f + _gunRigidbody.transform.up * 3f, ForceMode.Impulse);
            _gunRigidbody.AddTorque(_gunRigidbody.transform.right + _gunRigidbody.transform.forward, ForceMode.Impulse);
        }
        

        #endregion
    }
}