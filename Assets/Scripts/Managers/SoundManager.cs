using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource musicAudioSource;

    [SerializeField]
    private AudioMixer mainMixer;

    private float overallVolume = 0f;

    private static SoundManager instance;
    public static SoundManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        instance = this;
    }


    public static void LevelLoadComplete()
    {
        if (LevelManager.Instance && LevelManager.Instance.LevelMusic != null)
        {
            
            AudioClip levelMusic = LevelManager.Instance.LevelMusic;

            if (levelMusic)
            {
                instance.musicAudioSource.clip = levelMusic;
                instance.musicAudioSource.Play();
            }

            instance.AudioFadeLevelStart();
        }

    }


    private void AudioFadeLevelStart()
    {
        instance.StartCoroutine(LerpVolume(-80, overallVolume, .5f));
    }

    public IEnumerator UnLoadLevel()
    {
        yield return LerpVolume(overallVolume, -80, .5f);
    }

    private IEnumerator LerpVolume(float startVol, float endVol, float time)
    {
        float currVol = startVol;
        float currentTime = 0;
        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            currentTime = Mathf.Clamp(currentTime, 0, time);

            currVol = Mathf.Lerp(startVol, endVol, currentTime / time);
            instance.mainMixer.SetFloat("masterVolume", currVol);
            yield return null;
        }
    }


}

