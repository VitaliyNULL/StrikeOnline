using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StrikeOnline.UI
{
    public class SoundVolumeSlider : MonoBehaviour
    {
        #region Private Fields

        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text text;
        private const string SoundValueKey = "soundValue";

        #endregion

        #region MonoBehaviour CallBacks

        private void Start()
        {
            slider = GetComponent<Slider>();
            slider.value = PlayerPrefs.HasKey(SoundValueKey) ? PlayerPrefs.GetFloat(SoundValueKey) : 5;
        }

        private void OnEnable()
        {
            slider.value = PlayerPrefs.HasKey(SoundValueKey) ? PlayerPrefs.GetFloat(SoundValueKey) : 5;
        }

        #endregion
        #region Public Methods

        public void UpdateVolumeValue()
        {
            PlayerPrefs.SetFloat(SoundValueKey, slider.value);
            text.text = slider.value.ToString("0.0");
        }

        #endregion
    }
}