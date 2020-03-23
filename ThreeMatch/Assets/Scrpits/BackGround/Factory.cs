using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    private GameObject ObjectBack => GameManager.instance.BlockBackObject;
    private int BlockCnt => GameManager.instance.BlockHeight;
    public float firstY;
    public List<GameObject> BlockBackList { get; private set; }

    private void Awake()
    {
        firstY = GetComponentInParent<MatchBackFactory>().FirstVector.y;
    }

    public void FactorySetting()
    {
        BlockBackList = new List<GameObject>();
        for (int i = 0; i < BlockCnt; i++)
        {
            var blockBackObject = Instantiate(ObjectBack, this.transform);
            blockBackObject.transform.position = new Vector2(this.transform.position.x, firstY + (0.4f * i));
            var block = Instantiate(GameManager.instance.MatchBlockList.RandomChoose());
            block.transform.parent = blockBackObject.transform;
            block.transform.position = blockBackObject.transform.position;
        }
    }

    
}
