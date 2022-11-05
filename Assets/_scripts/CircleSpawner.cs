using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] circleGroupPrefabs;
    [SerializeField] Transform target;

    bool isActive = false;

    private void OnEnable()
    {
        AudioBase.OnStopPlaying += StopSpawning;
        GameManager.OnStartPlaying += StartSpawning;
    }

    private void OnDisable()
    {
        AudioBase.OnStopPlaying -= StopSpawning;
        GameManager.OnStartPlaying -= StartSpawning;
    }

    IEnumerator SpawnCircles()
    {
        while(isActive)
        {
            int index = Random.Range(0, circleGroupPrefabs.Length);
            GameObject circleGroup = Instantiate(circleGroupPrefabs[index], transform.position + Random.onUnitSphere * 30, Quaternion.identity);

            TransparentCirclesGroup circleGroupScript = circleGroup.GetComponent<TransparentCirclesGroup>();
            circleGroupScript.target = target;

            yield return new WaitForSeconds(Mathf.Abs(Random.Range(0.7f, 1.5f) - AudioBase.normalizedAverageVolume));
        }
    }

    void StartSpawning()
    {
        isActive = true;
        StartCoroutine(SpawnCircles());
    }

    void StopSpawning()
    {
        isActive = false;
    }
}
