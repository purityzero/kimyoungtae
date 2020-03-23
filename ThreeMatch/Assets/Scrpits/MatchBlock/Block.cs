using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum blockType
{
    Common, Event
}


public class Block : MonoBehaviour
{
    public int ID;
    public Vector2 StartPos;
    public Vector2 EndPos;

    public blockType BlockType;
    public ThreeMatchDefine.movePos MovePos;

    protected virtual void Start()
    {
        if (!this.GetComponent<BoxCollider2D>())
        {
            gameObject.AddComponent<BoxCollider2D>().size = new Vector2(1.5f, 1.5f);
        }
        StartPos = transform.parent.transform.localPosition;

        MovePos = ThreeMatchDefine.movePos.None;
        if (ID % 1000 != 0)
        {
            BlockType = blockType.Event;
        }
        else
        {
            BlockType = blockType.Common;
        }
    }

    protected virtual void Move(ThreeMatchDefine.movePos MoveDir) { }


    private void OnMouseUp()
    {
        // 이거 바뀌어서..ㅎㅎㅎ..ㅎ.ㅎ.ㅎ.ㅎ.ㅎ 안씁니다.
        EndPos = (transform.position - Camera.main.ScreenPointToRay(Input.mousePosition).origin);
        // 나중에 Round 말고 0.# 으로 바꿔보는건? Boxing 이루어지는건 좀..
        EndPos = new Vector2((float)Math.Round(EndPos.x, 1), (float)Math.Round(EndPos.y, 1));
        if (BlockType == blockType.Common) {
        }
    }

}
