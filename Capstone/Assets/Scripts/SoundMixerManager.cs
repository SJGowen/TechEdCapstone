using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider masterVolume;
    [SerializeField] Slider soundFXVolume;
    [SerializeField] Slider musicVolume;

    private void Awake()
    {
        masterVolume.value = PlayerPrefs.GetFloat("MasterVolume", 1f); ;
        audioMixer.SetFloat("masterVolume", Mathf.Log10(masterVolume.value) * 20f);
        soundFXVolume.value = PlayerPrefs.GetFloat("SoundFXVolume", 1f);
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(soundFXVolume.value) * 20f);
        musicVolume.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        audioMixer.SetFloat("musicVolume", Mathf.Log10(musicVolume.value) * 20f);
    }

    public void SetMasterVolume(float level)
    {
        PlayerPrefs.SetFloat("MasterVolume", level);
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20f);
    }

    public void SetSoundFXVolume(float level)
    {
        PlayerPrefs.SetFloat("SoundFXVolume", level);
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(level) * 20f);
    }

    public void SetMusicVolume(float level)
    {
        PlayerPrefs.SetFloat("MusicVolume", level);
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20f);
    }
}
