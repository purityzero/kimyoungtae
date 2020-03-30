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

    protected virtual void Start()
    {
        if (!this.GetComponent<BoxCollider2D>())
        {
            gameObject.AddComponent<BoxCollider2D>().size = new Vector2(1.5f, 1.5f);
        }
        StartPos = transform.parent.transform.localPosition;

        if (ID % 1000 != 0)
        {
            BlockType = blockType.Event;
        }
        else
        {
            BlockType = blockType.Common;
        }
    }

    public void BlockSetAgain()
    {
        StartPos = transform.parent.transform.localPosition;
    }

    public virtual void Move(ThreeMatchDefine.movePos MoveDir) { }

}
