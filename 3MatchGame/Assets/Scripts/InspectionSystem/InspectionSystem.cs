using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InspectionSystem : PasstiveSingleton<InspectionSystem>
{
    public List<MatchLine> MatchLines;
    public MatchObejct DragMatchObject;
    public MatchObejct CollisonObject;

    public Vector2 CollisionVector; // 애를 어찌 받아 오지?

    private void Start()
    {
        MatchLines = FindObjectsOfType<MatchLine>().ToList();
        DragMatchObject = null;
        StartCoroutine(coInspectionUpdate());
    }

    private IEnumerator coInspectionUpdate()
    {
        while (true)
        {
            yield return null; 
            if (DragMatchObject != null)
            {
                CollisonObject = MatchObjectManagement.instance.FindMatchObject(CollisionVector);
                yield return StartCoroutine(MoveSystem.instance.CollisionMove(CollisonObject, DragMatchObject.Position));
                yield return new WaitUntil(() => DragMatchObject.IsDrag == false);
                // 맞았을때 로직

                // 틀렸을때 로직
                if (DragMatchObject != null && CollisonObject != null)
                {
                    MoveSystem.instance.ReturnMatchObject(DragMatchObject, DragMatchObject.Position);
                    MoveSystem.instance.ReturnMatchObject(CollisonObject, CollisonObject.Position);
                    DragMatchObject = null;
                    CollisonObject = null;
                }
            }
        }
    }
}
