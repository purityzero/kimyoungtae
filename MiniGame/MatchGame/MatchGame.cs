using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

// 클래스명에 매니저 빼기
public class MatchGame : MiniGame
{

    public enum PickGameMode
    {
        Normal, DoubleLuck
    }

    [System.Serializable]
    public class ResultObj
    {
        public GameObject showObject;
        public Vector2 index;
        public ResultObjInfo resultObjInfo;
    }

    [System.Serializable]
    public class ResultObjInfo
    {
        public string ID;
        public Sprite Image;
    }

    [System.Serializable]
    public class PrizeInfo
    {
        public bool IsDoubleLuck;
        public string ID;
    }

    public Dictionary<Vector2Int, ResultObj> GeneratedObjectInfo = new Dictionary<Vector2Int, ResultObj>();
    [SerializeField]
    private PrizeInfo PrizeObj = new PrizeInfo();

    public PickGameMode pickGameMode;
    public List<ResultObjInfo> ResultObjInfoList = new List<ResultObjInfo>();
    public ResultObjInfo DoubleLuckObj;
    public int MatchCount = 0;
    [Tooltip("게임이 종료되었을 때 대기 시간을 설정 합니다.           ex) 종료시점까지 연출 나타냅니다.")]
    public float Endtime = 1f;

    protected override void Start()
    {
        base.Start();
        End();
    }

    public override void Play()
    {
        SetPrizeObj();
        base.Play();
    }

    public override void End()
    {
        GeneratedObjectInfo.ToList().ForEach(x => Destroy(x.Value.showObject));
        GeneratedObjectInfo.Clear();
        base.End();
    }

    public override IEnumerator coEndGame()
    {
        yield return new WaitForSeconds(Endtime);
        End();
    }

    public override void ClickAction(ClickAbleObject obj)
    {
        // Action
        obj.Hide();
        GeneratedObjectInfo.Add(new Vector2Int((int)obj.transform.position.x, (int)obj.transform.position.y), GenerateResultObjOfCurrentObj(obj.transform.position));
        if (IsGameEnd())
        {
            StartCoroutine(coEndGame());
        }
    }

    private void SetPrizeObj()
    {
        int prizeRandom = Random.Range(0, ResultObjInfoList.Count);
        PrizeObj.ID = ResultObjInfoList[prizeRandom].ID;
        if (pickGameMode == PickGameMode.DoubleLuck)
        {
            int DoubleLuckRandom = Random.Range(0, 2);
            if (DoubleLuckRandom == 0)
            {
                PrizeObj.IsDoubleLuck = true;
            }
            else PrizeObj.IsDoubleLuck = false;
        }
        else PrizeObj.IsDoubleLuck = false;
    }

    // Pick 했을 시 Object 생성
    private ResultObj GenerateResultObjOfCurrentObj(Vector2 cilckIndex)
    {
        ResultObj resultObj = new ResultObj();
        GameObject showObj = new GameObject();

        ResultObjInfo resultObjinfo;
        while (true)
        {
            resultObjinfo = ResultObjInfoList[Random.Range(0, ResultObjInfoList.Count)];
            Debug.Log(resultObjinfo.ID); 
            bool isLastCount = FindShowObjAll(resultObjinfo.ID).Count > MatchCount - 2;      // 생성되기 전에 계산하기 때문
            bool isPrizeID = resultObjinfo.ID == GetPrizeObjInfo().ID;

            if (isLastCount && !isPrizeID)
            {
                continue;
            }
            break;
        }

        if (PrizeObj.IsDoubleLuck)
        {
            // 생성된걸 게산하기 때문
            if (FindShowObjAll(GetPrizeObjInfo().ID).Count == MatchCount - 1)
            {
                if (FindShowObjAll(DoubleLuckObj.ID).Count < MatchCount)
                {
                    resultObjinfo = DoubleLuckObj;
                }
            }
        }

        showObj.AddComponent<SpriteRenderer>().sprite = resultObjinfo.Image;
        showObj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        showObj.transform.position = cilckIndex;

        resultObj.resultObjInfo = resultObjinfo;
        resultObj.showObject = showObj;
        resultObj.index = cilckIndex;

        return resultObj;
    }

    public override bool IsGameEnd()
    {
        return GeneratedObjectInfo.ToList().FindAll(x => x.Value.resultObjInfo.ID == PrizeObj.ID).Count == MatchCount;
    }
    public List<ResultObj> FindShowObjAll(string ID)
    {
        var findResultObjectList = new List<ResultObj>();
        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                Vector2Int key = new Vector2Int(i, j);
                if (GeneratedObjectInfo.ContainsKey(key) && GeneratedObjectInfo[key].resultObjInfo.ID == ID) {
                    findResultObjectList.Add(GeneratedObjectInfo[key]);
                }
            }
        }
        return findResultObjectList;
    }

    public Vector2 GetShowObjPos(Vector2 index)
    {
        return GeneratedObjectInfo.ToList().Find(x => x.Value.index == index).Value.showObject.transform.position;
    }

    public GameObject GetShowObj(Vector2 index)
    {
        return GeneratedObjectInfo.ToList().Find(x => x.Value.index == index).Value.showObject;
    }

    public ResultObjInfo GetPrizeObjInfo()
    {
        ResultObjInfo resultObj = ResultObjInfoList.Find(x => x.ID == PrizeObj.ID);
        return resultObj;
    }
}
