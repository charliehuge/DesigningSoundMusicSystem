using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeedControl : MonoBehaviour
{
    [SerializeField] private Metronome _metronome;
    [SerializeField, Range(30f, 240f)] private float _tempoMin = 60f;
    [SerializeField, Range(30f, 240f)] private float _tempoMax = 120f;

    /// <summary>
    /// Update the tempo based on the slider input
    /// </summary>
    /// <param name="sliderValue">The value of the slider. Note: expects a range of 0.0f-1.0f</param>
    public void OnSliderUpdate(float sliderValue)
    {
        float tempo = _tempoMin + ((_tempoMax - _tempoMin) * sliderValue);
        _metronome.SetTempo(tempo);
    }
}
