
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Fullscreen")]
    [SerializeField] private Toggle fullscreenToggle;

    private void Start()
    {
        // Recupera os valores salvos.
        float master = PlayerPrefs.GetFloat("Master", 1f);
        float music = PlayerPrefs.GetFloat("MusicVolume", 0.25f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        bool fullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

        // Atualiza os controles.
        masterSlider.value = master;
        musicSlider.value = music;
        sfxSlider.value = sfx;
        fullscreenToggle.isOn = fullscreen;

        // Aplica as configurações.
        SetMasterVolume(master);
        SetMusicVolume(music);
        SetSFXVolume(sfx);
        SetFullscreen(fullscreen);
    }

    public void SetMasterVolume(float value)
    {
        SetMixerVolume("Master", value);
        PlayerPrefs.SetFloat("Master", value);
    }

    public void SetMusicVolume(float value)
    {
        SetMixerVolume("MusicVolume", value);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        SetMixerVolume("SFXVolume", value);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    private void SetMixerVolume(string parameter, float value)
    {
        float decibels = value <= 0.0001f
            ? -80f
            : Mathf.Log10(value) * 20f;

        audioMixer.SetFloat(parameter, decibels);
    }

    public void SetFullscreen(bool value)
    {
        Screen.fullScreen = value;

        PlayerPrefs.SetInt("Fullscreen", value ? 1 : 0);
    }

    public void SaveSettings()
    {
        PlayerPrefs.Save();
    }
}
