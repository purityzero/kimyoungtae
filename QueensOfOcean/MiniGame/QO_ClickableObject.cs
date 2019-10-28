using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QO_ClickableObject : ClickAbleObject
{
    public override void OnClick()
    {
        if (IsClick)
            return;

        base.OnClick();
    }
}
