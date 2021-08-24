using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSpawnerController : MonoBehaviour
{
    [SerializeField] List<GameObject> tankPrefabs;
    [SerializeField] float tankEachSeconds = 6;
    [SerializeField] float tankVelocity = 2;
    [SerializeField] RoadPatrolPointController firstPatrolPoint;
    float nextTankAt;

    void Start()
    {
        SpawnTank();
    }

    void Update()
    {
        if(nextTankAt <= Time.time)
            SpawnTank();
    }


    void SpawnTank() {
        // Debug.Log("[TankSpawner].SpawnTank()");
        float zPosition = Utils.AddNoise(transform.position.z, 0.1f); // Adding z noise to avoid sprites render coupling
        Vector3 tankPosition = new Vector3(transform.position.x, transform.position.y, zPosition);
        GameObject tank = Instantiate(RandomTank(), tankPosition, Quaternion.identity, transform);
        nextTankAt = Time.time + Utils.AddNoise(tankEachSeconds);

        tank.GetComponent<TankController>().velocity = Utils.AddNoise(tankVelocity);
        tank.GetComponent<TankController>().OriginalZ = zPosition;
        tank.GetComponent<TankController>().NextPatrolPoint(firstPatrolPoint);
    }

    GameObject RandomTank()
    {
        return tankPrefabs[Random.Range(0, tankPrefabs.Count)];
    }
}
