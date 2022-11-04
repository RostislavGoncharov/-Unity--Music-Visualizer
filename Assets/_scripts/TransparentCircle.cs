using System.Collections;
using System.Collections.Generic;
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
        auraLight.LightComponent.intensity = AudioBase.outputVolume * 100;
    }
}
