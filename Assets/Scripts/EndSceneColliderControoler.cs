using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneColliderController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            GameManagerController.Instance.PlayEndScene();
        }
    }
}
