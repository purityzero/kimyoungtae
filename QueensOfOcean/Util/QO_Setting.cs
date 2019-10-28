using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using SlotGame;
using BSS;
using Sirenix.OdinInspector;
using System;

public class QO_Setting : MonoBehaviour
{
    public SpriteMask SlotMask;
    private Machine BaseMachine => QO_Manager.instance.BaseMachine;
    public TweenElement ReelStopTween;

    private IEnumerator Start()
    {
        NbSlotBaseData.baseMachine = Machine.CreateAndInit(new Vector2Int(5, 3), QO_Define.REEL_SPEED, SlotMask.bounds);
        BaseMachine.OnReelRenderUpdated = OnReelRenderUpdated;
        BaseMachine.OnReelStoped = OnReelStoped;
        yield return new WaitUntil(() => NbSlotBaseData.reelSetTable != null);
        BaseMachine.RefreshAll();
    }

    public async static Task InitSetting()
    {
        NbNetworkSystem.instance.Initialize(NbCommon.SMCode.Ocean);

        //문제없을시 내부 코드로 작업 (Smith)
        string deviceId;
#if UNITY_EDITOR
        deviceId = SystemInfo.deviceUniqueIdentifier.ToString();
# elif UNITY_WEBGL
        if (!PlayerPrefs.HasKey("UniqueIdentifier"))
            PlayerPrefs.SetString("UniqueIdentifier", Guid.NewGuid().ToString());
        deviceId = PlayerPrefs.GetString("UniqueIdentifier");
#else
        deviceId = SystemInfo.deviceUniqueIdentifier.ToString();
#endif
        await NbNetworkSystem.instance.LoginAsync(deviceId);

        Debug.Log($"deviceId  : {deviceId.ToString()}");
        Debug.Log($"accidx  : {NbNetworkSystem.instance.account.accIdx.ToString()}");



        await NbNetworkSystem.instance.CheckinAsync();
        await NbNetworkSystem.instance.GameEnterAsync();
    }

    //릴 관련 콜백함수
    private void OnReelRenderUpdated(Reel reel, int reelIndex, SpriteRenderer render, int cursor)
    {
        int reelSet = NbSlotBaseData.reelSetTable.GetReelSet(reelIndex, cursor);
        render.sprite = QO_Utility.GetSymbolSprite(reelSet, reelIndex);
        render.transform.localScale = new Vector2(1.385f, 1.385f);
    }

    private void OnReelStoped(Reel reel, int reelIndex)
    {
        ReelDelayEffecter.instance.ReelCheckStop(reelIndex);
        SoundSystem.PlayOnce(QO_Sound.GetSound(QO_Define.QO_REEL_STOP));
        if (reel.gameObject.GetComponent<TweenPlayer>() == null) {
            reel.gameObject.AddComponent<TweenPlayer>();
        }
        reel.gameObject.GetComponent<TweenPlayer>().Play(ReelStopTween);
        reel.ResetSpeed();
    }
}
