using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BSS.View;
using UnityEngine.UI;
using BSS;
using Sirenix.OdinInspector;
using NbCommon;

namespace SlotGame
{

    [System.Serializable]
    public class FreeSpinBottomView
    {
        public Image BackGround;
        public Text Cnt;

        public void Show()
        {
            BackGround.gameObject.SetActive(true);
            Cnt.gameObject.SetActive(true);
        }

        public void Hide()
        {
            BackGround.gameObject.SetActive(false);
            Cnt.gameObject.SetActive(false);
        }
    }

    public class BottomViewDefine
    {
        public enum AutoSpinCnt
        {
            Infinity = -1,
            First = 25,
            Second = 50,
            Third = 100,
        };
    }

    public class BottomView : BaseView
    {
        public enum BtnShowState
        {
            Idle, Spin, SpinTurnOff, Stop, StopTurnOff, Free
        }

        public BtnShowState forceState
        {
            get => _forceState;
            set
            {
                if (BtnShowState.Free == value)
                {
                    isPreAutoState = isAutoSpin;
                    isAutoSpin = true;
                    StartCoroutine(coAutoPlay());
                }
                else if (BtnShowState.Idle == value && _forceState == BtnShowState.Free)
                {
                    isAutoSpin = isPreAutoState;
                }
                _forceState = value;
            }
        }
        public BtnShowState _forceState;
        private BtnShowState baseState = BtnShowState.Idle;
        private bool isPreAutoState;


        private BtnShowState realState
        {
            get
            {
                if (forceState == BtnShowState.Idle) return baseState;
                else return forceState;
            }
        }

        public List<AudioClip> BottomAudio;

        public bool IsBottomReady = false;

        protected Machine baseMachine;
        public Button SpinBtn;
        public Button StopBtn;
        public FreeSpinBottomView FSBottomView;
        public Toggle AutoBtn;
        public Button AutoStopBtn;
        public Button MaxBetBtn;
        public Button DownBtn;
        public Button UpBtn;
        public Button InfoBtn;

        public Text BalanceTxt;
        public Text TotalBetTxt;
        public Text WinMoneyText;

        public GameObject SelectAuto;
        [BoxGroup("AutoPlay")]
        public Button AutoInfinit;
        [BoxGroup("AutoPlay")]
        public Button AutoHundred;
        [BoxGroup("AutoPlay")]
        public Button AutoFifty;
        [BoxGroup("AutoPlay")]
        public Button AutoTwentyFive;

        public Image infinitImg;
        public Text AutoCntText;
        public Image AutoImg;

        [SerializeField]
        [ReadOnly]
        [BoxGroup("AutoCount")]
        private int AutoCnt;

        // Press부분은 꼭 Manager에서 구현해주세요!!!!!!!!!!!!!!!
        // OnAutoStop 부분도 구현해주세요. AutoStopTime 후 Action을 호출합니다.
        // OnDownBet // OnUpBet // MaxBet도 기능적인 부분이 있으면 넣어주세요. 
        public static Action OnSpinPress;
        public static Action OnStopPress;
        public static Action OnInfoPress;
        public static Action OnAutoStop;

        public static Action OnDownBet;
        public static Action OnUpBet;
        public static Action OnMaxBet;

        private bool IsBetvChange = false;
        public bool isAutoSpin { get; private set; }

        public const string AUTO_SPIN = "auto_spin";
        public const string BET_DOWN = "bet_down";
        public const string BET_UP = "bet_up";
        public const string MAX_BET = "max_bet_button";

        [ReadOnly]
        public long Bottombalance;

        public long spinPressCount { get; private set; }
        public long stopPressCount { get; private set; }

        protected override void OnStart()
        {
            base.OnStart();
            AutoCnt = 0;
            StartCoroutine(coWaitBottomSetting());
        }

        private void AutoToggle(bool OnOff)
        {
            SoundSystem.PlayOnce(BottomAudio.Find(x => x.name == AUTO_SPIN));
            SelectAuto.gameObject.SetActive(OnOff);
        }

