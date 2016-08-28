using System;
using UnityEngine;
using System.Collections;

public class ASREnvelope
{
    private enum State
    {
        Idle,
        Attack,
        Sustain,
        Release
    }

    private State _state;
    private double _attackIncrement;
    private uint _sustainSamples;
    private double _releaseIncrement;
    private double _outputLevel;

    public void Reset(double attackTime_s, double sustainTime_s, double releaseTime_s, int sampleRate)
    {
        _state = State.Attack;
        _attackIncrement = (attackTime_s > 0.0) ? (1.0 / (attackTime_s * sampleRate)) : 1.0;
        _sustainSamples = (uint)(sustainTime_s * sampleRate);
        _releaseIncrement = (releaseTime_s > 0.0) ? (1.0 / (releaseTime_s * sampleRate)) : 1.0;
        _outputLevel = 0.0;
    }

    public double GetLevel()
    {
        switch (_state)
        {
            case State.Idle:
                _outputLevel = 0.0;
                break;
            case State.Attack:
                _outputLevel += _attackIncrement;

                if (_outputLevel > 1.0)
                {
                    _outputLevel = 1.0;
                    _state = State.Sustain;
                }

                break;
            case State.Sustain:
                if ((_sustainSamples == 0) || (--_sustainSamples == 0))
                {
                    _state = State.Release;
                }

                break;
            case State.Release:
                _outputLevel -= _releaseIncrement;

                if (_outputLevel < 0.0)
                {
                    _outputLevel = 0.0;
                    _state = State.Idle;
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return _outputLevel;
    }
}
