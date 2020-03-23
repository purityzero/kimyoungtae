using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPointSystem : PasstiveSingleton<TouchPointSystem>
{
    private Camera Camera;
    private Ray Ray;
    private Vector2 Pos = Vector2.zero;
    public bool IsTouch { get; private set; }
    private Vector2 TouchDir = new Vector2();

    void Start()
    {
        Camera = FindObjectOfType<Camera>();
        IsTouch = false;
    }

    private void OnMouseDown()
    {
        IsTouch = true;
        if (Input.touchCount > 0)
        {
            Pos = Input.GetTouch(0).position;
            Ray = Camera.ScreenPointToRay(Pos);
            Pos = new Vector2(Ray.origin.x + ThreeMatchDefine.BLOCK_COR_X, Ray.origin.y + ThreeMatchDefine.BLOCK_COR_Y);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Pos = Input.mousePosition;
            Ray = Camera.ScreenPointToRay(Pos);
            Pos = new Vector2(Ray.origin.x + ThreeMatchDefine.BLOCK_COR_X, Ray.origin.y + ThreeMatchDefine.BLOCK_COR_Y);
        }
    }

    private void OnMouseUp()
    {
        IsTouch = false;
        Vector2 EndPos = Input.mousePosition;
        var endRay = Camera.ScreenPointToRay(EndPos);
        TouchDir = -(Pos - new Vector2(endRay.origin.x + ThreeMatchDefine.BLOCK_COR_X, endRay.origin.y + ThreeMatchDefine.BLOCK_COR_Y));

        float absX = Mathf.Abs(TouchDir.x);
        float absY = Mathf.Abs(TouchDir.y);

        if (absX > absY)
        {
            if (TouchDir.x > 0)
            {
                // Right
            }
            else
            {
                // Left
            }
        }
        else
        {
            if (TouchDir.y > 0)
            {
                // Up
            }
            else
            {
                // Down
            }
        }
    }
}

