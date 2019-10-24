using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSweeperObject : ClickAbleObject
{
    public bool IsMine;
    public bool IsSelect;
    public bool IsSweeper;

    public void IsMineSelect(bool isMine)
    {
        IsMine = isMine;
    }

    public override void OnClick()
    {
        base.OnClick();
    }
}
