/*
 * This class controls the point light group positioned in front of the main camera.
 * It currently causes the lights to rotate based on audio volume.
 */

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
