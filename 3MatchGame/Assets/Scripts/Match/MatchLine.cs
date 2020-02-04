using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchLine : PasstiveSingleton<MatchLine> {
    public List<MatchObejct> MatchObejctList;
    public int MatchObjectCount;
    public int MatchLineNumber;

    public void Start()
    {
        CreateMatchObject();
    }

    private void CreateMatchObject()
    {
        // 다음에 생성시 json이나 csv 파일로 대처한다.
        var matchList = MatchObjectManagement.instance.MatchObjectList;

        for (int i = 0; i < MatchObjectCount; i++)
        {
            var createObject =  Instantiate(matchList.RandomChoose(), transform);
            createObject.transform.localPosition = new Vector2(0, (float)i / 2);
            MatchObejctList.Add(createObject);
        }
        
    }

    private void Update()
    {
        if (MatchObejctList.Find(x => x.IsDrag == true))
        {
            InspectionSystem.instance.DragMatchObject = MatchObejctList.Find(x => x.IsDrag == true);
        }
    }
}
