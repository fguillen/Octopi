using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.P))
        {
            Time.timeScale += 0.1f;
            Debug.Log($"GameManager.timeScale: {Time.timeScale}");
        }

        if(Input.GetKey(KeyCode.O))
        {
            float timeScale = Time.timeScale;
            timeScale -= 0.1f;
            if(timeScale <= 0)
                timeScale = 0.1f;

            Time.timeScale = timeScale;

            Debug.Log($"GameManager.timeScale: {Time.timeScale}");
        }

    }
}
