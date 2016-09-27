using System;
using System.Collections.Generic;
using UnityEngine;

public class StepSequencer : MonoBehaviour
{
    [Serializable]
    public class Step
    {
        public bool Active;
        public int MidiNoteNumber;
        public double Duration;
    }

    public delegate void HandleTick(double tickTime, int midiNoteNumber, double duration);

    public event HandleTick Ticked;

    public bool Suspend;

    [SerializeField] private Metronome _metronome;

    // hide this field in the inspector. We'll be making a custom inspector for these
    [SerializeField, HideInInspector] private List<Step> _steps;

    private int _currentTick = 0;

    // in the editor only, add a property to get the list of steps
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
            if (!Suspend && Ticked != null)
            {
                Ticked(tickTime, step.MidiNoteNumber, step.Duration);
            }
        }

        _currentTick = (_currentTick + 1) % numSteps;
    }
}
