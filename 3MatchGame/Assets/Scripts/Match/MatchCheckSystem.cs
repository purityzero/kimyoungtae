using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// InspectionSystem에서 DragObject와 CollisionObject를 받아온다.
public class MatchCheckSystem : MonoBehaviour
{

    private static MatchObejct DragObject => InspectionSystem.DragMatchObject;
    private static MatchObejct CollisionObject => InspectionSystem.CollisionObject;

    public static List<MatchObejct> MatchCheck()
    {
        var dragObjName = DragObject.ObjectName;
        var matchListX = CheckX(DragObject.ObjectName, CollisionObject.Position);
        var matchListY = CheckY(DragObject.ObjectName, CollisionObject.Position);

        for (int i = 0; i < matchListY.Count; i++)
        {
            if (matchListX.Exists(x => x != matchListY[i]))
            {
                matchListX.Add(matchListY[i]);
            }
        }
        matchListX.ForEach(x => Debug.Log( "x에 들어있는 값들 : " + x.Position));
        Debug.Log("총 값 : " + matchListX.Count);
        return matchListX;
    }

    private static List<MatchObejct> CheckX(string objName, Vector2 pos)
    {
        List<MatchObejct> matchObejcts = new List<MatchObejct>();

        // - 값을 찾는다.
        int findX = (int)pos.x;
        while (true)
        {
            if (MatchObjectManagement.instance.ExistMatchObject(new Vector2(findX, pos.y)))
            {
                if (MatchObjectManagement.instance.FindMatchObject(new Vector2(findX, pos.y)).ObjectName == objName)
                {
                    matchObejcts.Add(MatchObjectManagement.instance.FindMatchObject(new Vector2(findX, pos.y)));
                    findX -= 1;
                }
                else
                {
                    break;
                }
            }
            else break;


        }
        // + 값을 찾는다.
        findX = (int)pos.x;
        while (MatchObjectManagement.instance.ExistMatchObject(new Vector2(findX, pos.y)))
        {
                if (MatchObjectManagement.instance.FindMatchObject(new Vector2(findX, pos.y)).ObjectName == objName)
                {
                    matchObejcts.Add(MatchObjectManagement.instance.FindMatchObject(new Vector2(findX, pos.y)));
                    findX += 1;
                }
                else
                {
                    break;
                }
        }

        return matchObejcts;
    }

    private static List<MatchObejct> CheckY(string objName, Vector2 pos)
    {
        List<MatchObejct> matchObejcts = new List<MatchObejct>();

        int findY = (int)pos.y;
        while (MatchObjectManagement.instance.ExistMatchObject(new Vector2(pos.x, findY)))
        {
            if (MatchObjectManagement.instance.FindMatchObject(new Vector2(pos.x, findY)).ObjectName == objName)
            {
                matchObejcts.Add(MatchObjectManagement.instance.FindMatchObject(new Vector2(pos.x, findY)));
                findY -= 1;
            }
            else
            {
                break;
            }
        }
        // + 값을 찾는다.
        findY = (int)pos.y;
        while (MatchObjectManagement.instance.ExistMatchObject(new Vector2(pos.x, findY)))
        {
            if (MatchObjectManagement.instance.FindMatchObject(new Vector2(pos.x, findY)).ObjectName == objName)
            {
                matchObejcts.Add(MatchObjectManagement.instance.FindMatchObject(new Vector2(pos.x, findY)));
                findY += 1;
            }
            else
            {
                break;
            }

        }
        return matchObejcts;
    }
}
