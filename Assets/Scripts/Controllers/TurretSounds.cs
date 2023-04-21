using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSounds : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] fire;
    [SerializeField]
    private AudioClip idle;
    [SerializeField]
    private AudioClip damage;
    [SerializeField]
    private AudioClip dead;

    [SerializeField]
    private AudioSource gunAudioSource;
    [SerializeField]
    private AudioSource machineAudioSource;

    public void Fire()
    {
        int randSound = Random.Range(0, fire.Length);
        float randPitch = Random.Range(0.5f, 1.2f);
        gunAudioSource.clip = fire[randSound];
        gunAudioSource.pitch = randPitch;
        gunAudioSource.Play();
    }

    public void Idle()
    {
        machineAudioSource.clip = idle;
        machineAudioSource.Play();
    }

    public void Damage()
    {
        machineAudioSource.clip = damage;
        machineAudioSource.Play();
    }

    public void Dead()
    {
        machineAudioSource.clip = dead;
        machineAudioSource.Play();
    }
}
