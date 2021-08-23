using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawnerController : MonoBehaviour
{
    [SerializeField] GameObject cloudPrefab;
    [SerializeField] float cloudEachSeconds = 6;
    [SerializeField] float spawnAreaDelta = 2;
    [SerializeField] float cloudVelocity = 2;
    float nextCloudAt;

    void Start()
    {
        SpawnCloud();
    }

    void Update()
    {
        if(nextCloudAt <= Time.time)
            SpawnCloud();
    }


    void SpawnCloud() {
        Debug.Log("[CloudSpawner].SpawnCloud()");
        Vector3 cloudPosition = new Vector3(transform.position.x, Utils.AddNoise(transform.position.y, spawnAreaDelta), transform.position.z);
        GameObject cloud = Instantiate(cloudPrefab, cloudPosition, Quaternion.identity, transform);
        nextCloudAt = Time.time + Utils.AddNoise(cloudEachSeconds);

        cloud.GetComponent<CloudController>().velocity = Utils.AddNoise(cloudVelocity);
    }
}
