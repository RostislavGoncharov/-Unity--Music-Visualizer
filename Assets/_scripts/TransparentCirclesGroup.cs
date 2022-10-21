using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentCirclesGroup : MonoBehaviour
{
    public Transform target;
    float speed;

    private void Start()
    {
        transform.rotation = Random.rotation;
        speed = Random.Range(0.1f, 0.2f) + AudioBase.normalizedBandBuffers[6] * 3;
    }

    private void Update()
    {
        
        //transform.Rotate(new Vector3(AudioBase.normalizedBandBuffers[1], AudioBase.normalizedBandBuffers[2], AudioBase.normalizedBandBuffers[5]) * Time.deltaTime, Space.World);
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed);

        if (transform.position.z >= 200)
        {
            Destroy(this.gameObject);
        }
        
    }
}
