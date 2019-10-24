using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Todo
// init 함수를 지원하자. 설정이 없으면 Size값으로 Init 인자값을 받으면 X, y 값을 받는다
// Position은 숙제~~
public abstract class MiniGame : MonoBehaviour
{
    [HideInInspector]
    public bool IsPlaying;
    [Tooltip(" x * y의 값으로 ClickAble 오브젝트를 생성합니다.")]
    public Vector2Int Size;
    [Tooltip("Object간의 거리를 설정 합니다.")]
    public float SizeDistance;
    [Tooltip("ClickAble Object의 Base가 될 Prefab을 넣어주세요.")]
    public ClickAbleObject BaseClickable;
    public Dictionary<Vector2, ClickAbleObject> ClickableInfo = new Dictionary<Vector2, ClickAbleObject>();
    /// <summary>
    /// 게임이 끝날떄 결과를 보여주거나, 연출을 보여줍니다.
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerator coEndGame();
    /// <summary>
    /// 게임의 결과를 확인하는 함수를 생성합니다.
    /// </summary>
    /// <returns>true or false</returns>
    public abstract bool IsGameEnd();
    /// <summary>
    /// ClickAbleObject Component가 ClickAction을 Action으로 받아갑니다. 해당 오브젝트가 클릭될 시 할 행동을 함수에 적습니다.
    /// </summary>
    /// <param name="obj"> 해당 ClickAbleObject </param>
    public abstract void ClickAction(ClickAbleObject obj);

    protected virtual void Start()
    {
        ClickableObjPosSet();
        Debug.Log(ClickableInfo.Count);
    }

    private void ClickableObjPosSet()
    {
        int widthCnt = Size.x;
        int heightCnt = Size.y;

        for (int x = 0; x < widthCnt; x++)
        {
            for (int y = 0; y < heightCnt; y++)
            {
                var obj = Instantiate(BaseClickable, gameObject.transform);
                obj.transform.position = new Vector2(x + (x * SizeDistance), y + (y * SizeDistance));
                obj.gameObject.AddComponent<BoxCollider2D>();
                obj.ClickAction += ClickAction;
                ClickableInfo.Add(new Vector2(x + (x * SizeDistance), y + (y * SizeDistance)), obj);
            }
        }
    }

    public virtual void Play()
    {
        gameObject.SetActive(true);
        ClickAbleObjectAllActive(true);
        IsPlaying = true;
    }

    public virtual void End()
    {
        gameObject.SetActive(false);
        ClickAbleObjectAllActive(false);
        IsPlaying = false;
    }

    public void ClickAbleObjectAllActive(bool OnOff)
    {
        ClickableInfo.ToList().ForEach(x => x.Value.gameObject.SetActive(OnOff));
    }
}
