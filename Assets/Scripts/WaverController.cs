using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WaverController : MonoBehaviour
{
    [SerializeField] Transform waveable;
    [SerializeField] float offset = 1;
    [SerializeField] float velocity = 1;
    float originalY;

    void Start()
    {
        originalY = waveable.localPosition.y;
        waveable.localPosition = new Vector3(waveable.localPosition.x, originalY + offset, waveable.localPosition.z);
        Animate();
    }

    void Animate()
    {
        float offset = Utils.AddNoise(this.offset);
        float velocity = Utils.AddNoise(this.velocity);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(waveable.DOLocalMoveY(originalY - offset, velocity).SetEase(Ease.InOutCirc));
        sequence.Append(waveable.DOLocalMoveY(originalY + offset, velocity).SetEase(Ease.InOutCirc));
        sequence.SetUpdate(true);
        sequence.OnComplete(Animate);
    }
}
