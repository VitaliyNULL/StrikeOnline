using UnityEngine;

namespace StrikeOnline.Spawn
{
    public class SpawnPoint : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private GameObject point;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            point.SetActive(false);
        }

        #endregion
    }
}