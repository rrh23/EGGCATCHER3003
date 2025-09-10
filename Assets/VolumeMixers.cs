using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class VolumeMixers : MonoBehaviour
{
    [Header("------AudioSouce------")]
    public AudioSource BGMSource;
    public AudioSource SFXSource;

    [Header("------AudioClip------")]
    public AudioClip click;
    public AudioClip collect, mine, heal, voiid;
    public AudioClip bgm;
    public AudioClip WIN;

    public AudioMixer AudioMixer;
    public Slider BGMSlider;
    public Slider SFXSlider;

    public MainManager MainManager;

    public float BGMvolume = 1f;
    public float SFXvolume = 1f;

    private float stopDuration = 1.0f; // Time it takes to stop the audio

    private bool isStopping = false;
    private bool isActive = true;
    private float originalPitch;

    private void Start()
    {
        if (BGMSource != null)
            originalPitch = BGMSource.pitch;

        //plays bgm when game starts
        BGMSource.clip = bgm;
        BGMSource.Play();
        BGMSource.loop = true;

        Debug.Log("bgm volume:" + BGMvolume);
        Debug.Log("sfx volume:" + SFXvolume);

        //SetMusicVolume();
    }
    public void SetBGMVolume()
    {
        float volume = BGMSlider.value;
        AudioMixer.SetFloat("BGM",Mathf.Log10(volume)*20);
    }

    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        AudioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void playSound(string soundName)
    {
        if (soundName == "collect") PlaySFX(collect); 
        if (soundName == "mine") PlaySFX(mine); 
        if (soundName == "heal") PlaySFX(heal); 
        if (soundName == "void") PlaySFX(voiid); 
    }

    public void PlayWIN()
    {
        BGMSource.clip = WIN;
        BGMSource.Play();
        BGMSource.loop = false;
    }

    public void PlayEND()
    {
        TapeStop();

    }

    public void TapeStop()
    {
        if (!isStopping)
            StartCoroutine(TapeStopCoroutine());
    }

    private IEnumerator TapeStopCoroutine()
    {
        isStopping = true;

        float time = 0f;
        float startPitch = BGMSource.pitch;

        while (time < stopDuration)
        {
            time += Time.deltaTime;
            float t = time / stopDuration;

            // Gradually decrease pitch over time
            BGMSource.pitch = Mathf.Lerp(startPitch, 0.0f, t);

            // Optionally decrease volume (for realism)
            BGMSource.volume = Mathf.Lerp(1.0f, 0.0f, t);

            yield return null;
        }

        BGMSource.pitch = 0.0f;
        BGMSource.Stop();
        isStopping = false;
    }

    public void SetActiveFunc(bool boolean)
    {
        isActive = boolean;
        gameObject.SetActive(boolean);
    }
    
    public void savePreferences()
    {
        MainManager.Instance.BGMvolume = BGMSlider.value;
        MainManager.Instance.SFXvolume = SFXSlider.value;
    }
    public void loadPreferences()
    {
        BGMSlider.value = MainManager.Instance.BGMvolume;
        SFXSlider.value = MainManager.Instance.SFXvolume;
    }
}
