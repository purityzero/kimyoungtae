using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : PasstiveSingleton<GameManager>
{
    public GameObject BlockBackObject;
    private MatchBackFactory MatchFactory => FindObjectOfType<MatchBackFactory>();  // 주소 값 참조 깊은 복사

    public int FactoryCnt = 0;
    public int BlockHeight = 0;
    public List<MatchBlock> MatchBlockList;


    public void Start()
    {
        MatchFactory.MatchFactorySetting();
        
    }
}
