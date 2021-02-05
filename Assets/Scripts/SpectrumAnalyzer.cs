using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumAnalyzer : MonoBehaviour {
    AudioSource audioSource;
    public static float[] samples = new float[512];
    public static float[] freqBand = new float[8];
    float[] freqBandHighest = new float[8];
    public static float[] audioBands = new float[8];

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        CreateAudioBands();
    }

    void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    void MakeFrequencyBands()
    {
        int count = 0;

        // Iterate through the 8 bins.
        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i + 1);

            // Adding the remaining two samples into the last bin.
            if (i == 7)
            {
                sampleCount += 2;
            }

            // Go through the number of samples for each bin, add the data to the average
            for (int j = 0; j < sampleCount; j++)
            {
                average += samples[count];
                count++;
            }

            // Divide to create the average, and scale it appropriately.
            average /= count;
            freqBand[i] = (i + 1) * 100 * average;
        }
    }

    void CreateAudioBands()
    {
        int count = 0;

        // Iterate through the 8 bins.
        for (int i = 0; i < 8; i++)
        {
            float maxSample = 0;
            int sampleCount = (int)Mathf.Pow(2, i + 1);

            // Adding the remaining two samples into the last bin.
            if (i == 7)
            {
                sampleCount += 2;
            }

            // Go through the number of samples for each bin, track the highest sample
            for (int j = 0; j < sampleCount; j++)
            {
                float sample = samples[count];
                if (sample > maxSample)
                {
                    maxSample = sample;
                }
                count++;
            }

            // Set the max sample
            freqBandHighest[i] = maxSample;
            audioBands[i] = freqBand[i] / maxSample;
        }
    }
}
