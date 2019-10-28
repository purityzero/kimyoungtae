using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bread;
using Sirenix.OdinInspector;
using BSS;

public class TdMiniScrollViewContorller : PassiveSingleton<TdMiniScrollViewContorller>
{
    public MiniScrollView miniScrollView;
    [BoxGroup("ShineEffect")]
    public GameObject ShineEffect;
    [BoxGroup("ShineEffect")]
    public TweenElement ShineTween;
    [BoxGroup("HaloEffect")]
    public GameObject HaloEffect;
    [BoxGroup("HaloEffect")]
    public TweenElement HaloTween;
    [BoxGroup("MultiplierDonwEffect")]
    public GameObject Multipler;
    [BoxGroup("MultiplierDonwEffect")]
    public TweenElement MultiplierDownEffect;
    [BoxGroup("MultiplierDonwEffect")]
    public float WaitDownTime;

    public GameObject Board;

    public TweenElement BoardShowTween;
    public TweenElement BoardChoiceTween;

    public bool IsEffectStart { get; private set; }

    private IEnumerator Spin()
    {
        IsEffectStart = false;
        BoardTweenEffect(BoardShowTween);
        yield return new WaitUntil(() => !Board.GetComponent<TweenPlayer>().isPlaying);
        yield return new WaitForSeconds(0.5f);
        SoundSystem.PlayOnce(ThreeDragonUtility.TD_MULTIPLIER_WILD_SETTING);
        miniScrollView.Spin();
    }

    private void Confirm(string _confirmStr)
    {
        miniScrollView.Confirm(_confirmStr);
    }


    public void MultiplerEvent(List<string> multiplerList, string confirmStr, float confirmTime = 3f)
    {
        Show();
        EffectHide();
        SetContents(multiplerList);
        StartCoroutine(Spin());
        StartCoroutine(CoMultiplerConfirm(confirmStr));
    }

    private void SetContents(List<string> contents)
    {
        miniScrollView.SetContentsMember(contents);
    }

    private IEnumerator CoMultiplerConfirm(string confirmStr, float confirmTime = 3f)
    {
        yield return new WaitForSeconds(confirmTime);
        Confirm(confirmStr);
        yield return new WaitUntil(() => miniScrollView.IsComplelet);
        BoardTweenEffect(BoardChoiceTween);
        yield return new WaitUntil(() => !Board.GetComponent<TweenPlayer>().isPlaying);
        SoundSystem.PlayOnce(ThreeDragonUtility.TD_MULTIPLIER_WILD_FIX);
        EffectShow();
        EffectAnimationStart();
        StartCoroutine(coWaitBoardDownMove());
    }

    public void Show()
    {
        miniScrollView.gameObject.SetActive(true);
    }

    public void Hide()
    {
        miniScrollView.gameObject.SetActive(false);
    }

    public bool EffectAnimationEnd()
    {
        return ShineEffect.GetComponent<TweenPlayer>().isPlaying && HaloEffect.GetComponent<TweenPlayer>().isPlaying;
    }

    private void EffectShow()
    {
        ShineEffect.SetActive(true);
        HaloEffect.SetActive(true);
    }

    private void EffectHide()
    {
        ShineEffect.SetActive(false);
        HaloEffect.SetActive(false);
    }

    private void EffectAnimationStart()
    {
        IsEffectStart = true;
        ShineEffect.GetComponent<TweenPlayer>().Play(ShineTween);
        HaloEffect.GetComponent<TweenPlayer>().Play(HaloTween);
    }

    public void BoardTweenEffect(TweenElement tweener)
    {
        Board.GetComponent<TweenPlayer>().Play(tweener);
    }


    private IEnumerator coWaitBoardDownMove()
    {
        yield return new WaitForSeconds(WaitDownTime);
        EffectHide();
        Board.GetComponent<TweenPlayer>().Play(MultiplierDownEffect);
        yield return new WaitUntil(() => !Board.GetComponent<TweenPlayer>().isPlaying);
        IsEffectStart = false;
        Hide();
    }
}
