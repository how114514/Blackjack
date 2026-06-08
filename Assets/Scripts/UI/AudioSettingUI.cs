using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        LoadVolumes();
    }

    private void LoadVolumes()
    {
        float bgmDB;
        if (mixer.GetFloat("BGMVolume", out bgmDB))
        {
            bgmSlider.value = Mathf.Pow(10f, bgmDB / 20f);
        }

        float sfxDB;
        if (mixer.GetFloat("SFXVolume", out sfxDB))
        {
            sfxSlider.value = Mathf.Pow(10f, sfxDB / 20f);
        }
    }
}