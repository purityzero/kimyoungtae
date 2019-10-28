using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BSS;
using BSS.View;
using SlotGame;
using System;
using Sirenix.OdinInspector;


// Todo
// 3. 마지막 연출 Second 연출 에 Stop이 아닌 Spin으로 
public class ThreeDragonManager : PassiveSingleton<ThreeDragonManager>
{
    public Machine BaseMachine => NbSlotBaseData.baseMachine;
    public int BetLevel => NbSlotBaseData.betIndex;
    public int DragonLevel = 0;
    public int RandomDragonLevel = 0;
    public List<Sprite> SymbolIcons => ThreeDragonData.instance.SymbolIcons;

    public GameObject MatchCover;
    private BottomView bottomview => ViewUtility.Find<BottomView>();
    public ThreeDragonMatchGame tdMatchGame;

    private bool IsMatch => NbSlotBaseData.spinResponce.spinResult.Count > 0;

    private List<GameObject> winAnimations = new List<GameObject>();
    private List<GameObject> originSymbols = new List<GameObject>();
    [SerializeField] [ReadOnly]
    private List<string> AnimationEventList = new List<string>();
    private List<NbCommon.SpinResult> BeforeResult = new List<NbCommon.SpinResult>();

    private bool IsMatchPlay;
    private bool IsSpinFlowReady = false;
    public static bool IsMWEvent;

    private Coroutine coWaitAnimation;

    async void Start()
    {
        ThreeDragonData.instance.BonusIn.gameObject.SetActive(false);
        await ThreeDragonSetting.InitSetting();
        ButtonActionSetting();
        ThreeDragonSlotSetting();

        NbSlotBaseData.playReady = true;
        bottomview.SetForceWinMoneyUpdate(0);
    }

    #region "Setting"
    private void ThreeDragonSlotSetting()
    {
        TdMiniScrollViewContorller.instance.Hide();
        ThreeDragonReelOnManager.instance.ReelOnUpdate();
        SoundSystem.SetBgm(ThreeDragonUtility.TD_MAIN_BGM);
        BaseMachine.reels.ForEach(x => x.OnStoped += ReelStopSound);
        BaseMachine.OnSpinEnded += ResetReelSpeed;
    }

    private void ResetReelSpeed()
    {
        BaseMachine.reels.ForEach(x => {
            x.ResetSpeed();
       });
    }

    private void ReelStopSound()
    {
        SoundSystem.PlayOnce(SoundSystem.instance.clips.Find(x => x.name == ThreeDragonUtility.TD_REEL_STOP));
    }

    private void ButtonActionSetting()
    {
        BottomView.OnSpinPress += () =>
        {
            Spin();
        };
        BottomView.OnStopPress += () =>
        {
            StartCoroutine(coStop());
        };
        BottomView.OnAutoStop += () => StartCoroutine(CoWaitStop());
        BottomView.OnMaxBet += () =>
        {
            ThreeDragonReelOnManager.instance.ReelOnUpdate();
            bottomview.SpinBtn.onClick.Invoke();
            bottomview.SpinBtn.interactable = false;
        };
        BottomView.OnDownBet += ThreeDragonReelOnManager.instance.ReelOnUpdate;
        BottomView.OnUpBet += ThreeDragonReelOnManager.instance.ReelOnUpdate;
        BottomView.OnInfoPress += () =>
        {
            ViewUtility.Find<ThreeDragonInfopanel>().Show();
        };
    }
    #endregion

    #region "Spin N Stop"
    private async void Spin()
    {
        if (!NbSlotBaseData.playReady) return;
        NbSlotBaseData.playReady = false;
        IsMatchPlay = false;
        MatchCover.SetActive(false);
        originSymbols.ForEach(x => x.SetActive(true));
        originSymbols.Clear();

        winAnimations.ForEach(x => Destroy(x.gameObject));
        winAnimations.Clear();
        //await new WaitUntil(() => !MatchCover.activeSelf);
        IsMWEvent = false;
        IsSpinFlowReady = true;
        isStop = false;
        isWaitStop = false;
        BaseMachine.SpinAll();
        await NbNetworkSystem.instance.SpinAsync();
        //await NbNetworkSystem.instance.SpinCheatAsync(NbCommon.CheatCode.Scatter);
    }


