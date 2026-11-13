using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsController : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private void Start()
    {
        masterSlider.onValueChanged.AddListener(ChangeMasterVolume);
        musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
        sfxSlider.onValueChanged.AddListener(ChangeSFXVolume);

        ChangeMasterVolume(masterSlider.value);
        ChangeMusicVolume(musicSlider.value);
        ChangeSFXVolume(sfxSlider.value);
    }

    private void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
    }

    private void ChangeMusicVolume(float value)
    {
        musicSource.volume = value;
    }

    private void ChangeSFXVolume(float value)
    {
        sfxSource.volume = value;
    }
}
