/*
 * This class handles all audio analysis and should be attached to the music source in the scene.
 */

using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioBase : MonoBehaviour
{    
    public delegate void StopPlaying();
    public static event StopPlaying OnStopPlaying;

    public static float[] samples = new float[512];
    public static float normalizedAverageVolume;
    public static float[] normalizedBandBuffers = new float[8];
    public static float[] bandBuffers = new float[8];
    public static float[] freqBands = new float[8];

    public static bool isActive = false;

    float[] freqBandHighest = new float[8];
    float[] bufferDecrease = new float[8];

    [SerializeField] float defaultBufferDecreaseValue = 0.005f;
    [SerializeField] float bufferDecreaseMultiplier = 1.2f;

    public AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // Set the initial values to non-zero to avoid division by zero in CreateNormalizedBands.
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
        GetFrequencyBands();
        CreateBandBuffers();
        CreateNormalizedBands();
        GetNormalizedAverageVolume();

        // Raise an event and reset the scene upon reaching the end of the audio clip.
        if (!audioSource.isPlaying)
        {
            OnStopPlaying?.Invoke();
            isActive = false;

            GameManager.Instance.ResetScene();
        }
    }

    void GetSpectrumAudioData()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
    }

    // Split all samples into a smaller number of frequency bands.
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

    // Smooth out value changes.
    void CreateBandBuffers()
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
                // Decrease the value by bufferDecrease value which keeps increasing
                bandBuffers[i] -= bufferDecrease[i];
                bufferDecrease[i] *= bufferDecreaseMultiplier;
            }
        }
    }

    // Convert band buffer values into values between 0 and 1.
    void CreateNormalizedBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if (freqBands[i] > freqBandHighest[i])
            {
                freqBandHighest[i] = freqBands[i];
            }

            normalizedBandBuffers[i] = bandBuffers[i] / freqBandHighest[i];
        }
    }
    
    // Find the average value of all normalized band buffers. This value will be between 0 and 1.
    void GetNormalizedAverageVolume()
    {
        float sum = 0;

        foreach (float buffer in normalizedBandBuffers)
        {
            sum += buffer;
        }

        normalizedAverageVolume = Mathf.Abs(sum / normalizedBandBuffers.Length);
    }
}
