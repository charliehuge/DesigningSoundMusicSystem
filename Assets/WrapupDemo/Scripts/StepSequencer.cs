using System;
using System.Collections.Generic;
using UnityEngine;

public class StepSequencer : MonoBehaviour
{
    [Serializable]
    public struct Step
    {
        public bool Active;
        public int MidiNoteNumber;
        public double Duration;
    }

    public delegate void HandleTick(double tickTime, int midiNoteNumber, double duration);

    public event HandleTick Ticked;

    [SerializeField] private Metronome _metronome;

    [SerializeField] private List<Step> _steps;

    private int _currentTick = 0;

#if UNITY_EDITOR
    public List<Step> GetSteps()
    {
        return _steps;
    }
#endif

    private void OnEnable()
    {
        if (_metronome != null)
        {
            _metronome.Ticked += HandleTicked;
        }
    }

    private void OnDisable()
    {
        if (_metronome != null)
        {
            _metronome.Ticked -= HandleTicked;
        }
    }

    public void HandleTicked(double tickTime)
    {
        int numSteps = _steps.Count;

        if (numSteps == 0)
        {
            return;
        }

        Step step = _steps[_currentTick];

        if (step.Active)
        {
            if (Ticked != null)
            {
                Ticked(tickTime, step.MidiNoteNumber, step.Duration);
            }
        }

        _currentTick = (_currentTick + 1) % numSteps;
    }
}