        private IEnumerator coWaitBottomSetting()
        {
            FSBottomView.BackGround.gameObject.SetActive(false);
            DownBtn.interactable = false;
            UpBtn.interactable = false;
            MaxBetBtn.interactable = false;
            SpinBtn.interactable = false;
            InfoBtn.interactable = false;
            AutoBtn.interactable = false;
            yield return new WaitUntil(() => NbSlotBaseData.betRange != null);
            baseMachine = NbSlotBaseData.baseMachine;
            SelectAuto.SetActive(false);
            BtnSetting();
            TotalBetUpdate();
            BalanceUpdate();
        }

        private void AutoStopPressed()
        {
            isAutoSpin = false;
            AutoCnt = 0;
            AutoCntText.gameObject.SetActive(false);
            infinitImg.gameObject.SetActive(false);
            AutoImg.gameObject.SetActive(false);
            MaxBetBtn.gameObject.SetActive(true);
            AutoBtn.gameObject.SetActive(true);
            AutoStopBtn.gameObject.SetActive(false);
            Stop();
        }

        private void BtnSetting()
        {
            IsBottomReady = true;
            AutoStopBtn?.onClick.AddListener(AutoStopPressed);

            // Button Cnt Setting
            {
                AutoHundred?.onClick.AddListener(() => SetAutoCnt((int)BottomViewDefine.AutoSpinCnt.Third));
                AutoFifty?.onClick.AddListener(() => SetAutoCnt((int)BottomViewDefine.AutoSpinCnt.Second));
                AutoTwentyFive?.onClick.AddListener(() => SetAutoCnt((int)BottomViewDefine.AutoSpinCnt.First));
                AutoInfinit?.onClick.AddListener(() => SetAutoCnt((int)BottomViewDefine.AutoSpinCnt.Infinity));
            }

            // Button Action Setting
            {
                AutoBtn?.onValueChanged.AddListener(AutoToggle);
                SpinBtn?.onClick.AddListener(Spin);
                StopBtn?.onClick.AddListener(Stop);
                DownBtn?.onClick.AddListener(DownBet);
                UpBtn?.onClick.AddListener(UpBet);
                MaxBetBtn?.onClick.AddListener(MaxBet);
                InfoBtn?.onClick.AddListener(Info);
            }

            {
                AutoBtn.interactable = true;
                InfoBtn.interactable = true;
                MaxBetBtn.interactable = true;
                SpinBtn.interactable = true;
            }

            IsBetvChange = true;
        }

        private void BalanceUpdate()
        {
            if (NbSlotBaseData.spinType == SpinType.Free && NbSlotBaseData.freeSpinInfo != null)
                return;

            SetForceBalanceUpdate(NbSlotBaseData.balance);
            Bottombalance = NbSlotBaseData.balance;
        }

        private void BalanceSpinUpdate()
        {
            //프리스핀 경우 정보갱신 X
            if (NbSlotBaseData.spinType == SpinType.Free && NbSlotBaseData.freeSpinInfo != null)
                return; 

            var bettingMoney  = NbSlotBaseData.betRange[NbSlotBaseData.betIndex];
            var AfterSpinBalance = Bottombalance - bettingMoney;

            SetForceBalanceUpdate(AfterSpinBalance);
        }

        private void TotalBetUpdate()
        {
            TotalBetTxt.text = $"{NbSlotBaseData.betRange[NbSlotBaseData.betIndex].ToString(NbDefine.NUMBER_FORMAT)} {NbDefine.BET_UNIT_TYPE}";
        }

        private void Spin()
        {
            AutoBtn.isOn = false;
            SelectAuto.gameObject.SetActive(false);
            if (NbDefine.AUTO_STOP_TIME > 0)
            {
                StartCoroutine(CoWaitAutoStop());
            }
            spinPressCount++;
            OnSpinPress?.Invoke();
            StopBtn.interactable = true;
            BalanceSpinUpdate();
            WinmoneySpinUpdate();
        }
        private void WinmoneySpinUpdate()
        {
            if (NbSlotBaseData.spinType == SpinType.Free && NbSlotBaseData.freeSpinInfo != null)
                return;

            SetForceWinMoneyUpdate(0);
        }

