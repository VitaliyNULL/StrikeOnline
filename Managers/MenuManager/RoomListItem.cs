using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace StrikeOnline.Managers.MenuManager
{
    public class RoomListItem : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private TMP_Text text;
        private RoomInfo _info;

        #endregion

        #region Public Methods

        public void Setup(RoomInfo info)
        {
            _info = info;
            text.text = info.Name;
        }

        public void OnClick()
        {
            Launcher.Instance.JoinRoom(_info);
        }

        #endregion
    }
}