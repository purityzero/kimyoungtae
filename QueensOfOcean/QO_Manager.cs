using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using SlotGame;
using UnityEngine.UI;
using BSS;
using BSS.View;

public class QO_Manager : PassiveSingleton<QO_Manager>
{
    public Machine BaseMachine => NbSlotBaseData.baseMachine;
    private BottomView bottomview => ViewUtility.Find<BottomView>();
    public GameObject MatchCover;

    private bool IsMatch => NbSlotBaseData.spinResponce.spinResult.Count > 0;
    private bool IsSpinFlowReady = false;
    private bool IsMatchPlay;

    private List<GameObject> winAnimations = new List<GameObject>();
    private List<GameObject> originSymbols = new List<GameObject>();
    private List<GameObject> wildSymbols = new List<GameObject>();
    [SerializeField] [ReadOnly]
    private List<string> AnimationEventList = new List<string>();
    private List<NbCommon.SpinResult> BeforeResult = new List<NbCommon.SpinResult>();

    [SerializeField]
    private TweenElement WinTween;
    [SerializeField]
    private SpriteAnimator MWSign;
    [SerializeField]
    private TweenPlayer FlarescapeFlare;

    [SerializeField]
    private QO_MatchGameManager QO_MatchGame;
    [SerializeField]
    private Sprite TopTitle;
    [SerializeField]
    private Sprite TopFreeSpins;
    [SerializeField]
    private Image Top;

    private Coroutine coWaitAnimation;

    private async void Start()
    {
        MWSign.gameObject.SetActive(false);
        await QO_Setting.InitSetting();
        QO_HitFrameController.instance.HitFrameSetting();
        ButtonSetting();
        SoundSystem.SetBgm(QO_Sound.GetSound(QO_Define.QO_MAIN_BGM));
        NbSlotBaseData.playReady = true;
        bottomview.SetForceWinMoneyUpdate(0);
        QO_MatchGame.gameObject.SetActive(true);
    }

    private void ButtonSetting()
    {
        BottomView.OnSpinPress += () => {
            Spin();
        };
        BottomView.OnStopPress += () => {
            StartCoroutine(coStop());
        };
        BottomView.OnAutoStop += () => StartCoroutine(CoWaitStop());
        BottomView.OnMaxBet += () => {
            bottomview.SpinBtn.onClick.Invoke();
            bottomview.SpinBtn.interactable = false;
        };
        BottomView.OnInfoPress += () => {
            ViewUtility.Find<QO_PayTableView>().Show();
        };
    }

    #region SPIN N STOP
    private async void Spin()
    {
        if (!NbSlotBaseData.playReady) return;
        NbSlotBaseData.playReady = false;
        MatchCover.SetActive(false);
        if (winAnimations.Count > 0) ClearAnimation();
        if (wildSymbols.Count > 0) ClearWildAnmation();
        IsMatchPlay = false;
        IsSpinFlowReady = true;
        isStop = false;
        isWaitStop = false;
        ClearAnimation();
        ClearWildAnmation();
        BaseMachine.reels.ForEach(x => x.Spin());
        await NbNetworkSystem.instance.SpinAsync();
        //await NbNetworkSystem.instance.SpinCheatAsync(NbCommon.CheatCode.Jackpot, NbCommon.SubCheatCode.Mini);
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
        {
            StartCoroutine(CoSpinFlowStart());
        }
    }

    private IEnumerator CoWaitStop()
    {
        if (!BaseMachine.isStopReady || isWaitStop) yield break;
        yield return new WaitUntil(() => BaseMachine.isStopReady);
        isWaitStop = true;

        if (NbSlotBaseData.spinResponce.GetMWEvent() != null) {
            yield return StartCoroutine(coMWSignPlayOnce());
            yield return StartCoroutine(CoMWEvent());

        }

        for (int j = 0; j < BaseMachine.reels.Count; j++) {
            yield return new WaitForSeconds(ReelDelayEffecter.instance.GetDelayTime(j) + QO_Define.REEL_DELAY);
            BaseMachine.reels[j].Stop(0, NbSlotBaseData.spinResponce.cursorData[j]);

        }
        yield return new WaitUntil(() => BaseMachine.isStopAll);
        isStop = false;
        if (isWaitStop) {
            StartCoroutine(CoSpinFlowStart());
        }
    }
    #endregion

