using Photon.Pun;
using UnityEngine;

namespace StrikeOnline.UpdatedPlayer
{
    public class PlayerCamera : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private PhotonView pV;
        [SerializeField] private GameObject cameraHolder;
        [SerializeField] private float mouseSensitivity;
        private float _verticalLookRotation;
        private Camera _camera;
        private const string MouseSensitivityKey = "mouseSenvitivity";
        private PlayerManager _playerManager;
        #endregion

        #region MonoBehaviour CallBacks

        private void Awake()
        {
            _camera = GetComponentInChildren<Camera>();
            _playerManager = PhotonView.Find((int)pV.InstantiationData[0]).GetComponent<PlayerManager>();
        }

        private void Start()
        {
            if (pV.IsMine)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                if (PlayerPrefs.HasKey(MouseSensitivityKey))
                {
                    mouseSensitivity = PlayerPrefs.GetFloat(MouseSensitivityKey);
                }
                else
                {
                    PlayerPrefs.SetFloat(MouseSensitivityKey, 5f);
                }
            }
        }

        private void Update()
        {
            if (!pV.IsMine) return;
            if (_playerManager.GetExitMenuBool()) return;
            mouseSensitivity = PlayerPrefs.GetFloat(MouseSensitivityKey);
            CameraLook();
        }

        #endregion

        #region Private Fields

        private void CameraLook()
        {
            transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);
            _verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
            _verticalLookRotation = Mathf.Clamp(_verticalLookRotation, -90f, 90f);
            cameraHolder.transform.localEulerAngles = Vector3.left * _verticalLookRotation;
        }

        #endregion

        #region Public Methods

        public Camera GetPlayerCamera() => _camera;

        #endregion
    }
}