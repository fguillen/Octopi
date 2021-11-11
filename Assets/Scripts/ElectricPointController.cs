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
    [SerializeField] ElectricPoleController electricPole;

    Vector3 originalEndPosition;
    float nextLightningAt;
    bool lightningActive;
    bool electrocutingActive;

    IEnumerator spawnLightningCoroutine;
    IEnumerator electrocutedPlayerCoroutine;
    Tween doShakeTween;


    void Awake()
    {
        originalEndPosition = lightning.EndObject.transform.localPosition;
        CalculateNextLightningAt();
        lightningActive = false;
        electrocutingActive = false;
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
            SpawnLighting();
            CalculateNextLightningAt();
        }
    }

    void CalculateNextLightningAt()
    {
        nextLightningAt = Time.time + Utils.AddNoise(lightningFrequency);
    }

    void SpawnLighting()
    {
        spawnLightningCoroutine = SpawnLightningCoroutine();
        StartCoroutine(spawnLightningCoroutine);
    }

    void ElectrocutePlayer(Vector2 position, PlayerController player)
    {
        StartCoroutine(ElectrocutedPlayerCoroutine(position, player));
    }

    IEnumerator SpawnLightningCoroutine()
    {
        lightningActive = true;
        lightning.gameObject.SetActive(true);
        float duration = Utils.AddNoise(lightningDuration);
        doShakeTween = lightning.EndObject.transform.DOShakePosition(duration, 0.5f, 1, 90);
        yield return new WaitForSeconds(duration);
        lightning.gameObject.SetActive(false);
        lightning.EndObject.transform.localPosition = originalEndPosition;
        lightningActive = false;
    }

    IEnumerator ElectrocutedPlayerCoroutine(Vector2 position, PlayerController player)
    {
        lightning.EndObject.transform.position = position;

        if(spawnLightningCoroutine != null)
            StopCoroutine(spawnLightningCoroutine);

        if(doShakeTween != null)
            doShakeTween.Kill();

        lightningActive = true;
        electrocutingActive = true;
        lightning.gameObject.SetActive(true);
        float duration = Utils.AddNoise(lightningDuration);
        electricPole.FlashBackground(lightningDuration);
        yield return new WaitForSeconds(duration);
        lightning.gameObject.SetActive(false);
        lightning.EndObject.transform.localPosition = originalEndPosition;
        lightningActive = false;
        electrocutingActive = false;
        player.HitByElectrocution(position);
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.gameObject.CompareTag("Player") && !electrocutingActive)
        {
            ContactPoint2D[] contacts = new ContactPoint2D[1];
            collisionInfo.GetContacts(contacts);
            Vector2 position = contacts[0].point;
            ElectrocutePlayer(position, collisionInfo.gameObject.GetComponent<PlayerController>());
        }
    }
}
