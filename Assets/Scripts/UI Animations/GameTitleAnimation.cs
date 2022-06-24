using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTitleAnimation : MonoBehaviour
{
    void Start()
    {
        var mySequence = DOTween.Sequence();
        mySequence.Append(transform.DORotate(new Vector3(0, 0, 10), 3));
        mySequence.Join(transform.DOScale(new Vector3(0.8f, 0.8f, 1), 3));
        mySequence.SetLoops(-1, LoopType.Yoyo);
    }
}
