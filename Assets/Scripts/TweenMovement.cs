using DG.Tweening;
using UnityEngine;

public static class TweenMovement
{

    public static void BlobTweenUpMovement(GameObject gameObject, Vector3 startPosition, Vector3 endPosition, float duration)
    {
        gameObject.transform.position = startPosition;
        gameObject.transform.DOMove(endPosition, duration).SetEase(Ease.InOutCirc);
    }

}
