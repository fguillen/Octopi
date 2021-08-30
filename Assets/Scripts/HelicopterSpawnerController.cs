using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterSpawnerController : MonoBehaviour
{
    [SerializeField] List<GameObject> helicopterPrefabs;
    [SerializeField] float helicopterEachSeconds = 6;
    [SerializeField] float helicopterVelocity = 2;
    float nextHelicopterAt;

    void Start()
    {
        SpawnHelicopter();
    }

    void Update()
    {
        if(nextHelicopterAt <= Time.time && GameManagerController.instance.CanMoreHelicopters())
            SpawnHelicopter();
    }


    void SpawnHelicopter() {
        // Debug.Log("[HelicopterSpawner].SpawnHelicopter()");
        float zPosition = Utils.AddNoise(transform.position.z, 0.1f); // Adding z noise to avoid sprites render coupling
        Vector3 helicopterPosition = new Vector3(transform.position.x, transform.position.y, zPosition);
        GameObject helicopter = Instantiate(RandomHelicopter(), helicopterPosition, Quaternion.identity, transform);
        nextHelicopterAt = Time.time + Utils.AddNoise(helicopterEachSeconds);

        helicopter.GetComponent<HelicopterController>().velocity = Utils.AddNoise(helicopterVelocity);
        helicopter.GetComponent<HelicopterController>().OriginalZ = zPosition;
    }

    GameObject RandomHelicopter()
    {
        return helicopterPrefabs[Random.Range(0, helicopterPrefabs.Count)];
    }
}
