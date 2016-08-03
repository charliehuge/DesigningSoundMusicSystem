using System;
using System.Collections.Generic;
using UnityEngine;

public class StepSequencer : MonoBehaviour
{
    [Serializable]
    public struct Step
    {
        public bool Active;
        public int NoteNumber;
        public float Volume;
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
                _toneGenerator.Play();
            }
        }

        _currentTick = (_currentTick + 1) % numSteps;
    }
}
