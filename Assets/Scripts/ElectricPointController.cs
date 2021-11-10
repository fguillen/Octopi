using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningBolt;
using DG.Tweening;

public class ElectricPointController : MonoBehaviour
{
    [SerializeField] LightningBoltScript lightning;
    [SerializeField] float lightningDuration = 2.0f;
    [SerializeField] float lightningFrequency = 3.0f;
    Vector3 originalEndPosition;
    float nextLightningAt;
    bool lightningActive;


    void Awake()
    {
        originalEndPosition = lightning.EndObject.transform.localPosition;
        CalculateNextLightningAt();
        lightningActive = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        lightning.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextLightningAt && !lightningActive)
        {
            ShootLighting();
            CalculateNextLightningAt();
        }
    }

    void CalculateNextLightningAt()
    {
        nextLightningAt = Time.time + Utils.AddNoise(lightningFrequency);
    }

    void ShootLighting()
    {
        StartCoroutine(ShootLightningCoroutine());
    }

    IEnumerator ShootLightningCoroutine()
    {
        lightningActive = true;
        lightning.gameObject.SetActive(true);
        float duration = Utils.AddNoise(lightningDuration);
        lightning.EndObject.transform.DOShakePosition(duration, 0.5f, 1, 90);
        yield return new WaitForSeconds(duration);
        lightning.gameObject.SetActive(false);
        lightning.EndObject.transform.localPosition = originalEndPosition;
        lightningActive = false;
    }
}
