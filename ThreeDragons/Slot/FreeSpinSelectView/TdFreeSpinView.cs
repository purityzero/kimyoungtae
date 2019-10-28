using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BSS;
using Spine.Unity;
using BSS.View;
using NbCommon;

public class TdFreeSpinView : BaseView
{
    public Button FreeMysteryBtn;
    public Button FreeBlueBtn;
    public Button FreeRedBtn;
    public Button FreeGreenBtn;
    public List<int> FreeSpinFactor { get; private set; }
    private int MysterySpinCnt = 0;
    private List<Button> FreeSelectBtnList = new List<Button>();

    private void SelectBtnOnOff(bool Onoff)
    {
        FreeSelectBtnList.ForEach(x => {
            x.interactable = Onoff;
        });
    }

    public override void Show()
    {
        base.Show();
        TdFreeSpinViewController.instance.Init();
        SelectBtnOnOff(true);
    }

    public override void Close()
    {
        TdFreeSpinViewController.instance.ClearAnimationTracks();
        base.Close();
    }

    public void Start()
    {
        FreeSelectBtnList.Add(FreeMysteryBtn);
        FreeSelectBtnList.Add(FreeBlueBtn);
        FreeSelectBtnList.Add(FreeRedBtn);
        FreeSelectBtnList.Add(FreeGreenBtn);

        FreeMysteryBtn.onClick.AddListener(() => FreeButtonSettingAsync(ThreeDragonUtility.FREE_MYSTERY_DRAGON));
        FreeBlueBtn.onClick.AddListener(() => FreeButtonSettingAsync(ThreeDragonUtility.FREE_BLUE_DRAGON));
        FreeRedBtn.onClick.AddListener(() => FreeButtonSettingAsync(ThreeDragonUtility.FREE_RED_DRAGON));
        FreeGreenBtn.onClick.AddListener(() => FreeButtonSettingAsync(ThreeDragonUtility.FREE_GREEN_DRAGON));
    }

    private Button GetDragonButton(int idx)
    {
        if (idx == ThreeDragonUtility.FREE_GREEN_DRAGON) {
            return FreeGreenBtn;
        }  else if(idx == ThreeDragonUtility.FREE_BLUE_DRAGON)  {
            return FreeBlueBtn;
        } else if (idx == ThreeDragonUtility.FREE_RED_DRAGON) {
            return FreeRedBtn;
        } else {
            return FreeMysteryBtn;
        }
    }

    public async void FreeButtonSettingAsync(int btnIndex)
    {
        ThreeDragonManager.instance.DragonLevel = btnIndex;
        
        P_GAME_SELECTSCATTER_RS  rs = await NbNetworkSystem.instance.SelectScatterAsync(btnIndex);
        FreeSpinFactor = rs.factor;
        MysterySpinCnt = rs.cnt;
        if (btnIndex == ThreeDragonUtility.FREE_MYSTERY_DRAGON) {
            MysteryDragonClick();
        } else {
            DrgaonBtnClick(btnIndex);
        }
    }

    public void MysteryDragonClick()
    {
        TdFreeSpinViewController.instance.SetAlphaExptionIndex(ThreeDragonUtility.FREE_MYSTERY_DRAGON, 0.5f);
        GetDragonButton(ThreeDragonUtility.FREE_MYSTERY_DRAGON);
        TdFreeSpinViewController.instance.SpineAnimationChoose(ThreeDragonUtility.FREE_MYSTERY_DRAGON);
        TdFreeSpinViewController.instance.MysteryHideAndFreeGameShow(MysterySpinCnt);
        StartCoroutine(CoSpineMysteryDragonClick());

    }

    public void DrgaonBtnClick(int btnIdx)
    {
        SoundSystem.PlayOnce(ThreeDragonUtility.TD_FREE_SELECT);
        TdFreeSpinViewController.instance.SetAlphaExptionIndex(btnIdx, 0.5f);
        var button = GetDragonButton(btnIdx);
        var spine = TdFreeSpinViewController.instance.SpineAnimationChoose(btnIdx);
        StartCoroutine(CoWaitSpineAnimation(button, spine));
        SelectBtnOnOff(false);
    }


    private IEnumerator CoSpineMysteryDragonClick()
    {
        // 2. Red, 3. Green, 4.Blue

        int randomDragonLv = Random.Range(2, 5);
        ThreeDragonManager.instance.RandomDragonLevel = randomDragonLv;
        StartCoroutine(TdFreeSpinViewController.instance.MysteryAnimation(randomDragonLv));
        SelectBtnOnOff(false);
        yield return new WaitForSeconds(3.0f);
        yield return StartCoroutine(TdFreeSpinViewController.instance.SetMysteryTexts(FreeSpinFactor));
        yield return new WaitForSeconds(5);
        Close();
    }

    private IEnumerator CoWaitSpineAnimation(Button btn, SkeletonGraphic spineAnimation)
    {
        yield return new WaitForSeconds(spineAnimation.SkeletonDataAsset.GetSkeletonData(true).animations.Items[0].duration - 1f);
        spineAnimation.AnimationState.SetAnimation(1, spineAnimation.SkeletonDataAsset.GetSkeletonData(true).animations.Items[1].name, false);
        yield return new WaitUntil(() => spineAnimation.startingAnimation == spineAnimation.SkeletonDataAsset.GetSkeletonData(true).animations.Items[1].name);
        yield return null;
        spineAnimation.freeze = true;
        yield return new WaitForSeconds(5.0f);
        Close();
    }
}
