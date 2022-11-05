/*
 * This class is attached to Transparent Circles Group prefabs.
 * It controls their scale, rotation and movement towards the Target position.
 */

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
        baseSpeed = Mathf.Abs(AudioBase.normalizedAverageVolume * 100);

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
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (transform.position.z >= 1000 || transform.position.z <= -1000)
        {
            Destroy(this.gameObject);
        }
        
    }
}
