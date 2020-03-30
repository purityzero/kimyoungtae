using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

[RequireComponent(typeof(BoxCollider2D))]
public class MatchBlock : Block
{
    public static Action BlockAction;

    public MatchBlock()
    {

    }

    protected override void Start()
    {
        base.Start();
    }


    public override void Move(ThreeMatchDefine.movePos MoveDir)
    {
        base.Move(MoveDir);

        if (MoveDir == ThreeMatchDefine.movePos.None) return;

        if (MoveDir == ThreeMatchDefine.movePos.Right)
        {
            gameObject.transform.DOLocalMoveX(gameObject.transform.localPosition.x + ThreeMatchDefine.DISTANCE, ThreeMatchDefine.SPEED);
        }
        else if (MoveDir == ThreeMatchDefine.movePos.Left)
        {
            gameObject.transform.DOLocalMoveX(gameObject.transform.localPosition.x - ThreeMatchDefine.DISTANCE, ThreeMatchDefine.SPEED);
        }
        else if (MoveDir == ThreeMatchDefine.movePos.Up)
        {
            gameObject.transform.DOLocalMoveY(gameObject.transform.localPosition.y + ThreeMatchDefine.DISTANCE, ThreeMatchDefine.SPEED);
        }
        else
        {
            gameObject.transform.DOLocalMoveY(gameObject.transform.localPosition.y - ThreeMatchDefine.DISTANCE, ThreeMatchDefine.SPEED);
        }
    }
}
