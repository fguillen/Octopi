using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneSpawnerController : MonoBehaviour
{
    [SerializeField] GameObject airplanePrefab;
    [SerializeField] float airplaneEachSeconds = 6;
    [SerializeField] float spawnAreaDelta = 2;
    [SerializeField] float airplaneVelocity = 2;
    float nextAirplaneAt;

    void Start()
    {
        SpawnAirplane();
    }

    void Update()
    {
        if(nextAirplaneAt <= Time.time)
            SpawnAirplane();
    }


    void SpawnAirplane() {
        // Debug.Log("[AirplaneSpawner].SpawnAirplane()");
        Vector3 airplanePosition = new Vector3(transform.position.x, Utils.AddNoise(transform.position.y, spawnAreaDelta), Utils.AddNoise(transform.position.z, 0.1f)); // Adding z noise to avoid sprites render coupling
        GameObject airplane = Instantiate(airplanePrefab, airplanePosition, Quaternion.identity, transform);
        nextAirplaneAt = Time.time + Utils.AddNoise(airplaneEachSeconds);

        airplane.GetComponent<AirplaneController>().velocity = Utils.AddNoise(airplaneVelocity);
    }
}
