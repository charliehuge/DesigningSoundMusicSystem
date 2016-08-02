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

    [Header("Tone Config")]
    [SerializeField, Range(100.0f, 1000.0f)] private double _frequency_Hz = 440.0;
    [SerializeField, Range(0.1f, 2.0f)] private double _duration_s = 1.0;
    [SerializeField, Range(0f, 1f)] private float _volume = 0.5f;

    private double _startTime_s = 0.0;
    private uint _samplesRemaining = 0;
    private double _angle = 0.0;
    private double _angleDelta = 0.0;

    /// <summary>
    /// Call this to trigger a note at some point (preferably) in the future
    /// </summary>
    /// <param name="startTime">The start time of the note, relative to AudioSettings.dspTime</param>
    public void Trigger(double startTime)
    {
        // set the start time
        _startTime_s = AudioSettings.dspTime;
        // set the angle delta, which is how much we should advance
        // through the waveform each sample
        double cyclesPerSample = _frequency_Hz / AudioSettings.outputSampleRate;
        _angleDelta = cyclesPerSample * Math.PI * 2.0;
        // set the samples remaining
        _samplesRemaining = (uint)(_duration_s * AudioSettings.outputSampleRate);
        // reset the angle
        _angle = 0.0;
    }

    /// <summary>
    /// This gets called once per frame during the main game update loop
    /// </summary>
    private void Update()
    {
        // This is a handy way to trigger the note from the editor.
        // It checks to see if you've ticked the checkbox, and if so
        // it starts a new note right away.
        if (_debugTrigger)
        {
            // Trigger now
            Trigger(AudioSettings.dspTime);
            // reset the trigger
            _debugTrigger = false;
        }
    }

    /// <summary>
    /// This gets called every time the AudioSource this ToneGenerator is attached to sends
    /// a new audio buffer toward the output.
    /// </summary>
    /// <param name="buffer">The array of samples being sent from the AudioSource</param>
    /// <param name="numChannels">The number of channels in the buffer (interleaved)</param>
    private void OnAudioFilterRead(float[] buffer, int numChannels)
    {
        // Get the current time from the audio system
        double currentTime = AudioSettings.dspTime;
        // If the current time is after the start time, the tone has started
        bool hasStarted = (currentTime > _startTime_s);
        // If there are no samples remaining, the tone has ended
        bool hasEnded = (_samplesRemaining == 0);

        // don't bother doing anything if we're out of range of the triggered tone
        if (!hasStarted || hasEnded)
        {
            return;
        }

        float sample = 0f;
        // loop through the samples in the buffer and generate a sine
        for (int sIdx = 0; sIdx < buffer.Length; sIdx += numChannels)
        {
            // get the next sample in a sine and multiply it by the volume
            sample = (float)Math.Sin(_angle) * _volume;           

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
