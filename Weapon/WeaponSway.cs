using System;
using Photon.Pun;
using StrikeOnline.UpdatedPlayer;
using UnityEngine;

namespace StrikeOnline.Weapon
{
    public class WeaponSway : MonoBehaviour
    {
        #region Private Fields

        [Header("Sway Settings")] [SerializeField]
        private float smooth;

        [SerializeField] private PhotonView pV;
        private PlayerManager _playerManager;

        [SerializeField] private float multiplier;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            _playerManager = PhotonView.Find((int)pV.InstantiationData[0]).GetComponent<PlayerManager>();

        }

        private void Update()
        {
            if (_playerManager.GetExitMenuBool()) return;
            // get mouse input
            float mouseX = Input.GetAxisRaw("Mouse X") * multiplier;
            float mouseY = Input.GetAxisRaw("Mouse Y") * multiplier;

            // calculate target rotation
            Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
            Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

            Quaternion targetRotation = rotationX * rotationY;

            // rotate 
            transform.localRotation =
                Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
        }

        #endregion
    }
}