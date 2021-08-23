using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{
    public float velocity = -1;

    void Update()
    {
        transform.Translate(new Vector3(velocity, 0f, 0f) * Time.deltaTime);
    }
}
