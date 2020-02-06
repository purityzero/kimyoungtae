
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class MoveSystem : PasstiveSingleton<MoveSystem> {

    public void ReturnMatchObject(MatchObejct obj, Vector2 movePos)
    {
        obj.gameObject.transform.DOMove(movePos, 0.5f);
    }

    public IEnumerator CollisionMove(MatchObejct obj, Vector2 movePos)
    {
        bool isComplete = false;
        Debug.Log("움직이자 부딫친 오브젝트야 " + obj + "이동위치" + movePos);
        obj.gameObject.transform.DOMove(movePos, 0.5f).OnComplete(() => {
            isComplete = true;
        });
        yield return new WaitUntil(() => isComplete);
    }

    public IEnumerator coTweenMove(MatchObejct obj, Vector2 movePos)
    {
        bool isComplete = false;
        obj.gameObject.transform.DOMove(movePos, 0.5f).OnComplete(() => {
             isComplete = true;
        });
        yield return new WaitUntil(() => isComplete);
    }
}
