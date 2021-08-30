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
    }
}
