using System;
using UnityEngine;

/// <summary>
/// Metronome
/// 
/// Sends a "tick" every subdivision at the specified tempo
/// 
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Metronome : MonoBehaviour
{
    [SerializeField] private ToneGenerator _toneGenerator;
    [SerializeField, Range(30f, 240f)] private double _tempo_bpm = 120.0;
    [SerializeField, Range(1, 8)] private uint _subdivisions = 4;

    private uint _samplesPerTick = 0;
    private uint _currentSample = 0;

    public void Reset()
    {
        if (_tempo_bpm > 0.0)
        {
            double beatsPerSecond = _tempo_bpm / 60.0;
            double ticksPerSecond = beatsPerSecond * _subdivisions;
            _samplesPerTick = (uint) (AudioSettings.outputSampleRate / ticksPerSecond);
        }
        else
        {
            _samplesPerTick = 0;
        }

        _currentSample = 0;
    }

    private void OnEnable()
    {
        Reset();
    }

    private void OnAudioFilterRead(float[] buffer, int numChannels)
    {
        if (_samplesPerTick == 0)
        {
            return;
        }

        uint actualSamples = (uint)(buffer.Length / numChannels);

        for (int i = 0; i < actualSamples; ++i)
        {
            if (_currentSample == 0)
            {
                _toneGenerator.Play();
            }

            _currentSample = (_currentSample + 1) % _samplesPerTick;
        }
    }
}
