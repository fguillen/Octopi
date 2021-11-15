using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameWrapperController : MonoBehaviour
{
    [SerializeField] Transform startPosition;

    public void MoveToStartPosition()
    {
        transform.position = startPosition.position;
    }
}