    private IEnumerator CoSpinFlowStart()
    {
        if (!IsSpinFlowReady) yield break;

        bottomview.forceState = BottomView.BtnShowState.SpinTurnOff;
        IsSpinFlowReady = false;

        yield return new WaitUntil(() => NbSlotBaseData.spinResponce.spinResult != null);
        var spinReulst = NbSlotBaseData.spinResponce.spinResult;


        if (IsMatch)
        {
            yield return StartCoroutine(CoFirstMatchAnimationEvent(spinReulst));
        }

        if (IsMatch && NbSlotBaseData.spinType == SpinType.Normal)
            yield return StartCoroutine(CoWinEvent());
        else
        {
            if (NbSlotBaseData.freeSpinInfo != null)
                bottomview.SetForceWinMoneyUpdate(NbSlotBaseData.freeSpinInfo.totalWin);
        }

        if (NbCommonExtension.GetJackPotEvent(NbSlotBaseData.spinResponce) != null)
            yield return StartCoroutine(CoJackPotEvent());

        if (NbCommonExtension.GetScatterEvent(NbSlotBaseData.spinResponce) != null)
            yield return StartCoroutine(CoFreeSpinStartEvent());

        if (NbSlotBaseData.freeSpinInfo != null)
            yield return StartCoroutine(CoFreeSpinEndEvent());

        if (IsMatch && !(bottomview.isAutoSpin || NbSlotBaseData.spinType == SpinType.Free))
        {
            StartCoroutine(CoWaitPlayingMatchAnimationEvent(spinReulst));
        }

        if (bottomview.isAutoSpin)
        {
            yield return new WaitUntil(() => BaseMachine.isStopAll);
            yield return new WaitForSeconds(0.5f);
        }

        NbSlotBaseData.playReady = true;
        bottomview.forceState = BottomView.BtnShowState.Idle;

    }

    private IEnumerator CoMWEvent()
    {
        var MWEvent = NbSlotBaseData.spinResponce.GetMWEvent();

        for (int i = 0; i < MWEvent.mysteryWild.Count; i++) {

            var baseMw = MWEvent.mysteryWild[i];
            var MWSymbolPosKey = baseMw.reelPosition.ToVector2Int();
            int MwMutiple = baseMw.multiple;

            var createMW = Instantiate(QO_SymbolData.GetMWSymobl(MwMutiple).spriteRender);
            createMW.transform.position = BaseMachine.GetPosition(MWSymbolPosKey);
            SoundSystem.PlayOnce(QO_Sound.GetSound(QO_Define.QO_MULTIPLIER_WILD_APEAER));
            wildSymbols.Add(createMW.gameObject);
            yield return new WaitForSeconds(0.6f);
        }
    }


    #region SpinFlowEvent
    private IEnumerator CoFirstMatchAnimationEvent(List<NbCommon.SpinResult> spinResultList)
    {
        if (AnimationEventList.Count != 0)
        {
            StopCoroutine(coWaitAnimation);
            AnimationEventList.Clear();
            ClearAnimation();
        }

        IsMatchPlay = true;
        MatchCover.SetActive(true);
        yield return new WaitUntil(() => BeforeResult != spinResultList);
        AnimationEventList.Add("FirstMatchAnimation");
        BeforeResult.Clear();
        BeforeResult = spinResultList;
        coWaitAnimation = StartCoroutine(coAnimationPlayEvent(false, spinResultList));
        yield return coWaitAnimation;
        AnimationEventList.Remove("FirstMatchAnimation");
        MatchCover.SetActive(false);
        IsMatchPlay = false;
    }

    private IEnumerator CoJackPotEvent()
    {
        var scatterSymbols = QO_Utility.FindPositionsInSymbols(x => x == 30);
        List<GameObject> emptySymbols = new List<GameObject>();
        List<GameObject> orignSymbols = new List<GameObject>();
        SoundSystem.PlayOnce(QO_Sound.GetSound(QO_Define.QO_JACKPOT_SYMBOL));
        MatchCover.SetActive(true);
        for (int i = 0; i < scatterSymbols.Count; i++)
        {
            var sprAnim = Instantiate(QO_SymbolData.GetSymbolAnimator(3000.ToString()));
            sprAnim.gameObject.transform.position = BaseMachine.GetPosition(scatterSymbols[i]);
            sprAnim.gameObject.transform.position = new Vector2(sprAnim.gameObject.transform.position.x, sprAnim.gameObject.transform.position.y + 0.25f);
            sprAnim.sortingOrder = QO_Define.MATCH_CANVAS_SORTING_ORDER;
            emptySymbols.Add(sprAnim.gameObject);
            originSymbols.Add(BaseMachine.GetRender(scatterSymbols[i]).gameObject);
        }
        originSymbols.ForEach(x => x.SetActive(false));
        yield return new WaitForSeconds(0.45f);
        originSymbols.ForEach(x => x.SetActive(true));
        yield return new WaitForSeconds(0.5f);
        emptySymbols.ForEach(x =>
        {
            Instantiate(FlarescapeFlare).transform.position = x.transform.position;
        });
        emptySymbols.ForEach(x => Destroy(x.gameObject));
        emptySymbols.Clear();
        MatchCover.SetActive(false);
        yield return new WaitForSeconds(0.55f);
        QO_MatchGame.Play();
        yield return new WaitUntil(() => !QO_MatchGame.IsPlaying);

    }

