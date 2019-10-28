using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TdClickableObject : ClickAbleObject
{
    public override void OnClick()
    {
        if (!IsClick)
            base.OnClick();
    }
}