    bool isStop = false;
    bool isWaitStop = false;
    private IEnumerator coStop()
    {
        if (!BaseMachine.isStopReady) yield break;
        yield return new WaitUntil(() => NbSlotBaseData.spinResponce != null);
        yield return new WaitUntil(() => BaseMachine.isStopReady);
        if (NbSlotBaseData.spinResponce.GetMWEvent() != null)
        {
            bottomview.forceState = BottomView.BtnShowState.StopTurnOff;
            StartCoroutine(CoWaitStop());
            yield break;
        }
        isStop = true;
        BaseMachine.StopAll(NbSlotBaseData.spinResponce.cursorData);
        yield return new WaitUntil(() => BaseMachine.isStopAll);
        isWaitStop = false;
        if (isStop)
            StartCoroutine(coStartSpinFlow());
    }
    private IEnumerator CoWaitStop()
    {
        if (!BaseMachine.isStopReady || isWaitStop) yield break;
        yield return new WaitUntil(() => BaseMachine.isStopReady);
        isWaitStop = true;

        if (NbSlotBaseData.spinResponce.GetMWEvent() != null)
        {
            bottomview.forceState = BottomView.BtnShowState.StopTurnOff;
            SoundSystem.PlayOnce(ThreeDragonUtility.TD_MULTIPLIER_WILD_PRE);
            yield return StartCoroutine(ThreeDragonUtility.ShowMWEffectOnce());
            IsMWEvent = true;
            yield return new WaitForSeconds(0.5f);
        }
        for (int j = 0; j < BaseMachine.reels.Count; j++)
        {
            yield return new WaitForSeconds(ThreeDragonUtility.REEL_DELAY + ReelDelayEffecter.instance.GetDelayTime(j));
            BaseMachine.reels[j].Stop(0, NbSlotBaseData.spinResponce.cursorData[j]);
        }
        yield return new WaitUntil(() => BaseMachine.isStopAll);
        isStop = false;
        if (isWaitStop)
            StartCoroutine(coStartSpinFlow());
    }
    #endregion
    private IEnumerator coStartSpinFlow()
    {
        if (!IsSpinFlowReady) yield break;
        IsSpinFlowReady = false;
        bottomview.forceState = BottomView.BtnShowState.SpinTurnOff;
        yield return new WaitUntil(() => NbSlotBaseData.spinResponce.spinResult != null);
        var spinReulst = NbSlotBaseData.spinResponce.spinResult;
        if (IsMatch)
        {
            yield return StartCoroutine(coFirstMatchAnimaitonPlayEvent(spinReulst));
            bottomview.SetForceWinMoneyUpdate(NbSlotBaseData.spinResponce.winMoney / NbSlotBaseData.spinResponce.sumMultiple);
        }

        if (NbSlotBaseData.spinResponce.GetMWEvent() != null)
            yield return StartCoroutine(coMWEvent());

        if (IsMatch && NbSlotBaseData.spinType == SpinType.Normal) yield return StartCoroutine(coWinEvent());
        else
        {
            if (NbSlotBaseData.freeSpinInfo != null)
                bottomview.SetForceWinMoneyUpdate(NbSlotBaseData.freeSpinInfo.totalWin);
        }

        if (NbCommonExtension.GetJackPotEvent(NbSlotBaseData.spinResponce) != null)
            yield return StartCoroutine(coJackPotEvent());

        if (NbCommonExtension.GetScatterEvent(NbSlotBaseData.spinResponce) != null)
            yield return StartCoroutine(coScatterEvent());


        if (NbSlotBaseData.freeSpinInfo != null)
            yield return StartCoroutine(coFreeSpinExitEvent());

        if (IsMatch && !(bottomview.isAutoSpin || NbSlotBaseData.spinType == SpinType.Free))
        {
            StartCoroutine(coSecondMatchAnimaitonPlayEvent(spinReulst));
        }

        if (bottomview.isAutoSpin) {
            yield return new WaitForSeconds(0.5f);
        }

        NbSlotBaseData.playReady = true;
        bottomview.forceState = BottomView.BtnShowState.Idle;
    }