        private void Stop()
        {
            SelectAuto.gameObject.SetActive(false);
            stopPressCount++;
            OnStopPress?.Invoke();
        }

        private IEnumerator CoWaitAutoStop()
        {
            float time = 0f;
            yield return new WaitUntil(() => !SpinBtn.interactable);
            while (!SpinBtn.interactable)
            {
                if (!StopBtn.interactable && !NbSlotBaseData.playReady) yield break;
                yield return null;
                time += Time.deltaTime;
                if (time >= NbDefine.AUTO_STOP_TIME)
                {
                    time = 0;
                    AutoStop();
                    break;
                }
            }
        }

        private void AutoStop()
        {

            if (StopBtn.interactable)
            {
                OnAutoStop?.Invoke();
                StopBtn.interactable = false;
            }
        }

        private void DownBet()
        {
            var isDownBet = NbSlotBaseData.betIndex > 0;
            if (!isDownBet) return;
            SoundSystem.PlayOnce(BottomAudio.Find(x => x.name == BET_DOWN));
            AutoBtn.isOn = false;
            SelectAuto.gameObject.SetActive(false);
            IsBetvChange = true;
            NbSlotBaseData.betIndex -= 1;
            TotalBetUpdate();
            OnDownBet?.Invoke();
        }

        private void UpBet()
        {
            var isUpbet = NbSlotBaseData.betIndex < NbSlotBaseData.betRange.Length - 1;
            if (!isUpbet) return;
            SoundSystem.PlayOnce(BottomAudio.Find(x => x.name == BET_UP));
            AutoBtn.isOn = false;
            SelectAuto.gameObject.SetActive(false);
            IsBetvChange = true;
            NbSlotBaseData.betIndex += 1;
            TotalBetUpdate();
            OnUpBet?.Invoke();
        }

        private void MaxBet()
        {
            if (!IsBottomReady) return;
            AutoBtn.isOn = false;
            SoundSystem.PlayOnce(BottomAudio.Find(x => x.name == MAX_BET));
            SelectAuto.gameObject.SetActive(false);
            NbSlotBaseData.betIndex = NbSlotBaseData.betRange.Length - 1;
            IsBetvChange = true;
            TotalBetUpdate();
            OnMaxBet?.Invoke();
        }

        private void Info()
        {
            OnInfoPress?.Invoke();
        }


        private void BetChangeUpdate()
        {
            if (NbSlotBaseData.betIndex == NbSlotBaseData.betRange.Length - 1)
            {
                UpBtn.interactable = false;
                DownBtn.interactable = true;
            }
            else if (NbSlotBaseData.betIndex == 0)
            {
                DownBtn.interactable = false;
                UpBtn.interactable = true;
            }
            else
            {
                DownBtn.interactable = true;
                UpBtn.interactable = true;
            }
        }

        private void CustomState()
        {
            if (forceState == BtnShowState.Spin)
            {
                StopBtn.gameObject.SetActive(false);
                SpinBtn.gameObject.SetActive(true);
                SpinBtn.interactable = true;
                BetBtnInteractable(true);
                MaxBetBtn.interactable = true;
            }
            else if (forceState == BtnShowState.SpinTurnOff)
            {
                StopBtn.gameObject.SetActive(false);
                SpinBtn.gameObject.SetActive(true);
                StopBtn.interactable = false;
                SpinBtn.interactable = false;
                BetBtnInteractable(false);
                AutoBtn.interactable = false;
            }
            else if (forceState == BtnShowState.Stop)
            {
                SpinBtn.gameObject.SetActive(false);
                StopBtn.gameObject.SetActive(true);
                StopBtn.interactable = true;
                BetBtnInteractable(false);
                AutoBtn.interactable = false;
            }
            else if (forceState == BtnShowState.StopTurnOff)
            {

                AutoBtn.interactable = false;
                SpinBtn.gameObject.SetActive(false);
                StopBtn.gameObject.SetActive(true);
                SpinBtn.interactable = false;
                StopBtn.interactable = false;
                BetBtnInteractable(false);
                MaxBetBtn.interactable = false;
            }
            else if (isAutoSpin || forceState == BtnShowState.Free)
            {
                BetBtnInteractable(false);
                AutoBtn.interactable = false;
                if (forceState == BtnShowState.Free)
                {
                    FSBottomView.Show();
                }
            }
        }

