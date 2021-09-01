using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCore.Audio;

public class WindowBrokenController : MonoBehaviour
{
    [SerializeField] List<Sprite> frames;
    [SerializeField] SpriteRenderer frameRenderer;
    [SerializeField] List<GameObject> cracks;

    [SerializeField] ParticleSystem particlesExplosion;
    [SerializeField] ParticleSystem particlesSmoke;
    [SerializeField] ParticleSystem particlesFire;

    [SerializeField][Range(0, 1)] float probabilityFire = 0.1f;

    void Awake()
    {
        frameRenderer.sprite = frames[Random.Range(0, frames.Count)];

        foreach (var crack in cracks)
        {
            int random = Random.Range(0, 2);
            // Debug.Log($"Random: {random}");
            crack.SetActive(random == 1);
        }
    }

    void Start()
    {
        particlesExplosion.Play();
        particlesSmoke.Play();

        if(Random.Range(0.0f, 1.0f) < probabilityFire)
        {
            particlesFire.Play();
            StartCoroutine(SoundFireCoroutine());
        }
    }

    IEnumerator SoundFireCoroutine()
    {
        AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.SFX_fire, false);

        yield return new WaitForSeconds(Utils.AddNoise(5f));

        AudioController.instance.StopAudio(UnityCore.Audio.AudioType.SFX_fire, true);
    }
}