    #region "EventList"
    private IEnumerator coScatterEvent()
    {
        float waitDuration = 0;
        SoundSystem.PlayLoopInTime(ThreeDragonUtility.TD_FREE_ALARM, 2);
        var scatterSymbols = FindPositionsInSymbol(x => x == 20);
        List<GameObject> emptySymbols = new List<GameObject>();
        MatchCover.SetActive(true);
        for (int i = 0; i < scatterSymbols.Count; i++)
        {
            var spine = ThreeDragonUtility.SpineWinAnim(BaseMachine.GetRender(scatterSymbols[i]).sprite.name);
            spine.gameObject.transform.position = BaseMachine.GetPosition(scatterSymbols[i]);
            spine.gameObject.transform.position = new Vector2(spine.gameObject.transform.position.x, spine.gameObject.transform.position.y - 0.975f);
            spine.transform.localScale = new Vector2(1.35f, 1.35f);
            emptySymbols.Add(spine.gameObject);
            if (spine.skeletonDataAsset.GetSkeletonData(true).animations.Items[0].duration > waitDuration)
            {
                waitDuration = spine.skeletonDataAsset.GetSkeletonData(true).animations.Items[0].duration;
            }
        }
        yield return new WaitForSeconds(waitDuration);
        MatchCover.SetActive(false);
        emptySymbols.ForEach(x => Destroy(x.gameObject));
        emptySymbols.Clear();
        yield return StartCoroutine(coShowEventEffect());
        yield return StartCoroutine(CoFreeSpinEvent());
        bottomview.SetForceWinMoneyUpdate(0);
    }
    private IEnumerator coAnimationPlayEvent(bool isSequential, List<NbCommon.SpinResult> spinResultList)
    {
        yield return null;
        float waitDuration = 1.5f; // 기본 값
        for (int i = 0; i < spinResultList.Count; i++)
        {
            if (!IsMatchPlay) yield break;
            foreach (var postion in spinResultList[i].reelPositions)
            {
                var key = new Vector2Int(postion.x, postion.y);
                originSymbols.Add(BaseMachine.GetRender(key).gameObject);
                BaseMachine.GetRender(key).gameObject.SetActive(false);
                if (ThreeDragonUtility.IsSpineAnim(BaseMachine.GetRender(key)))  // 스파인 Animation의 존재 여부를 확인합니다.
                {
                    var spine = ThreeDragonUtility.SpineWinAnim(BaseMachine.GetRender(key).sprite.name);
                    spine.gameObject.transform.position = BaseMachine.GetPosition(key);
                    spine.gameObject.transform.position = new Vector2(spine.gameObject.transform.position.x, spine.gameObject.transform.position.y - 0.975f);
                    spine.transform.localScale = new Vector2(1.35f, 1.35f);
                    winAnimations.Add(spine.gameObject);
                    // 애니메이션의 초를 체크해 기본 wait 값보다 크면 값을 바꿔 줍니다.
                    if (spine.skeletonDataAsset.GetSkeletonData(true).animations.Items[0].duration > waitDuration)
                    {
                        waitDuration = spine.skeletonDataAsset.GetSkeletonData(true).animations.Items[0].duration;
                    }
                } else {       // Spine이 없으면 Tween으로 바꿔줍니다.
                    var Tween = ThreeDragonUtility.TweenWinAnim(BaseMachine.GetRender(key).sprite.name);
                    Tween.gameObject.transform.position = BaseMachine.GetPosition(key);
                    Tween.GetComponent<SpriteRenderer>().sortingOrder = 2000;
                    winAnimations.Add(Tween);
                }
            }
            if (isSequential)
            {
                yield return new WaitForSeconds(waitDuration);
                ClearAnimation();
                if (!IsMatchPlay) yield break;
            }
        }

        if (!isSequential)
        {
            yield return new WaitForSeconds(waitDuration);
            ClearAnimation();
            if (!IsMatchPlay) yield break;
        }
    }
    private IEnumerator coShowEventEffect()
    {
        ThreeDragonData.instance.BonusIn.gameObject.SetActive(true);
        bottomview.forceState = BottomView.BtnShowState.SpinTurnOff;
        SoundSystem.PlayOnce(ThreeDragonUtility.TD_EVENT_GAME_EFFECT_SOUND);
        ThreeDragonData.instance.BonusIn.AnimationState.SetAnimation(0, ThreeDragonData.instance.BonusIn.SkeletonDataAsset.GetSkeletonData(true).animations.Items[0].name, false);
        yield return new WaitForSeconds(ThreeDragonData.instance.BonusIn.SkeletonDataAsset.GetSkeletonData(true).Animations.Items[1].Duration - 0.5f);
        ThreeDragonData.instance.BonusIn.gameObject.SetActive(false);
    }
    private IEnumerator coMWEvent()
    {
        if (IsMatch)
        {
            List<string> numberList = ThreeDragonUtility.GetMWMultiList(NbSlotBaseData.spinResponce.sumMultiple.ToString());
            TdMiniScrollViewContorller.instance.Show();
            TdMiniScrollViewContorller.instance.MultiplerEvent(numberList, NbSlotBaseData.spinResponce.sumMultiple.ToString());
            yield return new WaitUntil(() => !TdMiniScrollViewContorller.instance.Multipler.activeSelf);
            yield return new WaitForSeconds(1f);
        }
    }
    private IEnumerator coWinEvent()
    {
        yield return StartCoroutine(bottomview.CoWinBalancePlay());
    }
    private IEnumerator coJackPotEvent()
    {
        SoundSystem.PlayLoopInTime(ThreeDragonUtility.TD_FREE_ALARM, 2);
        var jackPotSmybols = FindPositionsInSymbol(x => x == 30);
        float waitDuration = 0;
        List<GameObject> emptySymbols = new List<GameObject>();
        MatchCover.SetActive(true);
        for (int i = 0; i < jackPotSmybols.Count; i++)
        {
            var spine = ThreeDragonUtility.SpineWinAnim(BaseMachine.GetRender(jackPotSmybols[i]).sprite.name);
            spine.gameObject.transform.position = BaseMachine.GetPosition(jackPotSmybols[i]);
            spine.gameObject.transform.position = new Vector2(spine.gameObject.transform.position.x, spine.gameObject.transform.position.y - 0.975f);
            spine.transform.localScale = new Vector2(1.35f, 1.35f);
            emptySymbols.Add(spine.gameObject);
            if (spine.skeletonDataAsset.GetSkeletonData(true).animations.Items[0].duration > waitDuration)
            {
                waitDuration = spine.skeletonDataAsset.GetSkeletonData(true).animations.Items[0].duration;
            }
        }
        yield return new WaitForSeconds(waitDuration);
        MatchCover.SetActive(false);
        emptySymbols.ForEach(x => Destroy(x.gameObject));
        emptySymbols.Clear();

        SoundSystem.PauseBgm();
        yield return StartCoroutine(coShowEventEffect());
        SoundSystem.SetBgm(ThreeDragonUtility.TD_BONUS_GAME_BGM);
        tdMatchGame.Play();
        yield return new WaitUntil(() => !tdMatchGame.IsPlaying);
    }
    private IEnumerator coFreeSpinExitEvent()
    {
        if (NbSlotBaseData.freeSpinInfo.remainCount == 0)
        {
            NbSlotBaseData.playReady = false;
            NbSlotBaseData.spinType = SpinType.Normal;
            bottomview.forceState = BottomView.BtnShowState.SpinTurnOff;
            var freeExitPopup = ViewUtility.Find<TDFreeSpinExitPopup>();
            SoundSystem.PauseBgm();
            SoundSystem.PlayOnce(ThreeDragonUtility.TD_TOTAL_WIN_POPUP);
            freeExitPopup.Show();
            DragonLevel = 0;
            RandomDragonLevel = 0;
            yield return new WaitUntil(() => !freeExitPopup.IsVisible);
            SoundSystem.SetBgm(ThreeDragonUtility.TD_MAIN_BGM);

        }
    }
    private IEnumerator CoFreeSpinEvent()
    {
        SoundSystem.SetBgm(ThreeDragonUtility.TD_FREE_SELECT_BGM);
        var freespinView = ViewUtility.Find<TdFreeSpinView>();
        freespinView.Show();
        yield return new WaitUntil(() => !freespinView.IsVisible);

        var freeStartPopup = ViewUtility.Find<TDFreeSpinStartPopUp>();
        freeStartPopup.Show();
        SoundSystem.PauseBgm();
        SoundSystem.PlayOnce(ThreeDragonUtility.TD_FREE_SELECT_POPUP);
        yield return new WaitUntil(() => !freeStartPopup.IsVisible);

        SoundSystem.SetBgm(ThreeDragonUtility.TD_FREE_GAME_BGM);
        NbSlotBaseData.spinType = SpinType.Free;
    }
    private IEnumerator coFirstMatchAnimaitonPlayEvent(List<NbCommon.SpinResult> spinResultList)
    {
        if (AnimationEventList.Count != 0)
        {
            StopCoroutine(coWaitAnimation);
            AnimationEventList.Clear();
            ClearAnimation();
        }

        IsMatchPlay = true;
        SoundSystem.PlayOnce(ThreeDragonUtility.TD_PAY_LINE);
        MatchCover.SetActive(true);
        yield return new WaitUntil(() => BeforeResult != spinResultList);
        AnimationEventList.Add("FirstMatchAnimation");
        BeforeResult.Clear();
        BeforeResult = spinResultList;
        yield return StartCoroutine(coAnimationPlayEvent(false, spinResultList));
        AnimationEventList.Remove("FirstMatchAnimation");
        MatchCover.SetActive(false);
        IsMatchPlay = false;
    }
    private IEnumerator coSecondMatchAnimaitonPlayEvent(List<NbCommon.SpinResult> spinResults)
    {
        if (AnimationEventList.Count != 0)
        {
            StopCoroutine(coWaitAnimation);
            AnimationEventList.Clear();
            ClearAnimation();
        }

        IsMatchPlay = true;
        MatchCover.SetActive(true);
        AnimationEventList.Add("WaitPlayingMatchAnimation");
        while (IsMatchPlay && BaseMachine.isSpinReady ) {
            coWaitAnimation = StartCoroutine(coAnimationPlayEvent(true, spinResults));
            yield return coWaitAnimation;
        }
        AnimationEventList.Remove("WaitPlayingMatchAnimation");
        MatchCover.SetActive(false);
    }

