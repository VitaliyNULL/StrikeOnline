using UnityEngine;

namespace StrikeOnline.UI
{
    public class Billboard : MonoBehaviour
    {
        #region Private Fields

        private Camera _camera;

        #endregion

        #region MonoBehaviour Callbacks

        private void Update()
        {
            if (_camera == null)
            {
                _camera = FindObjectOfType<Camera>();
            }

            if (_camera == null) return;
            transform.LookAt(_camera.transform);
            transform.Rotate(Vector3.up * 180);
        }

        #endregion
    }
}