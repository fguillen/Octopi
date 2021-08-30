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

    bool onWall = true;

    public void StartGrab()
    {
    }

    public void StopGrab()
    {
    }

    public void Thrown()
    {
        if(onWall)
        {
            Instantiate(windowBrokenPrefab, transform.position, Quaternion.identity, transform.parent);
            onWall = false;
        }{
            particlesExplosion.Play();
        }
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
        // Debug.Log("Car.Explode()");
        particlesExplosion.Play();
        particlesSmoke.Play();

        gameObject.layer = LayerMask.NameToLayer("WindowsHidden");

        yield return colorizable.DOColor(burntColor, 0.5f).WaitForCompletion();
    }
}
