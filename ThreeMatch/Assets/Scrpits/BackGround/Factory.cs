using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    private ObjectBack ObjectBack => GameManager.instance.BlockBackObject;
    private int BlockCnt => GameManager.instance.BlockHeight;
    public float firstY;
    public List<ObjectBack> BlockBackList { get; private set; } = new List<ObjectBack>();
    public List<MatchBlock> MatchBLockList { get; private set; } = new List<MatchBlock>();

    private void Awake()
    {
        firstY = GetComponentInParent<MatchBackFactory>().FirstVector.y;
    }

    public void FactorySetting()
    {
        for (int i = 0; i < BlockCnt; i++)
        {
            var blockBackObject = Instantiate(ObjectBack, this.transform);
            blockBackObject.transform.position = new Vector2(this.transform.position.x, firstY + (0.4f * i));
            var block = Instantiate(GameManager.instance.MatchBlockList.RandomChoose());
            block.transform.parent = blockBackObject.transform;
            block.transform.position = blockBackObject.transform.position;
            MatchBLockList.Add(block);
        }
    }

    public void MatchBLockRemove(MatchBlock matchBLock)
    {
        MatchBLockList.Remove(matchBLock);
    }

    public void MatchBLockAdd(MatchBlock matchBLock)
    {
        MatchBLockList.Add(matchBLock);
    }

    public MatchBlock FindMatchBlock(int idx)
    {
        return MatchBLockList[idx];
    }

    
}
