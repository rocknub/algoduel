using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RotatorTween : MonoBehaviour
{
    [SerializeField] private float fullRotationInterval;
    [SerializeField] private Ease rotationEase;
    
    public void StartRotating()
    {
        transform.DOLocalRotate(new Vector3(0, 360, 0), fullRotationInterval, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental).SetEase(rotationEase);
    }
}
