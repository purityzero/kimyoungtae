
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class MoveSystem : MonoBehaviour {

    public static void ReturnMatchObject(MatchObejct obj, Vector2 movePos)
    {
        obj.gameObject.transform.DOMove(movePos, 0.5f);
    }

    public static IEnumerator CollisionMove(MatchObejct obj, Vector2 movePos)
    {
        bool isComplete = false;
        obj.gameObject.transform.DOMove(movePos, 0.5f).OnComplete(() => {
            isComplete = true;
        });
        yield return new WaitUntil(() => isComplete);
    }

    public static IEnumerator coTweenMove(MatchObejct obj, Vector2 movePos)
    {
        bool isComplete = false;
        obj.gameObject.transform.DOMove(movePos, 0.5f).OnComplete(() => {
             isComplete = true;
        });
        yield return new WaitUntil(() => isComplete);
    }
}
