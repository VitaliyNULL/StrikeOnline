using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace StrikeOnline.Managers.MenuManager
{
    public class PlayerListItem : MonoBehaviourPunCallbacks
    {
        #region Private Fields

        [SerializeField] private TMP_Text text;
        private Player _player;

        #endregion

        #region Public Fields

        public void Setup(Player player)
        {
            _player = player;
            text.text = player.NickName;
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (_player.Equals(otherPlayer))
            {
                Destroy(gameObject);
            }
        }

        public override void OnLeftRoom()
        {
            Destroy(gameObject);
        }

        #endregion
    }
}