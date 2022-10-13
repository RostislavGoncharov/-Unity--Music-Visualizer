using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLight : MonoBehaviour
{
    [SerializeField] int band;

    Light light;

    private void Start()
    {
        light = GetComponent<Light>();
    }

    private void Update()
    {
        light.intensity = AudioBase.normalizedBands[band];
    }
}
