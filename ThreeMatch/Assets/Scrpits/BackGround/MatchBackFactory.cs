using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchBackFactory : MonoBehaviour
{
    public int FactoryCnt => GameManager.instance.FactoryCnt;
    public Vector2 FirstVector;
    public Vector2 Interval;
    public Factory FactoryObject;
    public List<Factory> FactoryList { get; private set; }

    public void MatchFactorySetting()
    {
        FactoryList = new List<Factory>();
        for (int i = 0; i < FactoryCnt; i++)
        {
            var factoryObject = Instantiate(FactoryObject, this.transform);
            factoryObject.transform.position = new Vector2(FirstVector.x + (i * Interval.x), FirstVector.y + (i * Interval.y));
            FactoryList.Add(factoryObject);
        }
    }
}
