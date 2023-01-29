using UnityEngine;

namespace StrikeOnline.Spawn
{
    public class SpawnManager : MonoBehaviour
    {
        #region Public Fields

        public static SpawnManager Instance;

        #endregion

        #region Private Fields

        private SpawnPoint[] _spawnPoints;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                Debug.LogWarning("More than one SpawnManager on scene. Object was destroyed");
            }

            _spawnPoints = GetComponentsInChildren<SpawnPoint>();
        }

        #endregion

        #region Public Methods

        public Transform GetSpawnPoint() => _spawnPoints[Random.Range(0, _spawnPoints.Length)].transform;

        #endregion
    }
}