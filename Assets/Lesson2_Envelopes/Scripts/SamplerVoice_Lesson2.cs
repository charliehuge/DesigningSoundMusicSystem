using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SamplerVoice_Lesson2 : MonoBehaviour
{
    private readonly ASREnvelope_Lesson2 _envelope = new ASREnvelope_Lesson2();

    private AudioSource _audioSource;

    public void Play(AudioClip audioClip, double attackTime, double sustainTime, double releaseTime)
    {
        sustainTime = (sustainTime > attackTime) ? (sustainTime - attackTime) : 0.0;
        _envelope.Reset(attackTime, sustainTime, releaseTime, AudioSettings.outputSampleRate);

        _audioSource.clip = audioClip;
        _audioSource.Play();
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnAudioFilterRead(float[] buffer, int numChannels)
    {
        for (int sIdx = 0; sIdx < buffer.Length; sIdx += numChannels)
        {
            double volume = _envelope.GetLevel();

            for (int cIdx = 0; cIdx < numChannels; ++cIdx)
            {
                buffer[sIdx + cIdx] *= (float)volume;
            }
        }
    }
}
