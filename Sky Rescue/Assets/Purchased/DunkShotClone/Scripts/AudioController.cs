using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

    public static AudioController Instance
    {
        get;
        private set;
    }

    public AudioSource audioSource;

    public AudioClip bounseSound;
    public AudioClip rimSound;
    public AudioClip netSound;
    public AudioClip shotSound;
    public AudioClip coinSound;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayAudio(AudioClip currentClip)
    {
        audioSource.PlayOneShot(currentClip);
    }
}
