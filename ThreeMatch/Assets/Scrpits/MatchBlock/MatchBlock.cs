using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(BoxCollider2D))]
public class MatchBlock : Block
{

    protected override void Start()
    {
        base.Start();
    }


    protected override void Move(ThreeMatchDefine.movePos MoveDir)
    {
        base.Move(MoveDir);
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
            gameObject.transform.DOLocalMoveX(gameObject.transform.localPosition.y + ThreeMatchDefine.DISTANCE, ThreeMatchDefine.SPEED);
        }
        else
        {
            gameObject.transform.DOLocalMoveX(gameObject.transform.localPosition.y - ThreeMatchDefine.DISTANCE, ThreeMatchDefine.SPEED);
        }
    }

}
