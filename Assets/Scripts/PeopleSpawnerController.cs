using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleSpawnerController : MonoBehaviour
{
    [SerializeField] List<GameObject> personPrefabs;
    [SerializeField] float personEachSeconds = 6;
    [SerializeField] float personVelocity = 2;
    [SerializeField] RoadPatrolPointController firstPatrolPoint;
    float nextPersonAt;

    void Start()
    {
        SpawnPerson();
    }

    void Update()
    {
        if(nextPersonAt <= Time.time)
            SpawnPerson();
    }


    void SpawnPerson() {
        // Debug.Log("[PersonSpawner].SpawnPerson()");
        float zPosition = Utils.AddNoise(transform.position.z, 0.1f); // Adding z noise to avoid sprites render coupling
        Vector3 personPosition = new Vector3(transform.position.x, transform.position.y, zPosition);
        GameObject person = Instantiate(RandomPerson(), personPosition, Quaternion.identity, transform);
        nextPersonAt = Time.time + Utils.AddNoise(personEachSeconds);

        person.GetComponent<PersonController>().velocity = Utils.AddNoise(personVelocity);
        person.GetComponent<PersonController>().NextPatrolPoint(firstPatrolPoint);
    }

    GameObject RandomPerson()
    {
        return personPrefabs[Random.Range(0, personPrefabs.Count)];
    }
}
