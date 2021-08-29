using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowBrokenController : MonoBehaviour
{
    [SerializeField] List<Sprite> frames;
    [SerializeField] SpriteRenderer frameRenderer;
    [SerializeField] List<GameObject> cracks;

    [SerializeField] ParticleSystem particlesExplosion;
    [SerializeField] ParticleSystem particlesSmoke;

    [SerializeField] float smokeDurationTime = 20.0f;

    float originalRateOverTime;
    float startedAt;

    bool smokeDestroyed = false;

    void Awake()
    {
        frameRenderer.sprite = frames[Random.Range(0, frames.Count)];

        foreach (var crack in cracks)
        {
            int random = Random.Range(0, 2);
            // Debug.Log($"Random: {random}");
            crack.SetActive(random == 1);
        }

        originalRateOverTime = particlesSmoke.emission.rateOverTime.Evaluate(0);
        smokeDurationTime = Utils.AddNoise(smokeDurationTime);
        startedAt = Time.time;
    }

    void Start()
    {
        particlesExplosion.Play();
        particlesSmoke.Play();
    }

    void Update()
    {
        if(!smokeDestroyed)
        {
            if((Time.time - startedAt) > smokeDurationTime)
            {
                StopSmoke();
            } else
            {
                float timePassed = (Time.time - startedAt);
                float rateOverTime = Mathf.Lerp(originalRateOverTime, 0.0f, timePassed / smokeDurationTime);
                // Debug.Log($"originalRateOverTime: {originalRateOverTime}, smokeDurationTime: {smokeDurationTime}, TimePassed: {timePassed}, RateOverTime: {rateOverTime}");

                var emission = particlesSmoke.emission;
                emission.rateOverTime = 2.0f;
                emission.rateOverTime = rateOverTime;
            }
        }
    }

    void StopSmoke()
    {
        smokeDestroyed = true;
        particlesSmoke.Stop();
        Destroy(particlesSmoke, 20.0f);
        Destroy(particlesExplosion, 20.0f);
    }
}
