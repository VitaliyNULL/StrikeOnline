using Photon.Pun;
using TMPro;
using UnityEngine;

namespace StrikeOnline.UI
{
    public class DisplayNickname : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private PhotonView playerPhotonView;
        [SerializeField] private TMP_Text nickname;

        #endregion
        
        #region MonoBehaviour Callbacks

        private void Start()
        {
            if (playerPhotonView.IsMine)
            {
                Destroy(gameObject);
            }

            nickname.text = playerPhotonView.Owner.NickName;
        }

        #endregion
    }
}