    private IEnumerator CoFreeSpinStartEvent()
    {
        var scatterSymbols = QO_Utility.FindPositionsInSymbols(x => x == 20);
        List<GameObject> emptySymbols = new List<GameObject>();
        List<GameObject> orignSymbols = new List<GameObject>();
        MatchCover.SetActive(true);
        SoundSystem.PlayOnce(QO_Sound.GetSound(QO_Define.QO_FS_ALARM));
        for (int i = 0; i < scatterSymbols.Count; i++)
        {
            var sprAnim = Instantiate(QO_SymbolData.GetSymbolAnimator(2000.ToString()));
            sprAnim.gameObject.transform.position = BaseMachine.GetPosition(scatterSymbols[i]);
            sprAnim.gameObject.transform.position = new Vector2(sprAnim.gameObject.transform.position.x, sprAnim.gameObject.transform.position.y) + QO_SymbolData.CorrectSymbolPosition(2000.ToString());
            sprAnim.sortingOrder = QO_Define.MATCH_CANVAS_SORTING_ORDER;
            emptySymbols.Add(sprAnim.gameObject);
            originSymbols.Add(BaseMachine.GetRender(scatterSymbols[i]).gameObject);
        }
        originSymbols.ForEach(x => x.SetActive(false));
        yield return new WaitForSeconds(2f);
        originSymbols.ForEach(x => x.SetActive(true));
        emptySymbols.ForEach(x => Destroy(x.gameObject));
        emptySymbols.Clear();
        MatchCover.SetActive(false);

        ViewUtility.Find<QO_FreeSpinStartPopup>().Show();
        yield return new WaitUntil(() => !ViewUtility.Find<QO_FreeSpinStartPopup>().IsVisible);
        SoundSystem.SetBgm(QO_Sound.GetSound(QO_Define.QO_FS_BGM));
        NbSlotBaseData.spinType = SpinType.Free;
        bottomview.FSBottomView.Cnt.text = NbSlotBaseData.freeSpinInfo.remainCount.ToString();
        bottomview.SetForceWinMoneyUpdate(0);
        Top.sprite = TopFreeSpins;
        Top.SetNativeSize();
    }

    private IEnumerator CoWinEvent()
    {
        yield return StartCoroutine(bottomview.CoWinBalancePlay());
    }

    private IEnumerator CoFreeSpinEndEvent()
    {
        if (NbSlotBaseData.freeSpinInfo.remainCount == 0)
        {
            NbSlotBaseData.playReady = false;
            NbSlotBaseData.spinType = SpinType.Normal;
            bottomview.forceState = BottomView.BtnShowState.SpinTurnOff;
            var freeExitPopup = ViewUtility.Find<QO_FreeSpinEndPopup>();
            freeExitPopup.Show();
            yield return new WaitUntil(() => !freeExitPopup.IsVisible);
            SoundSystem.SetBgm(QO_Sound.GetSound(QO_Define.QO_MAIN_BGM));
            Top.sprite = TopTitle;
            Top.SetNativeSize();
        }
    }

