using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour {

    public static PlayerSounds instance;

    public AudioClip[] SoundEffects;

    private AudioSource myAudio;

    private float baseVolume;
    private float basePitch;


    public enum E_SoundEffects
    {
        death,
        brake,
        jump,
        step
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        myAudio = GetComponent<AudioSource>();
        baseVolume = myAudio.volume;
        basePitch = myAudio.pitch;

    }

    public void PlaySoundEffect(E_SoundEffects soundEffect)
    {
        myAudio.volume = baseVolume * Random.Range(0.9f, 1.1f);
        myAudio.pitch = basePitch * Random.Range(0.9f, 1.1f);
        myAudio.PlayOneShot(SoundEffects[(int)(soundEffect)]);
    }


    public void PlaySoundEffect(E_SoundEffects soundEffect, float volume, float pitch)
    {
        //myAudio.volume = volume;
        //myAudio.pitch = pitch;
        myAudio.PlayOneShot(SoundEffects[(int)(soundEffect)]);
    }
}
