using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdSpawnerController : MonoBehaviour
{
    [SerializeField] GameObject birdPrefab;
    [SerializeField] float birdEachSeconds = 6;
    [SerializeField] float spawnAreaDelta = 2;
    [SerializeField] float birdVelocity = 2;
    float nextBirdAt;

    void Start()
    {
        SpawnBird();
    }

    void Update()
    {
        if(nextBirdAt <= Time.time)
            SpawnBird();
    }


    void SpawnBird() {
        // Debug.Log("[BirdSpawner].SpawnBird()");
        Vector3 birdPosition = new Vector3(transform.position.x, Utils.AddNoise(transform.position.y, spawnAreaDelta), Utils.AddNoise(transform.position.z, 0.1f)); // Adding z noise to avoid sprites render coupling
        GameObject bird = Instantiate(birdPrefab, birdPosition, Quaternion.identity, transform);
        nextBirdAt = Time.time + Utils.AddNoise(birdEachSeconds);

        bird.GetComponent<BirdController>().velocity = Utils.AddNoise(birdVelocity);
    }
}
