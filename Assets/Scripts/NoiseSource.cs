using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NoiseSource
{
    public float frequency;
    public float amplitude;
    public uint octaves;
    public float persistence;
    public float scale;

    private float maxValue;

    public NoiseSource()
    {

    }

    public float Value(float x, float y)
    {
        float maxValue = 0f;
        float ret = 0f;
        float tmpFrequency = frequency;
        float tmpAmplitude = amplitude;


        /* Algorithm taken from:
            https://flafla2.github.io/2014/08/09/perlinnoise.html
        */

        for(uint o = 0; o < octaves; o++) {
            ret += Mathf.PerlinNoise(x * tmpFrequency, y * tmpFrequency) * tmpAmplitude;

            maxValue += tmpAmplitude;

            tmpAmplitude *= persistence;
            tmpFrequency *= 2;
        }

        return scale * ret / maxValue;
    }
}
