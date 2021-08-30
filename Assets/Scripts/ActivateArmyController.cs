using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateArmyController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            GameManagerController.Instance.ActivateArmy();
        }
    }
}
