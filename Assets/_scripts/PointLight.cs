using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class PointLight : MonoBehaviour
{
    [SerializeField] int band;
    [SerializeField] float minIntensity;
    [SerializeField] float maxIntensity;

    Light light;

    private void Start()
    {
        light = GetComponent<Light>();
    }

    private void Update()
    {
        light.intensity = AudioBase.normalizedBandBuffers[band] * maxIntensity + minIntensity;
    }
}
