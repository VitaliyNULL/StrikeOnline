using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace StrikeOnline.UI
{
    public class SensitivitySlider: MonoBehaviour
    {
        #region Private Fields
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_Text text;
        private const string MouseSensitivityKey = "mouseSenvitivity";

        #endregion

        #region MonoBehaviour CallBacks

        private void Start()
        {
            slider = GetComponent<Slider>();
            slider.value = PlayerPrefs.HasKey(MouseSensitivityKey) ? PlayerPrefs.GetFloat(MouseSensitivityKey) : 5;
        }

        private void OnEnable()
        {
            slider.value = PlayerPrefs.HasKey(MouseSensitivityKey) ? PlayerPrefs.GetFloat(MouseSensitivityKey) : 5;
        }

        #endregion

        #region Public Methods

        public void UpdateSensitivity()
        {
            PlayerPrefs.SetFloat(MouseSensitivityKey, slider.value);
            text.text = slider.value.ToString();
        }

        #endregion
    }
}