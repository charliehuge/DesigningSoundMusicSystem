using UnityEngine;
using System.Collections;

public class Sampler_Lesson3 : MonoBehaviour
{
    [SerializeField] private Metronome_Lesson3 _metronome;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField, Range(0f, 2f)] private double _attackTime;
    [SerializeField, Range(0f, 2f)] private double _sustainTime;
    [SerializeField, Range(0f, 2f)] private double _releaseTime;
    [SerializeField, Range(1, 8)] private int _numVoices = 2;
    [SerializeField] private SamplerVoice_Lesson3 _samplerVoicePrefab;

    private SamplerVoice_Lesson3[] _samplerVoices;
    private int _nextVoiceIndex;

    private void Awake()
    {
        _samplerVoices = new SamplerVoice_Lesson3[_numVoices];

        for (int i = 0; i < _numVoices; ++i)
        {
            SamplerVoice_Lesson3 samplerVoice = Instantiate(_samplerVoicePrefab);
            samplerVoice.transform.parent = transform;
            samplerVoice.transform.localPosition = Vector3.zero;
            _samplerVoices[i] = samplerVoice;
        }
    }

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

    private void HandleTicked(double tickTime)
    {
        _samplerVoices[_nextVoiceIndex].Play(_audioClip, tickTime, _attackTime, _sustainTime, _releaseTime);

        _nextVoiceIndex = (_nextVoiceIndex + 1) % _samplerVoices.Length;
    }
}