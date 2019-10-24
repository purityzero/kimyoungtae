using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ClickAbleObject : MonoBehaviour
{
    public System.Action<ClickAbleObject> ClickAction;
    public bool IsClick = false;
    public virtual void OnClick()
    {
        IsClick = true;
        ClickAction.Invoke(this);
    }

    private void OnMouseUp()
    {
       OnClick();
    }


    public void Show()
    {
        IsClick = false;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
