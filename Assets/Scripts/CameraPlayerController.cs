using UnityEngine;
using Cinemachine;

public class CameraPlayerController : MonoBehaviour
{
    CinemachineFramingTransposer cinemachineFramingTransposer;

    void Awake()
    {
        cinemachineFramingTransposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    void Update()
    {
        Vector2 mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distanceToMousePositionX = PlayerController.instance.transform.position.x - mousePosition.x;
        float distanceToMousePositionY = PlayerController.instance.transform.position.y - mousePosition.y;
        float screenX = Mathf.Lerp(0.371f, 0.556f, distanceToMousePositionX / 8.0f);
        float screenY = Mathf.Lerp(0.624f, 0.357f, distanceToMousePositionY / 6.0f);

        // Debug.Log($"distanceToMousePositionX: {distanceToMousePositionX}");
        // Debug.Log($"distanceToMousePositionY: {distanceToMousePositionY}");

        cinemachineFramingTransposer.m_ScreenX = screenX;
        cinemachineFramingTransposer.m_ScreenY = screenY;
    }


}
