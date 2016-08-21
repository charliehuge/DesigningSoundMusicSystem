using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SamplerVoice_Lesson1 : MonoBehaviour
{
    private AudioSource _audioSource;

    public void Play(AudioClip audioClip)
    {
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
}
