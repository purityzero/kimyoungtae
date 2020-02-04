
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

    public void CollisionMove(MatchObejct obj, Vector2 movePos)
    {
        obj.gameObject.transform.DOMove(movePos, 0.5f);
    }
}
