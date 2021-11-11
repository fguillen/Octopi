using System.Collections;
using UnityEngine;

public class ElectricPoleController : MonoBehaviour
{
    [SerializeField] SpriteRenderer background;
    [SerializeField] Color flashBackgroundColor;
    [SerializeField] float flashFlipingScale;

    public bool electrocuting = false;

    void Start()
    {
        background.enabled = false;
    }

    public void FlashBackground(float time)
    {
        StartCoroutine(FlashBackgroundCoroutine(time));
    }

    IEnumerator FlashBackgroundCoroutine(float time)
    {
        float untilTime = Time.time + time;

        while(Time.time < untilTime)
        {
            float noiseValue = Mathf.PerlinNoise(Time.time * flashFlipingScale, 0.0f);

            if(noiseValue < 0.5f)
            {
                background.enabled = true;
                background.color = flashBackgroundColor;
            } else
            {
                background.enabled = false;
            }

            yield return new WaitForEndOfFrame();
        }

        background.enabled = false;
    }
}
