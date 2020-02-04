﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
        IsDrag = true;
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
        bool iscomparerX = StartTouchPoint.x > EndTouchPoint.x;
        bool iscomparerY = StartTouchPoint.y > EndTouchPoint.y;

        var absX = Mathf.Abs(StartTouchPoint.x) + Mathf.Abs(EndTouchPoint.x);
        var absY = Mathf.Abs(StartTouchPoint.y) + Mathf.Abs(EndTouchPoint.y);

        Vector2 collisionPos = Vector2.zero;

        if (iscomparerX == true && iscomparerY == true) {
            if (absX > absY) {
                collisionPos = new Vector2(transform.position.x + 0.5f, transform.position.y);
                TweenMove(collisionPos);
            } else {
                collisionPos = new Vector2(transform.position.x, transform.position.y + 0.5f);
                TweenMove(collisionPos);
            }
        }
        else if (iscomparerY == false && iscomparerX == false) {
            if (absX > absY) {
                collisionPos = new Vector2(transform.position.x - 0.5f, transform.position.y);
                TweenMove(collisionPos);
            } else {
                collisionPos = new Vector2(transform.position.x, transform.position.y - 0.5f);
                TweenMove(collisionPos);
            }
        }
        else if (iscomparerX == true && iscomparerY == false) {
            if (absX > absY) {
                collisionPos = new Vector2(transform.position.x + 0.5f, transform.position.y);
                TweenMove(collisionPos);
            } else {
                collisionPos = new Vector2(transform.position.x, transform.position.y - 0.5f);
                TweenMove(collisionPos);
            }
        } else
        {
            if (absX > absY) {
                collisionPos = new Vector2(transform.position.x - 0.5f, transform.position.y);
                TweenMove(collisionPos);
            }
            else {
                collisionPos = new Vector2(transform.position.x, transform.position.y + 0.5f);
                TweenMove(collisionPos);
            }
        }

        // 이동하는 곳에 있는 물체도 움직여준다. Match에서 말고 MoveSystem에서 해서 교차 검증 하자.
        var collisionMatchObject = InspectionSystem.instance.FindMatchObject(collisionPos);
        MoveSystem.instance.CollisionMove(collisionMatchObject, Position);
        StartCoroutine(coWaitDragEnd());
    }

    private void TweenMove(Vector2 movePos)
    {
        gameObject.transform.DOMove(movePos, 0.5f).OnComplete(() =>
        {
            IsDrag = false;
        });
    }

    private IEnumerator coWaitDragEnd()
    {
        yield return new WaitUntil(() => IsDrag == false);
    }
}
