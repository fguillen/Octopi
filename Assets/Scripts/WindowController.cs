using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WindowController : MonoBehaviour
{
    [SerializeField] GameObject windowBrokenPrefab;
    [SerializeField] Color burntColor = new Color(0.27f, 0.076f, 0.15f);
    [SerializeField] SpriteRenderer colorizable;
    [SerializeField] ParticleSystem particlesExplosion;
    [SerializeField] ParticleSystem particlesSmoke;

    public void StartGrab()
    {
    }

    public void StopGrab()
    {
    }

    public void Thrown()
    {
        Instantiate(windowBrokenPrefab, transform.position, Quaternion.identity, transform.parent);
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if(
            collisionInfo.gameObject.CompareTag("WindowGround")
        )
        {
            StartCoroutine(ExplodeCoroutine());
        }
    }

    IEnumerator ExplodeCoroutine()
    {
        Debug.Log("Car.Explode()");
        particlesExplosion.Play();
        particlesSmoke.Play();

        yield return colorizable.DOColor(burntColor, 0.5f).WaitForCompletion();
    }
}
