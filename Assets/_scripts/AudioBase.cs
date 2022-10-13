using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioBase : MonoBehaviour
{
    public static float[] samples = new float[512];
    public static float[] normalizedBands = new float[8];
    public static float[] normalizedBandBuffers = new float[8];

    static float[] freqBands = new float[8];
    static float[] bandBuffers = new float[8];

    float[] bufferDecrease = new float[bandBuffers.Length];

    [SerializeField] float defaultBufferDecreaseValue = 0.005f;
    [SerializeField] float bufferDecreaseMultiplier = 1.2f;

    AudioSource audioSource;

    float[] freqBandHighest = new float[freqBands.Length];

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        GetSpectrumAudioData();
        GetFrequencyBands();
        UseBandBuffer();
        CreateNormalizedBands();
    }

    void GetSpectrumAudioData()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
    }

    // Formula to split all samples into a smaller number of frequency bands
    void GetFrequencyBands()
    {
        int count = 0;

        for (int i = 1; i <= freqBands.Length; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i);

            if (i == freqBands.Length)
            {
                sampleCount += 2;
            }

            for (int j = 0; j < sampleCount; j++)
            {
                average += samples[count] * (count + 1);
                count++;
            }

            average /= count;
            freqBands[i - 1] = average * 10;

        }
    }

    // Method to smooth out value changes
    void UseBandBuffer()
    {
        for (int i = 0; i < bandBuffers.Length; i++)
        {
            if (bandBuffers[i] < freqBands[i])
            {
                bandBuffers[i] = freqBands[i];
                bufferDecrease[i] = defaultBufferDecreaseValue;
            }

            if (bandBuffers[i] > freqBands[i])
            {
                freqBands[i] -= bufferDecrease[i];
                bufferDecrease[i] *= bufferDecreaseMultiplier;
            }
        }
    }

    void CreateNormalizedBands()
    {
        for (int i = 0; i < freqBands.Length; i++)
        {
            if (freqBands[i] > freqBandHighest[i])
            {
                freqBandHighest[i] = freqBands[i];
            }

            normalizedBands[i] = freqBands[i] / freqBandHighest[i];
            normalizedBandBuffers[i] = bandBuffers[i] / freqBandHighest[i];
        }
    }
}
