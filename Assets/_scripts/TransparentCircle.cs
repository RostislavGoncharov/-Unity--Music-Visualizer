using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentCircle : MonoBehaviour
{
    [SerializeField] int band;
    [SerializeField] Transform circlesGroup;

    Material material;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
        transform.rotation = Random.rotation;
    }

    private void Update()
    {
        material.color = new Color(material.color.r, material.color.g, material.color.b, AudioBase.normalizedBandBuffers[band]);
        //transform.Rotate(new Vector3(AudioBase.normalizedBandBuffers[band], AudioBase.normalizedBandBuffers[1], AudioBase.normalizedBandBuffers[7]) * Time.deltaTime);
    }
}
