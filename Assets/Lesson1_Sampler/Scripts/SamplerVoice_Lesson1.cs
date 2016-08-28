using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SamplerVoice_Lesson1 : MonoBehaviour
{
    // The attached AudioSource component
    // Note: the RequireComponent attribute above will automatically attach
    // an AudioSource component to any GameObject that we attach this script to
    private AudioSource _audioSource;

    /// <summary>
    /// Plays the provided AudioClip on this voice's AudioSource
    /// </summary>
    /// <param name="audioClip">The AudioClip we want to play</param>
    public void Play(AudioClip audioClip)
    {
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }

    /// <summary>
    /// This gets called when the GameObject is first created.
    /// Do our initialization here, which is just grabbing the AudioSource component.
    /// </summary>
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
}
