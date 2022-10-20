using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] circleGroupPrefabs;
    [SerializeField] Transform target;

    private void Start()
    {
        StartCoroutine(SpawnCircles());
    }

    IEnumerator SpawnCircles()
    {
        while(true)
        {
            int index = Random.Range(0, circleGroupPrefabs.Length);
            GameObject circleGroup = Instantiate(circleGroupPrefabs[index], transform.position + Random.onUnitSphere * 10, Quaternion.identity);

            TransparentCirclesGroup circleGroupScript = circleGroup.GetComponent<TransparentCirclesGroup>();
            circleGroupScript.target = target;

            yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
        }
    }
}
