using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BSS;
using Spine.Unity;

public class TdFreeSpinViewController : PassiveSingleton<TdFreeSpinViewController>
{
    [SerializeField]
    private TdFreeSpinViewText TextController;
    [SerializeField]
    private TdFreeSpinViewAnimation AnimationController;

    public void Init()
    {
        TextController.Init();
        AnimationController.Init();
    }

    public void SetAlphaExptionIndex(int index, float alpha)
    {
        AnimationController.SetAlphaAllExceptionIndex(index,alpha);
    }

    public SkeletonGraphic SpineAnimationChoose(int idx)
    {
        return AnimationController.StartChooseAnimation(idx);
    }

    public void SpineAllHide()
    {
        AnimationController.AllHide();
    }

    public void SpineAllShow()
    {
        AnimationController.AllShow();
    }

    public void MysteryHideAndFreeGameShow(int spinCnt)
    {
        TextController.MysteryTextSet(spinCnt.ToString());
        TextController.MysteryFreeGameFadeIn();
        TextController.MysteryChoiceFadeOut();
        StartCoroutine(TextController.CoMysteryTextFadeOut());
    }

    public IEnumerator SetMysteryTexts(List<int> multiplierList)
    {
        yield return StartCoroutine(TextController.SetMysteryMultipleText(multiplierList));
    }

    /// <summary>
    /// 미스테리 드래곤 애니메이션을 실행하면서, 자식을 생성하여 나올 드래곤을 결정 함.
    /// </summary>
    /// <param name="idx"> TdUtility에 있는 드래곤 상수 값을 넣어주세요.</param>
    /// <returns></returns>
    public IEnumerator MysteryAnimation(int idx)        // 2. Red 3. Green 4. blue 
    {
        string spineName;
        if (idx == 2) {
            spineName = "Red";
        } else if (idx == 3) {
            spineName = "Green";
        } else {
            spineName = "Blue";

        }
        yield return StartCoroutine(AnimationController.CoMysteryBtnSpineChange(spineName));
    }

    public void ClearAnimationTracks()
    {
        AnimationController.SpineList.ForEach(x => x.AnimationState.ClearTracks());
    }

}
