using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SamplerVoice_Lesson3 : MonoBehaviour
{
    private readonly ASREnvelope_Lesson3 _envelope = new ASREnvelope_Lesson3();

    private AudioSource _audioSource;
    private uint _samplesUntilEnvelopeTrigger;

    public void Play(AudioClip audioClip, double startTime, double attackTime, double sustainTime, double releaseTime)
    {
        sustainTime = (sustainTime > attackTime) ? (sustainTime - attackTime) : 0.0;
        _envelope.Reset(attackTime, sustainTime, releaseTime, AudioSettings.outputSampleRate);

        double timeUntilTrigger = (startTime > AudioSettings.dspTime) ? (startTime - AudioSettings.dspTime) : 0.0;
        _samplesUntilEnvelopeTrigger = (uint)(timeUntilTrigger * AudioSettings.outputSampleRate);

        _audioSource.clip = audioClip;
        _audioSource.PlayScheduled(startTime);
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnAudioFilterRead(float[] buffer, int numChannels)
    {
        for (int sIdx = 0; sIdx < buffer.Length; sIdx += numChannels)
        {
            double volume = 0;

            if (_samplesUntilEnvelopeTrigger == 0)
            {
                volume = _envelope.GetLevel();
            }
            else
            {
                --_samplesUntilEnvelopeTrigger;
            }

            for (int cIdx = 0; cIdx < numChannels; ++cIdx)
            {
                buffer[sIdx + cIdx] *= (float)volume;
            }
        }
    }
}
