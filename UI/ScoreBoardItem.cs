using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace StrikeOnline.UI
{
    public class ScoreBoardItem : MonoBehaviourPunCallbacks
    {
        #region Private Fields

        [SerializeField] private TMP_Text username;
        [SerializeField] private TMP_Text killCount;
        [SerializeField] private TMP_Text deathCount;
        private const string KillCountKey = "killCount";
        private const string DeathCountKey = "deathCount";
        private Player _player;

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (targetPlayer.Equals(_player))
            {
                if (changedProps.ContainsKey(KillCountKey) || changedProps.ContainsKey(DeathCountKey))
                {
                    UpdateStats();
                }
            }
        }

        #endregion

        #region Private Methods

        private void UpdateStats()
        {
            if (_player.CustomProperties.TryGetValue(KillCountKey, out object kills))
            {
                killCount.text = kills.ToString();
            }

            if (_player.CustomProperties.TryGetValue(DeathCountKey, out object deaths))
            {
                deathCount.text = deaths.ToString();
            }
        }

        #endregion

        #region Public Methods

        public void Initialize(Player player)
        {
            username.text = player.NickName;
            _player = player;
            UpdateStats();
        }

        #endregion
    }
}