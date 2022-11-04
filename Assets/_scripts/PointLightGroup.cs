using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLightGroup : MonoBehaviour
{
    float rotateSpeed = 3f;

    void Update()
    {
        if (AudioBase.normalizedAverageVolume > 0.5f)
        {
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        }
    }
}
