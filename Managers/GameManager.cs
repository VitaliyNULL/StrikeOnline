using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StrikeOnline.Managers
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Public Fields

        public static GameManager Instance;

        #endregion

        #region Private Fields

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                Debug.LogWarning("More than one GameManager on scene. Object was destroyed");
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnLeftRoom()
        {
            Debug.Log("OnLeftRoom");
            StartCoroutine(WaitToLeave());
        }

        #endregion

        #region Private Methods

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.buildIndex == 1)
            {
                PhotonNetwork.Instantiate("Player Manager", new Vector3(0, 5f, 0), Quaternion.identity);
            }
        }

        private IEnumerator WaitToLeave()
        {
            while (PhotonNetwork.InRoom)
            {
                Debug.Log("Wait To Leave Cycle");
                yield return null;
            }

            SceneManager.LoadScene(0);
        }

        #endregion

        #region Public Methods

        public void LeaveRoom()
        {
            Debug.Log("Leave Room");
            PhotonNetwork.LeaveRoom();
        }

        #endregion
    }
}