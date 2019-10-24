using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class MineSweeper : MiniGame
{
    public enum SelectMode
    {
        Normal, Sweeper, Find, NULL
    }

    private bool IsFirstTouch;

    public SelectMode selectMode = SelectMode.Normal;
    public int SetMaximumMineCount = 5;
    private int mineCount = 0;
    public Text MineText;
    public Text ShowCurrentMineCnt;
    public Text GameEndText;
    private List<Text> GeneratedTextList = new List<Text>();
    public Toggle SweeperToggle;
    public Toggle NormalToggle;
    public Toggle FindToggle;
    private Dictionary<Vector2, Text> SweeperText = new Dictionary<Vector2, Text>();
    public Text MineCountSetField;

    #region "Toggle"
    public void ToggleNormal()
    {
        if (NormalToggle.isOn)
        { 
            FindToggle.isOn = false;
            SweeperToggle.isOn = false;
            selectMode = SelectMode.Normal;
        }
        if (!NormalToggle.isOn && !SweeperToggle.isOn && !FindToggle.isOn)
        {
            selectMode = SelectMode.NULL;
        }
    }

    public void ToggleSweeper()
    {
        if (SweeperToggle.isOn)
        {
            FindToggle.isOn = false;
            NormalToggle.isOn = false;
            selectMode = SelectMode.Sweeper;
        }
        if (!NormalToggle.isOn && !SweeperToggle.isOn && !FindToggle.isOn)
        {
            selectMode = SelectMode.NULL;
        }
    }

    public void ToggleFind()
    {
        if (FindToggle.isOn)
        {
            SweeperToggle.isOn = false;
            NormalToggle.isOn = false;
            selectMode = SelectMode.Find;
        }
        if (!NormalToggle.isOn && !SweeperToggle.isOn && !FindToggle.isOn)
        {
            selectMode = SelectMode.NULL;
        }
    }

    protected override void Start()
    {
        base.Start();
        IsPlaying = true;
        End();
    }
    #endregion

    public override void Play()
    {
        if (!IsPlaying)
        {
            if (!string.IsNullOrEmpty(MineCountSetField.text)) {
                try
                {
                    SetMaximumMineCount = int.Parse(MineCountSetField.text);
                } catch {
                    SetMaximumMineCount = 5;
                    MineCountSetField.text = "5";
                }
            }
            IsFirstTouch = true;
            SetMine();
            GameEndText.text = "";
            MineTextUpdate();
            ClickAbleObjectAllActive(true);
            base.Play();
        }
    }

    public override void End()
    {
        if (IsPlaying)
        {
            mineCount = 0;
            MineTextUpdate();
            SweeperTextAllClear();
            GeneratedTextList.ForEach(x => Destroy(x.gameObject));
            GeneratedTextList.Clear();
            MineinfoClear();
            base.End();
        }
    }

    private void MineinfoClear()
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                var obj = ClickableInfo[new Vector2(x, y)] as MineSweeperObject;
                obj.IsMine = false;
                obj.IsSelect = false;
                obj.IsSweeper = false;
                obj.IsClick = false;
            }
        }
    }

    private void SetMine()
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                ClickableInfo[new Vector2(x, y)].GetComponent<MineSweeperObject>().IsMineSelect(IsMine());
            }
        }
    }

    private bool IsMine()
    {
        bool isMine;
        var mineRandom = Random.Range(0, 2);
        Debug.Log(mineRandom);
        if (mineRandom == 1 || SetMaximumMineCount == mineCount)
        {
            isMine = false;
        }
        else
        {
            isMine = true;
            mineCount += 1;
        }
        return isMine;
    }

    public override void ClickAction(ClickAbleObject obj)
    {
        if (selectMode == SelectMode.Normal)
        {
            if (IsFirstTouch) {
                IsFirstTouch = false;
                FirstTouch(obj);
            } else {
                NormalMode(obj);
            }
        }
        else if (selectMode == SelectMode.Sweeper)
        {
            SweeperMode(obj);
        }
        else if (selectMode == SelectMode.Find)
        {
            FindMode(obj);
        }
        else
        {
            obj.IsClick = false;
        }
    }

    private void FirstTouch(ClickAbleObject obj)
    {
        var transObj = obj as MineSweeperObject;
        if (!transObj.IsMine) {
            NormalMode(transObj);
        }
        else
        {
            SweeperMode(transObj);
        }
        AroundFindSafetyList(transObj).ForEach(x => NormalMode(x));
    }

    private void NormalMode(ClickAbleObject obj)
    {
        var transObj = obj as MineSweeperObject;
        if (!transObj.IsSelect)
        {
            if (transObj.IsMine)
            {
                Text textMineCnt = GenerateTextObject("Mine", transObj.transform.position);
                GeneratedTextList.Add(textMineCnt);
                StartCoroutine(coEndGame());
            }
            else
            {
                string mineCount = AroundFindMineList(transObj).Count.ToString();
                Text textMineCnt = GenerateTextObject(mineCount, transObj.transform.position);
                textMineCnt.color = GetRandomColor();
                GeneratedTextList.Add(textMineCnt);
                MineTextUpdate();
                if (IsGameEnd())
                {
                    StartCoroutine(coEndGame());
                }
            }
        }
        transObj.IsSelect = true;
    }

    private void SweeperTextAllClear()
    {
        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                var key = new Vector2(i, j);
                if (SweeperText.ContainsKey(key)) {
                    Destroy(SweeperText[key].gameObject);
                }
            }
        }
        SweeperText.Clear();
    }

    private void SweeperMode(ClickAbleObject obj)
    {
        var transObj = obj as MineSweeperObject;
        if (transObj.IsSweeper)
        {
            transObj.IsClick = false;
            transObj.IsSelect = false;
            transObj.IsSweeper = false;
            mineCount += 1;
            MineTextUpdate();
            var removeText = SweeperText.ToList().Find(x => x.Key == new Vector2(obj.transform.position.x, obj.transform.position.y)).Value;
            Destroy(removeText.gameObject);
            SweeperText.Remove(new Vector2(obj.transform.position.x, obj.transform.position.y));
        }
        else
        {
            transObj.IsClick = true;
            transObj.IsSelect = true;
            transObj.IsSweeper = true;
            mineCount -= 1;
            MineTextUpdate();
            Text sweeperText = GenerateTextObject("W", obj.transform.position);
            sweeperText.color = Color.gray;
            SweeperText.Add(obj.transform.position, sweeperText);
        }

    }

    private void FindMode(ClickAbleObject obj)
    {
        var transObj = obj as MineSweeperObject;
        if (transObj.IsSelect)
        {
            var mineSweeperFilterList = AroundFindMineList(transObj).FindAll(x => x == !x.IsSweeper);
            var safetySweeperFilterList = AroundFindSafetyList(transObj).FindAll(x => x.IsSweeper);

            if (safetySweeperFilterList.Count > 0)
            {
                StartCoroutine(coEndGame());
                return;
            }
            if (mineSweeperFilterList.Count == 0)
            {
                AroundFindSafetyList(transObj).ForEach(safeObj => NormalMode(safeObj));
                if (IsGameEnd())
                {
                    StartCoroutine(coEndGame());
                }
            }
        }
    }

    private Text GenerateTextObject(string str, Vector2 pos)
    {
        Text text = Instantiate(MineText, FindObjectOfType<Canvas>().transform);
        text.text = str;
        text.transform.position = pos;
        return text;
    }

    private Color GetRandomColor()
    {
        var color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        return color;
    }

    private List<MineSweeperObject> AroundFindSafetyList(MineSweeperObject mineObj)
    {
        List<MineSweeperObject> safetyList = new List<MineSweeperObject>();
        for (int i = (int)mineObj.transform.position.x - 1; i <= mineObj.transform.position.x + 1; i++)
        {
            for (int j = (int)mineObj.transform.position.y - 1; j <= mineObj.transform.position.y + 1; j++)
            {
                Vector2Int key = new Vector2Int(i, j);
                if (ClickableInfo.ContainsKey(key) || !mineObj)
                {
                    var clickableObj = ClickableInfo[key] as MineSweeperObject;
                    if (!clickableObj.IsMine)
                    {
                        safetyList.Add(clickableObj);
                    }
                }
            }
        }
        return safetyList;
    }

    private List<MineSweeperObject> AroundFindMineList(MineSweeperObject mineObj)
    {
        List<MineSweeperObject> mineList = new List<MineSweeperObject>();
        for (int i = (int)mineObj.transform.position.x - 1; i <= mineObj.transform.position.x + 1; i++)
        {
            for (int j = (int)mineObj.transform.position.y - 1; j <= mineObj.transform.position.y + 1; j++)
            {
                Vector2Int key = new Vector2Int(i, j);
                if (ClickableInfo.ContainsKey(key) || !mineObj)
                {
                    var clickableObj = ClickableInfo[key] as MineSweeperObject;
                    if (clickableObj.IsMine)
                    {
                        mineList.Add(clickableObj);
                    }
                }
            }
        }
        return mineList;
    }

    private void EndEffect()
    {
        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                var clickableObj = ClickableInfo[new Vector2(i, j)] as MineSweeperObject;
                if (clickableObj.IsMine)
                {
                    var text = GenerateTextObject("M", clickableObj.transform.position);
                    text.color = Color.red;
                    GeneratedTextList.Add(text);

                }
                else
                {
                    var text = GenerateTextObject("S", clickableObj.transform.position);
                    text.color = Color.blue;
                    GeneratedTextList.Add(text);
                }
            }
        }
    }

    public override IEnumerator coEndGame()
    {
        ClickAbleObjectAllActive(false);
        EndEffect();
        GameEndTextUpdate();
        yield return new WaitForSeconds(4.0f);
        End();
    }


    public override bool IsGameEnd()
    {
        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                var checkObj = ClickableInfo[new Vector2(i, j)] as MineSweeperObject;
                if (!checkObj.IsMine && checkObj.IsSweeper)
                {
                    return false;
                }
            }
        }

        return mineCount == 0;
    }

    private void MineTextUpdate()
    {
        ShowCurrentMineCnt.text = mineCount.ToString();
    }

    public void GameEndTextUpdate()
    {
        if (IsGameEnd())
        {
            GameEndText.text = "Clear!";
            GameEndText.color = GetRandomColor();
        }
        else
        {
            GameEndText.text = "Game Over";
            GameEndText.color = Color.red;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (NormalToggle.isOn) NormalToggle.isOn = false;
            else NormalToggle.isOn = true;
            ToggleNormal();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (SweeperToggle.isOn) SweeperToggle.isOn = false;
            else SweeperToggle.isOn = true;
            ToggleSweeper();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (FindToggle.isOn) FindToggle.isOn = false;
            else FindToggle.isOn = true;
            ToggleFind();
        }
    }
}
