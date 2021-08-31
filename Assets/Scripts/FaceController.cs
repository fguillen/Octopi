using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceController : MonoBehaviour
{
    Vector3 originalPosition;
    [SerializeField] float movementOffset = 0.1f;

    void Awake()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if(!GameManagerController.Instance.EndGame())
        {
            Vector2 mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
            Vector2 newPosition = (Vector2)originalPosition + (direction * movementOffset);

            transform.localPosition = new Vector3(newPosition.x, newPosition.y, originalPosition.z);
        } else {
            transform.localPosition = originalPosition;
        }
    }
}
