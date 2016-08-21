using UnityEngine;

public class Sampler_Lesson4 : MonoBehaviour
{
    [SerializeField] private StepSequencer_Lesson4 _sequencer;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField, Range(0f, 2f)] private double _attackTime;
    [SerializeField, Range(0f, 2f)] private double _releaseTime;
    [SerializeField, Range(1, 8)] private int _numVoices = 2;
    [SerializeField] private SamplerVoice_Lesson4 _samplerVoicePrefab;

    private SamplerVoice_Lesson4[] _samplerVoices;
    private int _nextVoiceIndex;

    private void Awake()
    {
        _samplerVoices = new SamplerVoice_Lesson4[_numVoices];

        for (int i = 0; i < _numVoices; ++i)
        {
            SamplerVoice_Lesson4 samplerVoice = Instantiate(_samplerVoicePrefab);
            samplerVoice.transform.parent = transform;
            samplerVoice.transform.localPosition = Vector3.zero;
            _samplerVoices[i] = samplerVoice;
        }
    }

    private void OnEnable()
    {
        if (_sequencer != null)
        {
            _sequencer.Ticked += HandleTicked;
        }
    }

    private void OnDisable()
    {
        if (_sequencer != null)
        {
            _sequencer.Ticked -= HandleTicked;
        }
    }

    private void HandleTicked(double tickTime, int midiNoteNumber, double duration)
    {
        float pitch = MusicMathUtils_Lesson4.MidiNoteToPitch(midiNoteNumber, MusicMathUtils_Lesson4.MidiNoteC4);
        _samplerVoices[_nextVoiceIndex].Play(_audioClip, pitch, tickTime, _attackTime, duration, _releaseTime);

        _nextVoiceIndex = (_nextVoiceIndex + 1) % _samplerVoices.Length;
    }
}
