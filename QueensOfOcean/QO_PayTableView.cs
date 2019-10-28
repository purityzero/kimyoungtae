using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BSS.View;

public class QO_PayTableView : BaseView
{
    public List<GameObject> ShowList;
    private int Index;

    public Button PreButton;
    public Button NextButton;
    public Button CloseButton;

    void Start()
    {
        CloseButton?.onClick.AddListener(Close);
        PreButton?.onClick.AddListener(PreviousView);
        NextButton?.onClick.AddListener(NextView);
    }

    public override void Show()
    {
        base.Show();
        Index = 0;
        ShowList[Index].SetActive(true);
    }

    public override void Close()
    {
        base.Close();
        ShowList.ForEach(x => x.SetActive(false));
    }

    public void NextView()
    {
        if (Index > ShowList.Count - 2) return;
        ShowList[Index].SetActive(false);
        Index += 1;
        ShowList[Index].SetActive(true);
    }

    public void PreviousView()
    {
        if (Index == 0) return;
        ShowList[Index].SetActive(false);
        Index -= 1;
        ShowList[Index].SetActive(true);
    }
}
