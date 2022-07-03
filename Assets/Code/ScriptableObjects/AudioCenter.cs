using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
