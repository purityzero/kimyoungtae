using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BSS;
using System;
using UnityEngine.UI;
using System.Linq;
using Sirenix.OdinInspector;
using Spine.Unity;
using BSS.View;
using SlotGame;

public class ThreeDragonMatchGame : MatchGame
{
    public Canvas MatchGameCanvas;
    public GameObject ChooseYourBonusSpr;
    public TweenElement ChooseYourBonusTween;

    private ResultObj ResultObject;
    private List<GameObject> DummyList = new List<GameObject>();
    private Color DimColor = new Color(0.3f, 0.3f, 0.3f);
    private TdClickableObject ClickObj;

    protected override void Start()
    {
        base.Start();
    }

    public override void Play()
    {
        base.Play();
        ClickableInfo.ToList().ForEach(x => x.Value.IsClick = false);
        MatchGameCanvas.gameObject.SetActive(true);
        ChooseYourBonusSpr.gameObject.SetActive(true);
        ChooseYourBonusSpr.GetComponent<TweenPlayer>().Play(ChooseYourBonusTween);
    }

    public override void End()
    {
        base.End();
        DummyList.ForEach(x => Destroy(x));
        DummyList.Clear();
        MatchGameCanvas.gameObject.SetActive(false);
    }

    public override void ClickAction(ClickAbleObject obj)
    {
        if (ChooseYourBonusSpr.GetComponent<TweenPlayer>().isPlaying) {
            obj.IsClick = false;
            return;
        }
        base.ClickAction(obj);
        SoundSystem.PlayOnce(ThreeDragonUtility.TD_BONUS_SELECT);
        if (IsGameEnd())
        {
            ChooseYourBonusSpr.SetActive(false);
            ClickObj = obj as TdClickableObject;
            ClickableInfo.ToList().ForEach(x => x.Value.IsClick = true);
            StartCoroutine(coStartEndEffect());
        }
    }

    protected override ResultObj GenerateResultObjOfCurrentObj(Vector2 cilckIndex)
    {
        return base.GenerateResultObjOfCurrentObj(cilckIndex);
    }

    public override bool IsGameEnd()
    {
        return base.IsGameEnd();
    }

    public IEnumerator coStartEndEffect()
    {
        yield return new WaitForSeconds(1.0f);
        string createObjID = "";
        ClickableInfo.ToList().ForEach(x =>
        {
            if (x.Value.gameObject != ClickObj.gameObject)
            {
                x.Value.gameObject.SetActive(false);
                var serach = ResultObjInfoList.FindAll(obj => obj.ID != GetPrizeObjInfo().ID && createObjID != obj.ID).RandomPeek();
                createObjID = serach.ID;

                var gameObj = Instantiate(serach.pickObj);
                gameObj.GetComponent<SkeletonAnimation>().skeleton.SetColor(DimColor);
                gameObj.GetComponentInChildren<SpriteRenderer>().color = DimColor;
                gameObj.transform.position = x.Value.gameObject.transform.position;
                DummyList.Add(gameObj);
            }
        });

        yield return new WaitForSeconds(2f);
        StartCoroutine(coShowJackPotPopup());
    }

    public IEnumerator coShowJackPotPopup()
    {

        var getJackPotTitle = new NbCommon.JackpotTitle();
        foreach (NbCommon.JackpotTitle title in Enum.GetValues(typeof(NbCommon.JackpotTitle)))
        {
            if (title.ToString() == GetPrizeObjInfo().ID)
            {
                getJackPotTitle = title;
                break;
            }
        }
        var findPopupView = WinPopupView.Find(getJackPotTitle);
        SoundSystem.SetBgm(ThreeDragonUtility.TD_JACK_POT_BGM);
        SoundSystem.PlayOnce(ThreeDragonUtility.TD_JACK_POT_WIN_POPUP);
        findPopupView.Show();
        yield return new WaitUntil(() => !findPopupView.IsVisible);
        if (NbSlotBaseData.spinType == SpinType.Normal)
        {
            SoundSystem.SetBgm(ThreeDragonUtility.TD_MAIN_BGM);
        }
        else SoundSystem.SetBgm(ThreeDragonUtility.TD_FREE_GAME_BGM);
        End();
    }
}
