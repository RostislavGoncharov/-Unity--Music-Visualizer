using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraController : MonoBehaviour
{
    LensDistortion lensDistortion;
    [SerializeField] int distortionBand = 1;

    private void Start()
    {
        lensDistortion = GetComponent<LensDistortion>();
    }

    private void Update()
    {
        lensDistortion.intensity.value = -100 * AudioBase.normalizedBandBuffers[distortionBand];
    }
}
