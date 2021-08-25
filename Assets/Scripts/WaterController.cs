using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WaterController : MonoBehaviour
{
    [SerializeField] float offset = 10;
    [SerializeField] float velocity = 1;
    float originalY;

    void Start()
    {
        originalY = transform.localPosition.y;
        Animate();
    }

    void Animate()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMoveY(originalY - Utils.AddNoise(offset), Utils.AddNoise(velocity)));
        sequence.Append(transform.DOLocalMoveY(originalY, Utils.AddNoise(0.5f * velocity)));
        sequence.SetLoops(-1, LoopType.Yoyo);
    }
}
