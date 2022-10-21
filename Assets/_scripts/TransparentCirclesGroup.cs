using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentCirclesGroup : MonoBehaviour
{
    public Transform target;
    float baseSpeed;
    float speedMultiplier = 3.0f;
    float speed;

    private void Start()
    {
        transform.rotation = Random.rotation;
        transform.localScale *= Random.Range(0.5f, 3f);
        baseSpeed = Mathf.Abs(AudioBase.normalizedAverageVolume * 5);

        if (AudioBase.normalizedAverageVolume >= 0.5f)
        {
            speed = baseSpeed * speedMultiplier;
        }

        else
        {
            speed = baseSpeed;
        }
    }

    private void Update()
    {
        //transform.Rotate(new Vector3(AudioBase.normalizedBandBuffers[1], AudioBase.normalizedBandBuffers[2], AudioBase.normalizedBandBuffers[5]) * Time.deltaTime, Space.World);
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (transform.position.z >= 1000 || transform.position.z <= -1000)
        {
            Destroy(this.gameObject);
        }
        
    }
}
