using System;
using UnityEngine;
using System.Collections;

public class IntensityControl : MonoBehaviour
{
    /// <summary>
    /// Container to map StepSequencer instances to an "intensity" value
    /// </summary>
    [Serializable]
    public class SequencerIntensityPair
    {
        public StepSequencer Sequencer;
        public float Intensity;
    }

    [SerializeField] private SequencerIntensityPair[] _sequencerIntensityPairs;

    /// <summary>
    /// When the slider gets updated, turn sequencers on or off based on the slider value
    /// </summary>
    /// <param name="sliderValue">The value of the slider.</param>
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
