using System;
using System.Collections.Generic;
using UnityEngine;

public class StepSequencer : MonoBehaviour
{
    [Serializable]
    public class Step
    {
        public bool Active = false;
        public int NoteNumber = 60;
        public double Duration = 0.25;
        public float Volume = 1f;
    }

    public List<Step> Steps = new List<Step>();

    [SerializeField] private ToneGenerator _toneGenerator;

    private int _currentTick = 0;

    public void Tick()
    {
        int numSteps = Steps.Count;

        if (numSteps == 0)
        {
            return;
        }

        if (Steps[_currentTick].Active)
        {
            if (_toneGenerator != null)
            {
                double frequency = MusicMathUtils.MidiNoteToFrequency(Steps[_currentTick].NoteNumber);
                double duration = Steps[_currentTick].Duration;
                float volume = Steps[_currentTick].Volume;
                _toneGenerator.Play(frequency, duration, volume);
            }
        }

        _currentTick = (_currentTick + 1) % numSteps;
    }
}
