using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BSS;
using BSS.View;
using System.Linq;
using System;

public class QO_MatchGameManager : MatchGame
{
    public Canvas MatchGameCanvas;
    public QO_MatchGameCover MatchCover;

    protected override void Start()
    {
        base.Start();
        ClickableObjectSizeY();
    }

    public override void Play()
    {
        base.Play();
        ClickableInfo.ToList().ForEach(x => x.Value.IsClick = false);
        MatchGameCanvas.gameObject.SetActive(true);
        SoundSystem.SetBgm(QO_Sound.GetSound(QO_Define.QO_JACKPOT_BOUNS_BGM));
        SoundSystem.PlayOnce(QO_Sound.GetSound(QO_Define.QO_JACKPOT_BOUNS_INTRO));
        MatchCover.Play();
    }

    public override void End()
    {
        base.End();
        MatchGameCanvas.gameObject.SetActive(false);
    }

    private void ClickableObjectSizeY()
    {
        ClickableInfo.ToList().ForEach(obj =>
        {
            if (obj.Value.transform.position.y != FirstCreatePoint.y)
            {
                float correctionValue = (obj.Value.transform.position.y/FirstCreatePoint.y);
            }
        });
    }

    public override void ClickAction(ClickAbleObject obj)
    {
        if (MatchCover.MatchCover.activeSelf)
        {
            obj.IsClick = false;
            return;
        }
        base.ClickAction(obj);

        SoundSystem.PlayOnce(QO_Sound.GetSound(QO_Define.QO_JACKPOT_BOUNS_PICK));
        if (IsGameEnd())
        {
            ClickableInfo.ToList().ForEach(x => x.Value.IsClick = true);
            StartCoroutine(coShowJackPotPopup());
        }
    }

    public override bool IsGameEnd()
    {
        return base.IsGameEnd();
    }

    public IEnumerator coShowJackPotPopup()
    {

        yield return new WaitForSeconds(1.0f);
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
        findPopupView.Show();
        yield return new WaitUntil(() => !findPopupView.IsVisible);
        yield return new WaitForSeconds(1.0f);
        if (NbSlotBaseData.spinType == SpinType.Normal)
        {
            SoundSystem.SetBgm(QO_Sound.GetSound(QO_Define.QO_MAIN_BGM));
        }
        else SoundSystem.SetBgm(QO_Sound.GetSound(QO_Define.QO_FS_BGM));
        End();
    }
}

