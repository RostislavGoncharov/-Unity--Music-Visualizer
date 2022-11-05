/*
 * This class controls the scale, rotation and light intensity
 * of each Transparent Circle moving through the scene.
 */


using UnityEngine;
using Aura2API;

[RequireComponent(typeof(AuraLight))]
public class TransparentCircle : MonoBehaviour
{
    [SerializeField] Transform circlesGroup;

    AuraLight auraLight;

    private void Start()
    {
        auraLight = GetComponent<AuraLight>();
        auraLight.LightComponent.range = 1;
        auraLight.LightComponent.intensity = 10;

        transform.rotation = Random.rotation;
        transform.localScale *= Random.Range(0.5f, 3f);
    }

    private void Update()
    {
        auraLight.LightComponent.intensity = AudioBase.normalizedAverageVolume * 100;
    }
}
