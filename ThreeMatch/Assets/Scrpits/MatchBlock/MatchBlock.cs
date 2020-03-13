using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum blockType
{
    Common, Event
}

[RequireComponent(typeof(BoxCollider2D))]
public class MatchBlock : Block
{
    public blockType BlockType;

    void Start()
    {
    }

}
