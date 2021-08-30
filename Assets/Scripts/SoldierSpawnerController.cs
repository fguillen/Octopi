using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierSpawnerController : MonoBehaviour
{
    [SerializeField] List<GameObject> soldierPrefabs;
    [SerializeField] float soldierEachSeconds = 6;
    [SerializeField] float soldierVelocity = 2;
    [SerializeField] RoadPatrolPointController firstPatrolPoint;
    float nextSoldierAt;

    void Start()
    {
        if(GameManagerController.Instance.CanMoreSoldiers())
            SpawnSoldier();
    }

    void Update()
    {
        if(nextSoldierAt <= Time.time && GameManagerController.Instance.CanMoreSoldiers())
            SpawnSoldier();
    }


    void SpawnSoldier() {
        // Debug.Log("[SoldierSpawner].SpawnSoldier()");
        float zPosition = Utils.AddNoise(transform.position.z, 0.1f); // Adding z noise to avoid sprites render coupling
        Vector3 soldierPosition = new Vector3(transform.position.x, transform.position.y, zPosition);
        GameObject soldier = Instantiate(RandomSoldier(), soldierPosition, Quaternion.identity, transform);
        nextSoldierAt = Time.time + Utils.AddNoise(soldierEachSeconds);

        soldier.GetComponent<SoldierController>().velocity = Utils.AddNoise(soldierVelocity);
        soldier.GetComponent<SoldierController>().NextPatrolPoint(firstPatrolPoint);
    }

    GameObject RandomSoldier()
    {
        return soldierPrefabs[Random.Range(0, soldierPrefabs.Count)];
    }
}
