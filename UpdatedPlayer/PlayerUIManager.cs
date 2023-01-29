using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StrikeOnline.UpdatedPlayer
{
    public class PlayerUIManager : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private PhotonView pV;
        [SerializeField] private Image healthBar;
        [SerializeField] private TMP_Text ammoBar;
        [SerializeField] private TMP_Text gunName;
        [SerializeField] private Canvas canvas;
        

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            if (!pV.IsMine)
            {
                Destroy(canvas.gameObject);
            }
        }

        #endregion

        #region Public Methods

        public void UpdateHealthBar(float value)
        {
            healthBar.fillAmount = value;
        }

        public void UpdateAmmoBar(float currentAmmo, float allAmmo)
        {
            ammoBar.text = string.Format($"{currentAmmo}/{allAmmo}");
        }

        public void UpdateGunName(string gName)
        {
            gunName.text = gName;
        }

        #endregion
    }
}