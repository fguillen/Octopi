using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowController : MonoBehaviour
{
    [SerializeField] GameObject windowBrokenPrefab;

    public void StartGrab()
    {
    }

    public void StopGrab()
    {
    }

    public void Thrown()
    {
        Instantiate(windowBrokenPrefab, transform.position, Quaternion.identity, transform.parent);
    }
}
