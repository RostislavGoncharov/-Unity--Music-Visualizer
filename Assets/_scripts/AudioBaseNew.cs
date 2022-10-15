using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioBaseNew : MonoBehaviour
{
    public float[] samples = new float[64];
    public static float[] normalizedSamples = new float[64];

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
        NormalizeSamples();
    }

    void NormalizeSamples()
    {
        float maxValue = 0;

        for (int i = 0; i < samples.Length; i++)
        {
            if (samples[i] > maxValue)
            {
                maxValue = samples[i];
            }

            normalizedSamples[i] = samples[i] / maxValue;
        }
    }
}
