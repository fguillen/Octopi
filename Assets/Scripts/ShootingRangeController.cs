using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRangeController : MonoBehaviour
{
    [SerializeField] Shooter shooter;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
            shooter.InRange();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
            shooter.OutOfRange();
    }
}