    private IEnumerator CoWaitPlayingMatchAnimationEvent(List<NbCommon.SpinResult> spinResultList)
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
        while (IsMatchPlay && BaseMachine.isSpinReady)
        {
            coWaitAnimation = StartCoroutine(coAnimationPlayEvent(true, spinResultList));
            yield return coWaitAnimation;
        }
        AnimationEventList.Remove("WaitPlayingMatchAnimation");
        MatchCover.SetActive(false);
    }
    #endregion

    #region EventUtility


    private IEnumerator coMWSignPlayOnce()
    {
        SoundSystem.PlayOnce(QO_Sound.GetSound(QO_Define.QO_MULTIPLIER_WILD_SIGN));
        MWSign.gameObject.SetActive(true);
        MWSign.Play(MWSign.animations[0]);
        yield return new WaitForSeconds(MWSign.animations[0].realDuration);
        MWSign.gameObject.SetActive(false);
    }

    private IEnumerator coAnimationPlayEvent(bool isSequential, List<NbCommon.SpinResult> spinResultList)
    {
        yield return new WaitUntil(() => !instance.BaseMachine.reels[4].GetComponent<TweenPlayer>().isPlaying);
        yield return null;
        float waitDuration = 2f; // 기본 값
        for (int i = 0; i < spinResultList.Count; i++)
        {
            foreach (var postion in spinResultList[i].reelPositions)
            {
                var key = new Vector2Int(postion.x, postion.y);
                instance.originSymbols.Add(BaseMachine.GetRender(key).gameObject);
                var winSymbolSprRenderer = BaseMachine.GetRender(key);
                QO_HitFrameController.instance.HitItemShow(key);
                string symbolID = QO_SymbolData.GetSymbol(winSymbolSprRenderer).ID;
                //Symbol Spr Animation 이 존재하는지 여부
                if (!wildSymbols.Exists(x => BaseMachine.GetPosition(postion.ToVector2Int()) == x.transform.position))
                {
                    if (QO_SymbolData.ExistSymbolAnimator(symbolID))
                    {
                        var animaitor = Instantiate(QO_SymbolData.GetSymbolAnimator(symbolID));
                        animaitor.transform.position = winSymbolSprRenderer.transform.position;
                        animaitor.transform.position = (Vector2)animaitor.transform.position + QO_SymbolData.CorrectSymbolPosition(symbolID);
                        animaitor.sortingOrder = QO_Define.MATCH_CANVAS_SORTING_ORDER;
                        instance.winAnimations.Add(animaitor.gameObject);
                    }
                    else
                    {
                        var Tween = Instantiate(winSymbolSprRenderer).gameObject;
                        Tween.SetActive(true);
                        Tween.transform.position = winSymbolSprRenderer.transform.position;
                        Tween.AddComponent<TweenPlayer>().Play(instance.WinTween);
                        Tween.GetComponent<SpriteRenderer>().sortingOrder = QO_Define.MATCH_CANVAS_SORTING_ORDER;
                        winAnimations.Add(Tween.gameObject);
                    }
                } else {
                    var wildSymbol = wildSymbols.Find(x => BaseMachine.GetPosition(postion.ToVector2Int()) == x.transform.position).GetComponent<SpriteAnimator>();
                    wildSymbol.sortingOrder = QO_Define.MATCH_CANVAS_SORTING_ORDER;
                    wildSymbol.Play(wildSymbol.animations[0]);
                    var multipleAnim = wildSymbol.transform.GetChild(0).GetComponent<SpriteAnimator>();
                    multipleAnim.sortingOrder = wildSymbol.sortingOrder + 1;
                    multipleAnim.Play(multipleAnim.animations[0]);
                }
            }
            if (isSequential && instance.BaseMachine.isSpinReady)
            {
                yield return new WaitForSeconds(waitDuration);
                ClearAnimation();
                WildAnmationReset();
                if (!instance.IsMatchPlay) {
                    yield break;
                }
            }
        }

        if (!isSequential)
        {
            yield return new WaitForSeconds(waitDuration);
            ClearAnimation();
            WildAnmationReset();
            if (!instance.IsMatchPlay) {
                yield break;
            }
        }
    }
    #endregion

    private void ClearAnimation()
    {
        winAnimations.ForEach(x => Destroy(x.gameObject));
        originSymbols.ForEach(x => x.SetActive(true));
        winAnimations.Clear();
        originSymbols.Clear();
        QO_HitFrameController.instance.HitItemHideAll();
    }

    private void ClearWildAnmation()
    {
        wildSymbols.ForEach(x => Destroy(x.gameObject));
        wildSymbols.Clear();
    }



    private void WildAnmationReset()
    {
        wildSymbols.ForEach(x =>
        {
            x.GetComponent<SpriteAnimator>().sortingOrder = QO_Define.MATCH_CANVAS_SORTING_ORDER - 3;
            var multipleAnim = x.transform.GetChild(0).GetComponent<SpriteAnimator>();
            multipleAnim.sortingOrder = QO_Define.MATCH_CANVAS_SORTING_ORDER -2;
        });
    }

}
