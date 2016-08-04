using System;

/// <summary>
/// Utilities to make converting various music things easy
/// </summary>
public static class MusicMathUtils
{
    public const int MidiNoteA440 = 69;

    /// <summary>
    /// Converts a MIDI note number to a frequency, based on A 440
    /// </summary>
    /// <param name="midiNote">MIDI note number to convert</param>
    /// <returns></returns>
    public static double MidiNoteToFrequency(int midiNote)
    {
        return 440f * Math.Pow(2.0, (midiNote - MidiNoteA440) / 12.0);
    }
}
