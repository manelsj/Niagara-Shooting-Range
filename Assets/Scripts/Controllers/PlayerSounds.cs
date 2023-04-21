using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] fire1;
    [SerializeField]
    private AudioClip[] fire2;

    [SerializeField]
    private AudioSource gunAudioSource;

    public void Fire1()
    {
        int randSound = Random.Range(0, fire1.Length);
        float randPitch = Random.Range(0.5f, 1.2f);
        gunAudioSource.clip = fire1[randSound];
        gunAudioSource.pitch = randPitch;
        gunAudioSource.Play();
    }

    public void Fire2()
    {
        int randSound = Random.Range(0, fire2.Length);
        float randPitch = Random.Range(0.5f, 1.2f);
        gunAudioSource.clip = fire2[randSound];
        gunAudioSource.pitch = randPitch;
        gunAudioSource.Play();
    }
}
