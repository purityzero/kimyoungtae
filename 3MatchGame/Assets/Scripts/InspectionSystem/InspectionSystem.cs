using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InspectionSystem : MonoBehaviour
{
    public List<MatchLine> MatchLines;
    public static MatchObejct DragMatchObject;
    public static MatchObejct CollisionObject;

    public static Vector2 CollisionVector; // 애를 어찌 받아 오지?

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
                CollisionObject = MatchObjectManagement.instance.FindMatchObject(CollisionVector);
                yield return StartCoroutine(MoveSystem.CollisionMove(CollisionObject, DragMatchObject.Position));
                yield return new WaitUntil(() => DragMatchObject.IsDrag == false);
                //// 맞았을때 로직
                //var tt = MatchCheckSystem.MatchCheck();
                //Debug.Log(tt.Count);

                //틀렸을때 로직
                if (DragMatchObject != null && CollisionObject != null)
                {
                    MoveSystem.ReturnMatchObject(DragMatchObject, DragMatchObject.Position);
                    MoveSystem.ReturnMatchObject(CollisionObject, CollisionObject.Position);
                    DragMatchObject = null;
                    CollisionObject = null;
                }
            }
        }
    }
}
