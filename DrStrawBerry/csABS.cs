using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

public class csABS : MonoBehaviour
{
    int SceneNum;
    Text ABSFailText;


    void Awake()
    {
        SceneNum = Application.loadedLevel;
        ABSFailText = GameObject.Find("ABSFailText").GetComponent<Text>();

        if (!PlayerPrefs.HasKey("AdsLimit"))
        {
            PlayerPrefs.SetInt("AdsLimit", 0); //처음 들어왔을때 광고 데이터를 초기화 해준다.
        }

        if (!PlayerPrefs.HasKey("Day")) // 현재시간 데이터가 없을 경우에 
        {
            PlayerPrefs.SetInt("Day", DateTime.Now.Day); // 현재 시간을 받아온다.
        }
        else if (PlayerPrefs.GetInt("Day") != DateTime.Now.Day) // 지금 날짜 데이터가 현재 날짜 데이터와 다를 경우에
        {
            PlayerPrefs.SetInt("AdsLimit", 0); // 광고를 초기화 하고
            PlayerPrefs.SetInt("Day", DateTime.Now.Day); // 날짜 데이터를 최신으로 바꿔준다.

        }
    }
    // 버튼을 누르면
    public void ABSYes()
    {
#if UNITY_ADS
        if (PlayerPrefs.GetInt("AdsLimit") < 5) // 광고 5번 이하로 봤을 경우
        {
            if (Advertisement.IsReady())
            {
                ShowOptions options = new ShowOptions();
                options.resultCallback = HandleShowResult;
                Advertisement.Show(null, options); // 광고가 보여지고 Finished로 이어진다.
            }
        }
        else // 광고 5번 초과로 봤을 경우
        {
            Advertisement.IsReady("null");
            ShowOptions options = new ShowOptions();
            options.resultCallback = HandleShowResult;
            Advertisement.Show("Failed", options); // Failed로 강제로 이동한다.

        }
#endif
    }

#if UNITY_ADS
    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                PlayerPrefs.SetInt("AdsLimit", PlayerPrefs.GetInt("AdsLimit") + 1);
                PlayerPrefs.SetInt("hp", 6);
                PlayerPrefs.GetInt("hp");
                Time.timeScale = 1;
                Application.LoadLevel(SceneNum);
                break;

            case ShowResult.Skipped:
                ABSFailText.text = ("스킵하셔서 부활을 할 수가 없어요.");
                StartCoroutine("Destroy1");
                break;

            case ShowResult.Failed:
                ABSFailText.text = ("오늘은 더 이상 광고를 보실 수 없습니다.");
                StartCoroutine("Destroy1");
                break;
        }
    }

    IEnumerator Destroy1()
    {
        float desTime = 0.0f;

        while (desTime < 6.0f)
        {
            desTime += Time.unscaledDeltaTime;
            yield return null;
        }

        ABSFailText.text = ("");
    }
#endif
}