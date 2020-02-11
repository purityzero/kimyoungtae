using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchObjectManagement : PasstiveSingleton<MatchObjectManagement> {

    public List<MatchObejct> MatchObjectList;
    public List<MatchLine> MatchLineList;

    public void Start()
    {
        MatchLineList = FindObjectsOfType<MatchLine>().OrderBy(x => x.MatchLineNumber).ToList();
    }

    public MatchObejct FindMatchObject(Vector2 pos)
    {
        for (int i = 0; i < MatchLineList.Count; i++)
        {
            if (MatchLineList[i].ExistMatchObject(pos))
            {
                var obj = MatchLineList[i].FindMatchObject(pos);
                return obj;
            }
        }
        Debug.LogError("오브젝트를 찾지 못하였습니다.");
        return null;
    }

    public bool ExistMatchObject(Vector2 pos)
    {
        for (int i = 0; i < MatchLineList.Count; i++)
        {
            if (MatchLineList[i].ExistMatchObject(pos))
            {
                return true;
            }
        }

        return false;
    }
}
