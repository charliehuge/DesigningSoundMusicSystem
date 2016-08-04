using System;
using UnityEngine;

/// <summary>
/// Tone Generator
/// 
/// Generates a sine wave when triggered
/// 
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class ToneGenerator : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool _debugTrigger = false;
    // The frequency of the tone in cycles per second (a.k.a. Hertz)
    [SerializeField, Range(100.0f, 1000.0f)] private double _frequency_Hz = 440.0;
    // The duration of the tone in seconds
    [SerializeField, Range(0.1f, 2.0f)] private double _duration_s = 1.0;
    // The volume of the tone (0.0 is silent, 1.0 is full volume)
    [SerializeField, Range(0f, 1f)] private float _volume = 0.5f;

    private int _sampleRate = 0;
    // The number of samples remaining in the current tone
    private uint _samplesRemaining = 0;
    // The current "angle" through the sine wave (a.k.a. phase)
    private double _angle = 0.0;
    // The amount the angle should move each sample, advancing through the waveform
    private double _angleDelta = 0.0;
    // Keep track of this, as it will be different from the debug volume
    private float _currentVolume = 0f;

    /// <summary>
    /// Call this to trigger a note.
    /// </summary>
    public void Play(double frequency_Hz, double duration_s, float volume)
    {
        // set the angle delta, which is how much we should advance
        // through the waveform each sample
        double cyclesPerSample = frequency_Hz / _sampleRate;
        _angleDelta = cyclesPerSample * Math.PI * 2.0; // 2*PI is one cycle
        // set the samples remaining
        _samplesRemaining = (uint)(duration_s * _sampleRate);
        // reset the angle
        _angle = 0.0;
        // set the volume
        _currentVolume = volume;
    }

    /// <summary>
    /// This gets called every time the object is enabled.
    /// </summary>
    private void OnEnable()
    {
        // Get the sample rate here, because we're not allowed to get it from the audio thread
        _sampleRate = AudioSettings.outputSampleRate;
    }

    /// <summary>
    /// This gets called once per frame during the main game update loop, on the main thread.
    /// </summary>
    private void Update()
    {
        // This is a handy way to trigger the note from the editor.
        // It checks to see if you've ticked the checkbox, and if so
        // it starts a new note right away.
        if (_debugTrigger)
        {
            // Trigger now
            Play(_frequency_Hz, _duration_s, _volume);
            // reset the trigger
            _debugTrigger = false;
        }
    }

    /// <summary>
    /// This gets called every time the AudioSource this ToneGenerator is attached to sends
    /// a new audio buffer toward the output. This gets called from the audio thread, so
    /// some Unity API functions aren't available, and anything in here should execute quickly.
    /// </summary>
    /// <param name="buffer">The array of samples being sent from the AudioSource</param>
    /// <param name="numChannels">The number of channels in the buffer (interleaved)</param>
    private void OnAudioFilterRead(float[] buffer, int numChannels)
    {
        // If there are no samples remaining, the tone has ended
        bool hasEnded = (_samplesRemaining == 0);

        // don't bother doing anything if we're out of range of the triggered tone
        if (hasEnded)
        {
            return;
        }

        float sample = 0f;
        // loop through the samples in the buffer and generate a sine
        for (int sIdx = 0; sIdx < buffer.Length; sIdx += numChannels)
        {
            // get the next sample in a sine and multiply it by the volume
            sample = (float)Math.Sin(_angle) * _currentVolume;           

            // assign the sample to each channel in the buffer
            for (int cIdx = 0; cIdx < numChannels; ++cIdx)
            {
                buffer[sIdx + cIdx] = sample;
            }

            // increment the current angle
            _angle += _angleDelta;
            
            // decrement the samples remaining
            --_samplesRemaining;

            // check and see if the tone should end
            if (_samplesRemaining == 0)
            {
                return;
            }
        }
    }
}
