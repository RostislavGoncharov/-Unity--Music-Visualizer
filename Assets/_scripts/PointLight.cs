/*
 * This class controls the intensity of point lights in the scene.
 * Select a band 0 to 7 in the inspector to choose the frequencies the light will react to.
 */

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

        // Prevent the band index from going out of bounds.
        if (band < 0)
        {
            band = 0;
        }

        if (band > 7)
        {
            band = 7;
        }
    }

    private void Update()
    {
        light.intensity = AudioBase.normalizedBandBuffers[band] * maxIntensity + minIntensity;
    }
}
