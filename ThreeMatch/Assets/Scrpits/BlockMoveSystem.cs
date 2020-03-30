using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMoveSystem : MonoBehaviour
{
    public IEnumerator coBlockMoveSetting(MatchBlock matchBlock, MatchBlock swapObject, ThreeMatchDefine.movePos matchMovePos)
    {
        StartCoroutine(BlockSwapMove(matchBlock, swapObject, matchMovePos));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(SwapSetAgain(matchBlock, swapObject));
    }

    private IEnumerator BlockSwapMove(MatchBlock matchBlock, MatchBlock swapBlock, ThreeMatchDefine.movePos matchMovePos)
    {
        yield return null;
        if (matchMovePos == ThreeMatchDefine.movePos.Left)
        {
            matchBlock.Move(matchMovePos);
            swapBlock.Move(ThreeMatchDefine.movePos.Right);
        }
        else if (matchMovePos == ThreeMatchDefine.movePos.Right)
        {
            matchBlock.Move(matchMovePos);
            swapBlock.Move(ThreeMatchDefine.movePos.Left);

        }
        else if (matchMovePos == ThreeMatchDefine.movePos.Up)
        {
            matchBlock.Move(matchMovePos);
            swapBlock.Move(ThreeMatchDefine.movePos.Down);
        }
        else
        {
            matchBlock.Move(matchMovePos);
            swapBlock.Move(ThreeMatchDefine.movePos.Up);
        }
    }

    private IEnumerator SwapSetAgain(MatchBlock matchBlock, MatchBlock swapBlock)
    {
        yield return null;
        var emptyBLockParent = matchBlock.transform.parent;
        matchBlock.transform.parent = swapBlock.transform.parent;
        swapBlock.transform.parent = emptyBLockParent;

        matchBlock.BlockSetAgain();
        swapBlock.BlockSetAgain();
    }
}
