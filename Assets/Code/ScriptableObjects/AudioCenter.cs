using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioCenter : MonoBehaviour
{
    [SerializeField] AudioSource backGround;
    [SerializeField] AudioSource soundFx;

    public void ChangeBackgroundMusic(AudioClip audioClip)
    {
        backGround.Stop();
        backGround.clip = audioClip;
        backGround.Play();
        backGround.loop = true;
    }

    public void PlaySoundFx(AudioClip audioClip)
    {
        soundFx.PlayOneShot(audioClip);
    }

    public void SwithSoundOnOff()
    {
        backGround.mute = !backGround.mute;
        soundFx.mute = !soundFx.mute;
    }

    public bool ReturnSoundFXState() =>
        soundFx.mute;

    public bool IsBackgroundMuted() =>
        backGround.mute;

}