        private void BetBtnInteractable(bool onOff)
        {
            UpBtn.interactable = onOff;
            DownBtn.interactable = onOff;
            MaxBetBtn.interactable = onOff;
            if(NbSlotBaseData.betIndex == 0) {
                DownBtn.interactable = false;
            } else if(NbSlotBaseData.betIndex == NbSlotBaseData.betRange.Length - 1) {
                UpBtn.interactable = false;
            }
        }

        public void SetForceWinMoneyUpdate(long winMoney)
        {
            WinMoneyText.text = $"{winMoney.ToString(NbDefine.NUMBER_FORMAT)} {NbDefine.BET_UNIT_TYPE}";
        }
        public void SetForceBalanceUpdate(long balance) {
            BalanceTxt.text = $"{balance.ToString(NbDefine.NUMBER_FORMAT)} {NbDefine.BET_UNIT_TYPE}";
        }

        public void WinMoneyUpdate()
        {
            long winMoney = 0;
            if (NbSlotBaseData.spinType == SpinType.Free && NbSlotBaseData.freeSpinInfo != null)
            {
                winMoney = NbSlotBaseData.freeSpinInfo.totalWin;
            }
            else if (NbSlotBaseData.spinResponce != null && baseMachine.isStopAll && NbSlotBaseData.spinResponce.winMoney != 0)
            {
                winMoney = NbSlotBaseData.spinResponce.winMoney;
            }

            SetForceWinMoneyUpdate(winMoney);
        }

        private void Update()
        {
            if (baseMachine == null) return;
            if (!IsBottomReady) return;
            if (forceState == BtnShowState.Idle)
            {
                SpinBtn.interactable = baseMachine.isSpinReady;
                AutoBtn.interactable = baseMachine.isSpinReady;
                FSBottomView.Hide();
                if (baseMachine.isSpinReady)
                {
                    StopBtn.gameObject.SetActive(false);
                    SpinBtn.gameObject.SetActive(true);
                    BetBtnInteractable(true);
                }
                else
                {
                    SpinBtn.gameObject.SetActive(false);
                    StopBtn.gameObject.SetActive(true);
                    BetBtnInteractable(false);
                }
            }
            else CustomState();

            if (AutoCnt != 0) AutoStopBtn.gameObject.SetActive(true);
            else AutoStopBtn.gameObject.SetActive(false);

            if (IsBetvChange == true)
            {
                IsBetvChange = false;
                BetChangeUpdate();
            }
            FreeSpinStateForceUpdate();
        }

        private void SetAutoCnt(int cnt)
        {
            SoundSystem.PlayOnce(BottomAudio.Find(x => x.name == AUTO_SPIN));
            isAutoSpin = true;
            AutoCnt = cnt;
            SelectAuto.SetActive(false);
            MaxBetBtn.gameObject.SetActive(false);
            AutoBtn.gameObject.SetActive(false);
            AutoImg.gameObject.SetActive(true);
            AutoCntText.gameObject.SetActive(false);
            infinitImg.gameObject.SetActive(false);
            if (AutoCnt < 0)
            {
                infinitImg.gameObject.SetActive(true);
            }
            else
            {
                AutoCntText.gameObject.SetActive(true);
            }
            StartCoroutine(coAutoPlay());
        }

        private IEnumerator coWaitAutoEndCount()
        {
            AutoCntText.text = AutoCnt.ToString();
            yield return new WaitForSeconds(NbDefine.AUTO_STOP_TIME);
            AutoImg.gameObject.SetActive(false);
            AutoCntText.gameObject.SetActive(false);
            infinitImg.gameObject.SetActive(false);
            AutoBtn.gameObject.SetActive(true);
            MaxBetBtn.gameObject.SetActive(true);
        }