    public List<Vector2Int> FindPositionsInSymbol(Predicate<int> symbolPredicate)
    {
        var list = new List<Vector2Int>();

        for (int i = 0; i < BaseMachine.reels.Count; i++)
            for (int j = 0; j < BaseMachine.slotSize.y; j++)
            {
                if (symbolPredicate(GetSymbolID(new Vector2Int(i, j))))
                {
                    list.Add(new Vector2Int(i, j));
                }
            }
        return list;
    }

    public static int GetSymbolID(Vector2Int pos)
    {
        var data = NbSlotBaseData.reelSetResult[pos];
        return data / 1000;
    }

    #endregion

    private void ClearAnimation()
    {
        winAnimations.ForEach(x => Destroy(x.gameObject));
        originSymbols.ForEach(x => x.SetActive(true));
        originSymbols.Clear();
    }

    public async void CheateSpin(NbCommon.CheatCode cheatCode)
    {
        if (!NbSlotBaseData.playReady) return;
        NbSlotBaseData.playReady = false;
        IsMatchPlay = false;
        MatchCover.SetActive(false);
        originSymbols.ForEach(x => x.SetActive(true));
        originSymbols.Clear();

        winAnimations.ForEach(x => Destroy(x.gameObject));
        winAnimations.Clear();

        IsMWEvent = false;
        IsSpinFlowReady = true;
        isStop = false;
        isWaitStop = false;
        BaseMachine.SpinAll();
        await NbNetworkSystem.instance.SpinCheatAsync(cheatCode);
        bottomview.forceState = BottomView.BtnShowState.Stop;
    }
    
    public async void CheateSpin(NbCommon.CheatCode cheatCode, NbCommon.SubCheatCode subCode)
    {
        if (!NbSlotBaseData.playReady) return;
        NbSlotBaseData.playReady = false;
        IsMatchPlay = false;
        MatchCover.SetActive(false);
        originSymbols.ForEach(x => x.SetActive(true));
        originSymbols.Clear();

        winAnimations.ForEach(x => Destroy(x.gameObject));
        winAnimations.Clear();

        IsMWEvent = false;
        IsSpinFlowReady = true;
        isStop = false;
        isWaitStop = false;
        BaseMachine.SpinAll();
        await NbNetworkSystem.instance.SpinCheatAsync(cheatCode, subCode);
        bottomview.forceState = BottomView.BtnShowState.Stop;
    }

}
