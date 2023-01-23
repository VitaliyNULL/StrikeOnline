using UnityEngine;

namespace StrikeOnline.Camera
{
    public class CameraLook : MonoBehaviour
    {
        [Header("Value")] [SerializeField] private float sens;
        [Header("Object")] [SerializeField] private Transform orientation;

        private float _xRotation;
        private float _yRotation;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sens;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sens;
            

            _yRotation += mouseX;
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -60f, 60f);
            transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, _yRotation, 0);
        }
    }
}