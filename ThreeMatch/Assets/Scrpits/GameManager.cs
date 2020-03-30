using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : PasstiveSingleton<GameManager>
{
    [Tooltip("해당신에서 Block 오브젝트 뒤에 있는 배경 Object 입니다.")]
    public ObjectBack BlockBackObject;
    private MatchBackFactory MatchFactory => FindObjectOfType<MatchBackFactory>();
    private BlockMoveSystem BlockMoveSystem => FindObjectOfType<BlockMoveSystem>();

    [Tooltip("Factory Cnt 를 x 축으로 배열 합니다.")]
    public int FactoryCnt = 0;
    [Tooltip("Factory 객체에서 Block Object 객체를 몇개 생성할 것인지 결정 합니다. 생성 방향은 y축 입니다.")]
    public int BlockHeight = 0;
    [Tooltip("해당 Scene에서 사용할 MatchBlock을 가지고 있습니다.")]
    public List<MatchBlock> MatchBlockList;


    public void Start()
    {
        MatchFactory.MatchFactorySetting();
    }

    public Factory FindFactory(int idx)
    {
        return MatchFactory.FindFactory(idx);
    }

    public Factory FindFactory(float vectorX)
    {
        return MatchFactory.FindFactory(vectorX);
    }

    public void MatchBlockMove(MatchBlock match, MatchBlock swap, ThreeMatchDefine.movePos movePos)
    {
        StartCoroutine(BlockMoveSystem.coBlockMoveSetting(match, swap, movePos));
    }

    public MatchBlock FindMatchObject(Vector2 pos)
    {

        int y = -1;
        int x = -1;

        for (int i = 0; i < BlockHeight; i++)
        {
            if (pos.y > -0.2f + (i * 0.4f) && pos.y < 0.2f + (i * 0.4f))
            {
               y = i;
                break;
            }
        }

        for (int i = 0; i < FactoryCnt; i++)
        {
            if (pos.x > -0.2f + (i * 0.4f) && pos.x < 0.2f + (i * 0.4f))
            {
                x = i;
                break;
            }
        }
        if (MatchFactory.GetMatchBlock(x, y) != null)
        {
            return MatchFactory.GetMatchBlock(x, y);
        }
        else return null;
    }


}
