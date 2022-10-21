using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] circleGroupPrefabs;
    [SerializeField] Transform target;

    bool isActive = true;

    private void OnEnable()
    {
        AudioBase.onStopPlaying += StopSpawning;
    }

    private void OnDisable()
    {
        AudioBase.onStopPlaying -= StopSpawning;
    }

    private void Start()
    {
        StartCoroutine(SpawnCircles());
    }

    IEnumerator SpawnCircles()
    {
        while(isActive)
        {
            int index = Random.Range(0, circleGroupPrefabs.Length);
            GameObject circleGroup = Instantiate(circleGroupPrefabs[index], transform.position + Random.onUnitSphere * 30, Quaternion.identity);

            TransparentCirclesGroup circleGroupScript = circleGroup.GetComponent<TransparentCirclesGroup>();
            circleGroupScript.target = target;

            yield return new WaitForSeconds(Random.Range(0.5f, 2.0f) - AudioBase.normalizedBandBuffers[7]);
        }
    }

    void StopSpawning()
    {
        isActive = false;
    }
}