        private IEnumerator coAutoPlay()
        {
            while (isAutoSpin)
            {
                if (AutoCnt != 0 || NbSlotBaseData.spinType == SpinType.Free)
                {
                    yield return null;
                    if (NbSlotBaseData.playReady)
                    {
                        AutoCntUpdate();
                        Spin();
                        FreeCntUpdate();
                        yield return new WaitForSeconds(NbDefine.AUTO_STOP_TIME);
                        AutoStop();
                    }
                }
                else isAutoSpin = false;
            }
        }

        private void FreeSpinStateForceUpdate()
        {
            if (NbSlotBaseData.spinType != SpinType.Free) return;
            AutoBtn.interactable = false;
            MaxBetBtn.interactable = false;
            UpBtn.interactable = false;
            DownBtn.interactable = false;
            FSBottomView.Show();
            if (forceState == BtnShowState.Idle)
            {
                EditorDebug.Log($"BottomView \n현재 스핀 타입은 {NbSlotBaseData.spinType} 입니다. State를 강제로 Free로 바꿉니다.");
                forceState = BtnShowState.Free;
            }
        }

        private void FreeCntUpdate()
        {
            if (NbSlotBaseData.spinType == SpinType.Normal) return;
            FSBottomView.Cnt.text = (NbSlotBaseData.freeSpinInfo.remainCount - 1).ToString();

            if (int.Parse(FSBottomView.Cnt.text) == 0)
            {
                StartCoroutine(coFreeBottomViewOff());
            }
        }

        private IEnumerator coFreeBottomViewOff()
        {
            yield return new WaitUntil(() => baseMachine.isStopAll);
            FSBottomView.Hide();
        }

        private void AutoCntUpdate()
        {
            if (NbSlotBaseData.spinType == SpinType.Free) return;

            if (AutoCnt > 0)
                AutoCnt -= 1;

            if (AutoCnt != 0)
            {
                AutoCntText.text = AutoCnt.ToString();
            }
            else
            {
                if (AutoCnt == 0)
                {
                    StartCoroutine(coWaitAutoEndCount());
                }
                else if (AutoCnt < 0)
                {
                    infinitImg.gameObject.SetActive(true);
                    AutoCntText.gameObject.SetActive(false);
                }
                else
                {
                    AutoCntText.gameObject.SetActive(true);
                    infinitImg.gameObject.SetActive(false);
                }
            }
        }

        //당첨 연출이 필요한 곳에서 스타뚜
        public IEnumerator CoWinBalancePlay()
        {
            AutoStopBtn.interactable = false;
            if (NbSlotBaseData.spinResponce.winType != WinType.Normal)
            {
                var winView = WinPopupView.Find(NbSlotBaseData.spinResponce.winType);
                winView.Show();
                yield return new WaitUntil(() => !winView.IsVisible);
            }

            WinMoneyUpdate();
            BalanceUpdate();
            AutoStopBtn.interactable = true;
            yield return new WaitForSeconds(NbDefine.WIN_MONEY_WAIT_TIME);
        }

        public void SettingBet(int _index)
        {
            if (NbSlotBaseData.betIndex == _index)
                return; 

            int prevBet = NbSlotBaseData.betIndex;

            NbSlotBaseData.betIndex = Math.Min(NbSlotBaseData.betRange.Length - 1,_index);

            if(prevBet > NbSlotBaseData.betIndex)
                SoundSystem.PlayOnce(BottomAudio.Find(x => x.name == BET_UP));
            else
                SoundSystem.PlayOnce(BottomAudio.Find(x => x.name == BET_DOWN));

            AutoBtn.isOn = false;
            SelectAuto.gameObject.SetActive(false);
            IsBetvChange = true;

            TotalBetUpdate();
            OnUpBet?.Invoke();
        }

    }

   
}
