using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Block : MonoBehaviour
{
    public int ID;

    private void Start()
    {
        if (!this.GetComponent<BoxCollider2D>())
        {
            gameObject.AddComponent<BoxCollider2D>().size = new Vector2(1.5f, 1.5f);
        }
    }

    public virtual void Move() { }
    public virtual void Click() { }
    public virtual void Drag() { }

}
