using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crosstales.FB;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System;

[RequireComponent(typeof(AudioSource))]
public class AudioBase : MonoBehaviour
{
    public delegate void StartPlaying();
    public static event StartPlaying OnStartPlaying;
    
    public delegate void StopPlaying();
    public static event StopPlaying OnStopPlaying;

    public static float[] samples = new float[512];
    public static float[] outputSamples = new float[512];
    public static float outputVolume;
    public static float normalizedAverageVolume;
    public static float[] normalizedBands = new float[8];
    public static float[] normalizedBandBuffers = new float[8];
    public static float[] bandBuffers = new float[8];

    public static float[] freqBands = new float[8];

    float[] freqBandHighest = new float[8];
    float[] bufferDecrease = new float[8];

    [SerializeField] float defaultBufferDecreaseValue = 0.005f;
    [SerializeField] float bufferDecreaseMultiplier = 1.2f;

    public AudioSource audioSource;

    bool isActive = true;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        for (int i = 0; i < normalizedBandBuffers.Length; i++)
        {
            normalizedBandBuffers[i] = 0.01f;
        }

        for (int i = 0; i < freqBandHighest.Length; i++)
        {
            freqBandHighest[i] = 0.01f;
        }
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        GetSpectrumAudioData();
        GetAverageVolume();
        GetFrequencyBands();
        UseBandBuffer();
        CreateNormalizedBands();

        if (!audioSource.isPlaying)
        {
            OnStopPlaying?.Invoke();
            isActive = false;
        }
    }

    public void Launch()
    {
        audioSource.Play();
        OnStartPlaying?.Invoke();
    }

    void GetSpectrumAudioData()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
    }

    void GetAverageVolume()
    {
        audioSource.GetOutputData(outputSamples, 1);

        outputVolume = 0;

        foreach (float sample in outputSamples)
        {
            outputVolume += sample;
        }
    }

    //void GetAverageVolume()
    //{
    //    float sum = 0;

    //    foreach(float buffer in normalizedBandBuffers)
    //    {
    //        sum += buffer;
    //    }

    //    normalizedAverageVolume = Mathf.Abs(sum / normalizedBandBuffers.Length);
    //}

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
        for (int i = 0; i < 8; i++)
        {
            if (bandBuffers[i] < freqBands[i])
            {
                bandBuffers[i] = freqBands[i];
                bufferDecrease[i] = defaultBufferDecreaseValue;
            }

            if (bandBuffers[i] > freqBands[i])
            {
                bandBuffers[i] -= bufferDecrease[i];
                bufferDecrease[i] *= bufferDecreaseMultiplier;
            }
        }
    }

    void CreateNormalizedBands()
    {
        for (int i = 0; i < 8; i++)
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
