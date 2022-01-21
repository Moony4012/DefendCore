using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinMaxAttribute : PropertyAttribute
{
    public float Min, Max;

    public MinMaxAttribute(float min, float max)
    {
        Min = min;
        Max = max;
    }
}

[Serializable]
public class MinMaxPair
{
    public float Min, Max;

    public MinMaxPair()
    {
        Min = 0.0f;
        Max = 1.0f;
    }

    public MinMaxPair(float min, float max)
    {
        Min = min;
        Max = max;
    }

    public float Clamp(float value)
    {
        return Mathf.Clamp(value, Min, Max);
    }

    public float RandomValue
    {
        get { return UnityEngine.Random.Range(Min, Max); }
    }
}
