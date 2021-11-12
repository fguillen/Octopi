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
    [SerializeField] AudioClip clipElectroLow;
    [SerializeField] AudioClip clipElectroHigh;
    [SerializeField] AudioClip clipElectroOut;
    AudioSource audioSource;

    Vector3 originalEndPosition;
    float nextLightningAt;
    bool lightningActive;

    IEnumerator spawnLightningCoroutine;
    IEnumerator electrocutedPlayerCoroutine;
    Tween doShakeTween;


    void Awake()
    {
        originalEndPosition = lightning.EndObject.transform.localPosition;
        CalculateNextLightningAt();
        lightningActive = false;
        audioSource = GetComponent<AudioSource>();

        if(audioSource == null)
            throw new System.Exception("AudioSource Required");
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

        // TODO Sound - INI: There is a pseudo 3D implementation here because I don't know how the new sound system works with 3D
        audioSource.clip = clipElectroLow;
        float maxDistanceSound = 5f;
        float distanceToPlayer = Vector2.Distance(PlayerController.instance.transform.position, transform.position);
        float distanceNormalized = distanceToPlayer / maxDistanceSound;
        float volume = Mathf.Lerp(1, 0, distanceNormalized);
        // Debug.Log($"SpawnLightningCoroutine sound: {distanceToPlayer}, {distanceNormalized}, {volume}");
        audioSource.volume = volume;
        audioSource.Play();
        // TODO Sound - END

        lightning.gameObject.SetActive(true);
        float duration = Utils.AddNoise(lightningDuration);
        doShakeTween = lightning.EndObject.transform.DOShakePosition(duration, 0.5f, 1, 90);
        yield return new WaitForSeconds(duration);
        lightning.gameObject.SetActive(false);
        lightning.EndObject.transform.localPosition = originalEndPosition;

        // TODO Sound - INI
        audioSource.Stop();
        // TODO Sound - END

        lightningActive = false;
    }

    IEnumerator ElectrocutedPlayerCoroutine(Vector2 position, PlayerController player)
    {
        Debug.Log("ElectrocutedPlayerCoroutine() :: INI");
        lightningActive = true;
        electricPole.electrocuting = true;

        player.SetControlsActive(false);


        lightning.EndObject.transform.position = position;

        if(spawnLightningCoroutine != null)
            StopCoroutine(spawnLightningCoroutine);

        if(doShakeTween != null)
            doShakeTween.Kill();

        // TODO Sound - INI
        audioSource.Stop(); // in case it is playing the other sound
        audioSource.clip = clipElectroHigh;
        audioSource.volume = 1f;
        audioSource.Play();
        // TODO Sound - END

        lightning.gameObject.SetActive(true);
        float duration = Utils.AddNoise(lightningDuration);
        player.FlashBodyBackground(duration);
        electricPole.FlashBackground(lightningDuration);
        yield return new WaitForSeconds(duration);
        lightning.gameObject.SetActive(false);
        lightning.EndObject.transform.localPosition = originalEndPosition;

        player.SetControlsActive(true);
        player.HitByElectrocution(position);

        // TODO Sound - INI
        audioSource.Stop();
        audioSource.PlayOneShot(clipElectroOut);
        // TODO Sound - END

        lightningActive = false;
        electricPole.electrocuting = false;

        Debug.Log("ElectrocutedPlayerCoroutine() :: END");
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(collisionInfo.gameObject.CompareTag("Player") && !electricPole.electrocuting)
        {
            ContactPoint2D[] contacts = new ContactPoint2D[1];
            collisionInfo.GetContacts(contacts);
            Vector2 position = contacts[0].point;
            ElectrocutePlayer(position, collisionInfo.gameObject.GetComponent<PlayerController>());
        }
    }
}
