using System;
using UnityEngine;
using System.Collections;

public class IntensityControl : MonoBehaviour
{
    [Serializable]
    public class SequencerIntensityPair
    {
        public StepSequencer Sequencer;
        public float Intensity;
    }

    [SerializeField] private SequencerIntensityPair[] _sequencerIntensityPairs;

    public void OnSliderUpdate(float sliderValue)
    {
        foreach (var sequencerIntensityPair in _sequencerIntensityPairs)
        {
            sequencerIntensityPair.Sequencer.Suspend = (sliderValue < sequencerIntensityPair.Intensity);
        }
    }

    private void Start()
    {
        OnSliderUpdate(0f);
    }
}
