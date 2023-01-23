using UnityEngine;

namespace StrikeOnline.Camera
{
    public class CameraFollow: MonoBehaviour
    {
        [SerializeField] private Transform cameraPosition;

        private void LateUpdate()
        {
            transform.position = cameraPosition.position;
        }
    }
}