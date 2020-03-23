using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeMatchDefine : MonoBehaviour
{
    public enum movePos
    {
        None, Left, Right, Up, Down
    }


    public const float DISTANCE = 1.58f;
    public const float SPEED = 0.5f;

    // 블럭 0,0 왼쪽 하단 좌표 x.y 보정 값
    public const float BLOCK_COR_X = 1.7f;
    public const float BLOCK_COR_Y = 3.5f;
}
