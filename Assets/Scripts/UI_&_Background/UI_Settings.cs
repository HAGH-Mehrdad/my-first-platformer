using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class UI_Settings : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float audioMultiplier = 20;

    [Header("SFX settings")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI txtSfxSlider;
    [SerializeField] private string sfxParameter;

    [Header("BGM settings")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private TextMeshProUGUI txtBgmSlider;
    [SerializeField] private string bgmParameter;



    public void ApplySavedSettings()
    {
        sfxSlider.value = PlayerPrefs.GetFloat(sfxParameter, 0.7f);
        bgmSlider.value = PlayerPrefs.GetFloat(bgmParameter, 0.7f);
    }

    public void SetSfxVolume(float value)// SetSfx and SetBgm methods are assined to the sliders 
    {
        float newValue = Mathf.Log10(value) * audioMultiplier; // logarithmic scale for audio
        txtSfxSlider.text = Mathf.RoundToInt(value * 100) + "%"; //mulipy by 100 to get percentage
        audioMixer.SetFloat(sfxParameter , newValue);// the slider value is passed as a parameter to the audio mixer
    }

    public void SetBgmValue(float value) //logic here is like the SFX one
    {
        float newValue = Mathf.Log10(value) * audioMultiplier;
        txtBgmSlider.text = Mathf.RoundToInt(value * 100) + "%";
        audioMixer.SetFloat(bgmParameter, newValue);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(sfxParameter, sfxSlider.value); // save the slider value when the settings UI is closed
        PlayerPrefs.SetFloat(bgmParameter, bgmSlider.value); // save the slider value when the settings UI is closed
    }

    private void OnEnable()
    {
        sfxSlider.value = PlayerPrefs.GetFloat(sfxParameter, 0.7f);
        bgmSlider.value = PlayerPrefs.GetFloat(bgmParameter, 0.7f);
    }
}
