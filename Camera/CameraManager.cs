using System;
using UnityEngine;

namespace StrikeOnline.Camera
{
    public class CameraManager : MonoBehaviour
    {
        #region Private Fields

        [Header("Value")] [SerializeField] private float sens;
        private Transform _orientation;
        private Transform _cameraPosition;
        private Transform _mainCamera;

        private float _xRotation;
        private float _yRotation;
        private bool _isCameraSet;

        #endregion

        #region Public Fields

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnEnable()
        {
            Debug.Log("CameraManager Enabled");
        }

        private void OnDisable()
        {
            Debug.Log("CameraManager Disabled");
        }

        private void Update()
        {
            if (_isCameraSet)
            {
                float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sens;
                float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sens;
                _yRotation += mouseX;
                _xRotation -= mouseY;
                _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
                transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0);
                _orientation.localRotation = Quaternion.Euler(0, _yRotation, 0);
            }
        }

        private void LateUpdate()
        {
            if (_isCameraSet)
            {
                transform.position = _cameraPosition.position;
            }
        }

        #endregion

        #region Public Methods

        public void SetCameraPosition(Transform cameraPos, Transform orientation)
        {
            _cameraPosition = cameraPos;
            _orientation = orientation;
            _isCameraSet = true;
        }
        public Transform GetCameraPosition()
        {
            return transform;
        }

        #endregion
    }
}