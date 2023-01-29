using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace StrikeOnline.UI
{
    public class ScoreBoard : MonoBehaviourPunCallbacks
    {
        #region Private Fields

        [SerializeField] private Transform container;
        [SerializeField] private GameObject scoreboardItem;
        private readonly Dictionary<Player, ScoreBoardItem> _scoreBoardItems = new Dictionary<Player, ScoreBoardItem>();
        [SerializeField] private CanvasGroup canvasGroup;

        #endregion

        #region MonoBehaviour Callbacks
        private void Start()
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                AddScoreboardItem(player);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                canvasGroup.alpha = 1;
            }
            else if (Input.GetKeyUp(KeyCode.Tab))
            {
                canvasGroup.alpha = 0;
            }
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            AddScoreboardItem(newPlayer);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            RemoveScoreboardItem(otherPlayer);
        }

        #endregion

        #region Private Methods

        private void AddScoreboardItem(Player player)
        {
            ScoreBoardItem item = Instantiate(scoreboardItem, container).GetComponent<ScoreBoardItem>();
            item.Initialize(player);
            _scoreBoardItems[player] = item;
        }

        private void RemoveScoreboardItem(Player player)
        {
            Destroy(_scoreBoardItems[player].gameObject);
            _scoreBoardItems.Remove(player);
        }

        #endregion
    }
}