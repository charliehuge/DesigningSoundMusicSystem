using System;
using UnityEngine;

/// <summary>
/// Metronome
/// 
/// Sends a "tick" every subdivision of a beat at the specified tempo
/// 
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Metronome : MonoBehaviour
{
    // Assign a ToneGenerator here for testing
    [SerializeField] private ToneGenerator _toneGenerator;
    // The tempo in beats per minute
    [SerializeField, Range(30f, 240f)] private double _tempo_bpm = 120.0;
    // Number of subdivisions in each beat
    [SerializeField, Range(1, 8)] private uint _subdivisions = 4;

    private int _sampleRate = 0;
    private uint _samplesPerTick = 0;
    private uint _currentSample = 0;

    public void Reset()
    {
        Recalculate();
        _currentSample = 0;
    }

    /// <summary>
    /// Recalculate the samples per tick.
    /// </summary>
    public void Recalculate()
    {
        if (_tempo_bpm > 0.0)
        {
            // convert beats per minute to beats per second
            double beatsPerSecond = _tempo_bpm / 60.0;
            // multiply by the number of subdivisions to get the number of ticks per second
            double ticksPerSecond = beatsPerSecond * _subdivisions;
            // divide the sample rate (samples per second) by the ticks per second to get samples per tick
            _samplesPerTick = (uint) (_sampleRate / ticksPerSecond);
        }
        else
        {
            _samplesPerTick = 0;
        }
    }

    /// <summary>
    /// This gets called every time the object is enabled.
    /// </summary>
    private void OnEnable()
    {
        _sampleRate = AudioSettings.outputSampleRate;
        Reset();
    }

    /// <summary>
    /// This gets called when some editor UI control changes, 
    /// such as the sliders in the inspector
    /// </summary>
    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        // Update the tempo/subdivisions
        Recalculate();
    }

    /// <summary>
    /// This gets called every time the attached AudioSource sends
    /// a new audio buffer toward the output.
    /// </summary>
    /// <param name="buffer">The array of samples being sent from the AudioSource</param>
    /// <param name="numChannels">The number of channels in the buffer (interleaved)</param>
    private void OnAudioFilterRead(float[] buffer, int numChannels)
    {
        // bail out if we haven't initialized or have a zero tempo
        if (_samplesPerTick == 0)
        {
            return;
        }

        // the length of the buffer in samples for one channel
        uint actualSamples = (uint)(buffer.Length / numChannels);

        for (int i = 0; i < actualSamples; ++i)
        {
            // if the current sample equals zero, then it means we just started
            // or just looped around. That's when we want to trigger a tick.
            if (_currentSample == 0)
            {
                // if we have a ToneGenerator hooked up, tick it
                if (_toneGenerator != null)
                {
                    _toneGenerator.Play();
                }
            }

            // increment the sample counter, then wrap it by the number of samples per tick
            _currentSample = (_currentSample + 1) % _samplesPerTick;
        }
    }
}
