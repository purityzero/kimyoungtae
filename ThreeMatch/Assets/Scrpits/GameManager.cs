using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : PasstiveSingleton<GameManager>
{
    public GameObject BlockBackObject;
    private MatchBackFactory MatchFactory;

    public int FactoryCnt = 0;
    public int BlockHeight = 0;

    public List<MatchBlock> MatchBlockList;


    private void Awake()
    {
        MatchFactory = FindObjectOfType<MatchBackFactory>();
    }

    public void Start()
    {
        MatchFactory.MatchFactorySetting();
        MatchFactory.FactoryList.ForEach(x => x.FactorySetting());
    }
}
