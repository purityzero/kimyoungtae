using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InspectionSystem : PasstiveSingleton<InspectionSystem>
{
    public List<MatchLine> MatchLines;
    public MatchObejct DragMatchObject;
    public MatchObejct CollisonObject;

    public Vector2 CollisionVector;

    private void Start()
    {
        MatchLines = FindObjectsOfType<MatchLine>().ToList();
        DragMatchObject = null;
    }

    public MatchObejct FindMatchObject(Vector2 pos)
    {
        for (int i = 0; i < MatchLines.Count; i++)
        {
            if (MatchLines[i].ExistMatchObject(pos))
            {
                CollisonObject = MatchLines[i].FindMatchObject(pos);
                return CollisonObject;
            }
        }

        Debug.LogError("그 위치로는 이동 할 수 없습니다.");
        return null;
    }

    private void Update()
    {
        if (DragMatchObject != null)
        {
            // 맞았을때 로직

            // 틀렸을때 로직
            if (DragMatchObject.IsDrag == false)
            {
                MoveSystem.instance.ReturnMatchObject(DragMatchObject, DragMatchObject.Position);
                MoveSystem.instance.ReturnMatchObject(CollisonObject, CollisonObject.Position);
                DragMatchObject = null;
                CollisonObject = null;
            }
        }
    }
}
