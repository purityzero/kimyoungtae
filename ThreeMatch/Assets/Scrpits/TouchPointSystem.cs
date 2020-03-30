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

        // Match Object를 찾는다.
        var blockObject = GameManager.instance.FindMatchObject(Pos);
        // swapObject 얕은 복사로 해올 거 셋팅
        MatchBlock swapObejct;
        // 이동 방향 셋팅 준비
        ThreeMatchDefine.movePos matchMovePos = ThreeMatchDefine.movePos.None;

        if (blockObject == null) return;

        if (absX > absY)
        {
            if (TouchDir.x > 0)
            {
                swapObejct = GameManager.instance.FindMatchObject(new Vector2(Pos.x +0.4f, Pos.y));
                matchMovePos = ThreeMatchDefine.movePos.Right;
            }
            else
            {
                swapObejct = GameManager.instance.FindMatchObject(new Vector2(Pos.x -0.4f, Pos.y));
                matchMovePos = ThreeMatchDefine.movePos.Left;
            }
        }
        else
        {
            if (TouchDir.y > 0)
            {
                swapObejct = GameManager.instance.FindMatchObject(new Vector2(Pos.x, Pos.y+0.4f));
                matchMovePos = ThreeMatchDefine.movePos.Up;
            }
            else
            {
                swapObejct = GameManager.instance.FindMatchObject(new Vector2(Pos.x, Pos.y -0.4f));
                matchMovePos = ThreeMatchDefine.movePos.Down;
            }
        }
        if (swapObejct == null) return;
        GameManager.instance.MatchBlockMove(blockObject, swapObejct, matchMovePos);
    }


    // 밑의 두개는 Block이 가지고 있어야 하는 것입니다

}

