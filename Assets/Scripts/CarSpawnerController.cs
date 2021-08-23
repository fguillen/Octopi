using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawnerController : MonoBehaviour
{
    [SerializeField] List<GameObject> carPrefabs;
    [SerializeField] float carEachSeconds = 6;
    [SerializeField] float carVelocity = 2;
    [SerializeField] RoadPatrolPointController firstPatrolPoint;
    float nextCarAt;

    void Start()
    {
        SpawnCar();
    }

    void Update()
    {
        if(nextCarAt <= Time.time)
            SpawnCar();
    }


    void SpawnCar() {
        Debug.Log("[CarSpawner].SpawnCar()");
        float zPosition = Utils.AddNoise(transform.position.z, 0.1f); // Adding z noise to avoid sprites render coupling
        Vector3 carPosition = new Vector3(transform.position.x, transform.position.y, zPosition);
        GameObject car = Instantiate(RandomCar(), carPosition, Quaternion.identity, transform);
        nextCarAt = Time.time + Utils.AddNoise(carEachSeconds);

        car.GetComponent<CarController>().velocity = Utils.AddNoise(carVelocity);
        car.GetComponent<CarController>().OriginalZ = zPosition;
        car.GetComponent<CarController>().NextPatrolPoint(firstPatrolPoint);
    }

    GameObject RandomCar()
    {
        return carPrefabs[Random.Range(0, carPrefabs.Count)];
    }
}
