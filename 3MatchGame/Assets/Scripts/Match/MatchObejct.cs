using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


// Todo
// 1. MoveSystem에서 움직임 뺄 것
// 2. MatchManagement 클래스에서 움직임을 뺄 것인지, InspetionSystem에서 움직임을 뺄 것인지 고민
// 3. MatchObejct를 세분화 해서 더욱 다양한 MatchObject를 만들 것을 추후 고민
// 4. 기본 Pos, ObjectName(ID->int), Sprite는 뺄 것.
[RequireComponent(typeof(BoxCollider2D))]
[System.Serializable]
public class MatchObejct : MonoBehaviour
{
    public Vector2 Position;
    public string ObjectName;
    public Sprite Sprite;
    public bool IsDrag { get; private set; }
    private Vector2 StartTouchPoint;
    private Vector2 EndTouchPoint;

    private void Start()
    {
        IsDrag = false;
        Position = gameObject.transform.position;
    }

    private void OnMouseDown()
    {
        var canvasTouchPoint = transform.position - (Camera.main.ScreenPointToRay(Input.mousePosition).origin);
        StartTouchPoint = new Vector2(canvasTouchPoint.x, canvasTouchPoint.y);
    }

    private void OnMouseUp()
    {
        var canvasTouchPoint = transform.position - Camera.main.ScreenPointToRay(Input.mousePosition).origin;
        EndTouchPoint = new Vector2(canvasTouchPoint.x, canvasTouchPoint.y);
        ComparerVecterMove();
    }

    private void ComparerVecterMove()
    {
        IsDrag = true;
        bool iscomparerX = StartTouchPoint.x > EndTouchPoint.x;
        bool iscomparerY = StartTouchPoint.y > EndTouchPoint.y;

        var absX = Mathf.Abs(StartTouchPoint.x) + Mathf.Abs(EndTouchPoint.x);
        var absY = Mathf.Abs(StartTouchPoint.y) + Mathf.Abs(EndTouchPoint.y);
        Vector2 collisionPos = Vector2.zero;
        if (iscomparerX == true && iscomparerY == true) {
            if (absX > absY) {
                StartCoroutine(coMove(this, (collisionPos = new Vector2(transform.position.x + 0.5f, transform.position.y))));
            } else {
                StartCoroutine(coMove(this, (collisionPos = new Vector2(transform.position.x, transform.position.y+ 0.5f))));
            }
        }
        else if (iscomparerY == false && iscomparerX == false) {
            if (absX > absY) {
                StartCoroutine(coMove(this, (collisionPos = new Vector2(transform.position.x -0.5f, transform.position.y))));
            } else {
                StartCoroutine(coMove(this, (collisionPos = new Vector2(transform.position.x, transform.position.y - 0.5f))));
            }
        }
        else if (iscomparerX == true && iscomparerY == false) {
            if (absX > absY) {
                StartCoroutine(coMove(this, (collisionPos = new Vector2(transform.position.x + 0.5f, transform.position.y))));
            } else {
                StartCoroutine(coMove(this, (collisionPos = new Vector2(transform.position.x, transform.position.y - 0.5f))));
            }
        } else
        {
            if (absX > absY) {
                StartCoroutine(coMove(this, (collisionPos = new Vector2(transform.position.x - 0.5f, transform.position.y))));
            }
            else {
                StartCoroutine(coMove(this, (collisionPos = new Vector2(transform.position.x, transform.position.y + 0.5f))));
            }
        }
        InspectionSystem.CollisionVector = collisionPos;
    }


    private IEnumerator coMove(MatchObejct obj, Vector2 pos)
    {
        if (MatchObjectManagement.instance.ExistMatchObject(pos))
        yield return StartCoroutine(MoveSystem.coTweenMove(obj, pos));
        IsDrag = false;
    }
}
