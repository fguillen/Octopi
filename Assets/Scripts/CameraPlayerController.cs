using UnityEngine;
using Cinemachine;

public class CameraPlayerController : MonoBehaviour
{
    CinemachineVirtualCamera cinemachineVirtualCamera;

    void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        Vector2 mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)PlayerController.instance.transform.position).normalized;
        float distanceToMousePosition = Vector2.Distance(PlayerController.instance.transform.position, mousePosition);


        Debug.Log($"distanceToMousePosition: {distanceToMousePosition}");

        float orthographicSize = Mathf.Lerp(5, 6, distanceToMousePosition / 13.0f);
        cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;
    }


}
