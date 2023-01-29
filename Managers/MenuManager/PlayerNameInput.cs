using Photon.Pun;
using TMPro;
using UnityEngine;

namespace StrikeOnline.Managers.MenuManager
{
    public class PlayerNameInput : MonoBehaviourPunCallbacks
    {
        #region Private Fields

        private const string PlayerNickNameKey = "PlayerNickName";
        private TMP_InputField _inputField;

        #endregion

        #region MonoBehaviour CallBacks

        private void Start()
        {
            _inputField = GetComponent<TMP_InputField>();
            if (PlayerPrefs.HasKey(PlayerNickNameKey))
            {
                _inputField.text = PlayerPrefs.GetString(PlayerNickNameKey);
                PhotonNetwork.NickName = PlayerPrefs.GetString(PlayerNickNameKey);
            }
            else
            {
                _inputField.text = "Player" + Random.Range(0, 10000).ToString("0000");
                ChangeName();
            }
        }

        #endregion

        #region Public Methods

        public void ChangeName()
        {
            PlayerPrefs.SetString(PlayerNickNameKey, _inputField.text);
            PhotonNetwork.NickName = PlayerPrefs.GetString(PlayerNickNameKey);
        }

        #endregion
    }
}