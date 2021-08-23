using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    public float velocity = -1;

    void Update()
    {
        transform.Translate(new Vector3(velocity, 0f, 0f) * Time.deltaTime);

        if(
            (velocity > 0 && transform.localScale.x > 0) ||
            (velocity < 0 && transform.localScale.x < 0)
        )
        {
            transform.localScale = new Vector3(- transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }
}
