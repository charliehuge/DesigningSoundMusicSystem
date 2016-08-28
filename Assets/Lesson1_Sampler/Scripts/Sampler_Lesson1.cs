using UnityEngine;
using System.Collections;

public class Sampler_Lesson1 : MonoBehaviour
{
    [Header("Debug")]
    // Plays the sound when the toggle is pressed in the editor while the scene is running
    [SerializeField] private bool _debugPlay;

    [Header("Config")]
    // The audio file we want to play
    [SerializeField] private AudioClip _audioClip;
    // The number of voices. Set this higher if you're hearing excessive voice stealing.
    [SerializeField, Range(1, 8)] private int _numVoices = 2;
    // The prefab for the sampler voice, which is just a SamplerVoice component attached to a GameObject
    [SerializeField] private SamplerVoice_Lesson1 _samplerVoicePrefab;

    // The list of sampler voices we created during initialization
    private SamplerVoice_Lesson1[] _samplerVoices;
    // The index of the next voice to play
    private int _nextVoiceIndex;

    /// <summary>
    /// This gets called when the GameObject first gets created.
    /// Create our sampler voices here.
    /// </summary>
    private void Awake()
    {
        _samplerVoices = new SamplerVoice_Lesson1[_numVoices];

        for (int i = 0; i < _numVoices; ++i)
        {
            SamplerVoice_Lesson1 samplerVoice = Instantiate(_samplerVoicePrefab);
            samplerVoice.transform.parent = transform;
            samplerVoice.transform.localPosition = Vector3.zero;
            _samplerVoices[i] = samplerVoice;
        }

        _nextVoiceIndex = 0;
    }

    /// <summary>
    /// This gets called every game frame.
    /// Do our debug play here.
    /// </summary>
    private void Update()
    {
        // if _debugPlay is true, it means we ticked the toggle in the editor
        if (_debugPlay)
        {
            // play the sound
            Play();
            // turn off the toggle
            _debugPlay = false;
        }
    }

    /// <summary>
    /// Pick a voice and play the sound
    /// </summary>
    private void Play()
    {
        // Play the sound on the next voice
        _samplerVoices[_nextVoiceIndex].Play(_audioClip);
        // Increment the next voice and wrap it around if necessary
        ++_nextVoiceIndex;
        if (_nextVoiceIndex >= _samplerVoices.Length)
        {
            _nextVoiceIndex = 0;
        }
    }
}
