using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using BSS;
using BSS.View;
using System.Linq;

public class TdFreeSpinViewAnimation : MonoBehaviour
{
    public SkeletonGraphic SpineRed;
    public SkeletonGraphic SpineBlue;
    public SkeletonGraphic SpineGreen;
    public SkeletonGraphic SpineMystery;

    //[HideInInspector]
    public List<SkeletonGraphic> SpineList = new List<SkeletonGraphic>();

    public TweenElement MsteryEffect;

    private void Start()
    {
        SpineList.Add(SpineRed);
        SpineList.Add(SpineBlue);
        SpineList.Add(SpineGreen);
        SpineList.Add(SpineMystery);
    }

    public void Init()
    {
        AllShow();
        AllFreezeOff();
        AllStartingLoop();

        SpineMystery.transform.Rotate(Vector3.zero);
        SpineBlue.AnimationState.SetAnimation(0, SpineBlue.SkeletonDataAsset.GetSkeletonData(true).Animations.Items[1].name, true);
        SpineRed.AnimationState.SetAnimation(0, SpineRed.SkeletonDataAsset.GetSkeletonData(true).Animations.Items[1].name, true);
        SpineGreen.AnimationState.SetAnimation(0, SpineGreen.SkeletonDataAsset.GetSkeletonData(true).Animations.Items[1].name, true);
        SpineMystery.AnimationState.SetAnimation(0, SpineMystery.SkeletonDataAsset.GetSkeletonData(true).Animations.Items[0].name, true);
        SpineList.ForEach(x => x.color = new Color(1, 1, 1, 1));
    }

    public SkeletonGraphic GetChooseAnimation(int idx)
    {
        if (idx == ThreeDragonUtility.FREE_BLUE_DRAGON) {
            return SpineBlue;
        }
        else if (idx == ThreeDragonUtility.FREE_GREEN_DRAGON) {
            return SpineGreen;
        }
        else if (idx == ThreeDragonUtility.FREE_RED_DRAGON) {
            return SpineRed;
        }
        else {
            return SpineMystery;
        }
    }

    /// <summary>
    /// 인덱스를 제외한 모든 알파값을 조정 합니다.
    /// </summary>
    /// <param name="index">ThreeDragonUtility 상수 값</param>
    public void SetAlphaAllExceptionIndex(int index, float alpha)
    {
        SpineList.ForEach(x =>
        {
            if (x != GetChooseAnimation(index)) x.color = new Color(1,1,1, alpha);
        });
    }

    public SkeletonGraphic StartChooseAnimation(int idx)
    {
        var spine = GetChooseAnimation(idx);
        spine.AnimationState.SetAnimation(0, spine.SkeletonDataAsset.GetSkeletonData(true).Animations.Items[0].name, true);
        return spine;
    }

    public void AllShow()
    {
        SpineMystery.gameObject.SetActive(true);
        SpineGreen.gameObject.SetActive(true);
        SpineRed.gameObject.SetActive(true);
        SpineBlue.gameObject.SetActive(true);
    }

    public void AllHide()
    {
        SpineMystery.gameObject.SetActive(false);
        SpineGreen.gameObject.SetActive(false);
        SpineRed.gameObject.SetActive(false);
        SpineBlue.gameObject.SetActive(false);
    }

    public IEnumerator CoMysteryBtnSpineChange(string spineName)
    {
        SpineMystery.gameObject.GetComponent<TweenPlayer>().Play(MsteryEffect);
        yield return new WaitUntil(() => SpineMystery.transform.rotation.y > 0.6f);

        var obj = Instantiate(SpineList.Find(x => x.name == spineName).gameObject, SpineMystery.transform);
        obj.GetComponent<SkeletonGraphic>().color = Color.white;
        obj.transform.position = SpineMystery.transform.position;
        Destroy(obj.GetComponentInChildren<Text>().gameObject);
        obj.transform.localScale = new Vector2(1, 1);

        var MysteryTextList = SpineMystery.GetComponentsInChildren<Text>().ToList();
        MysteryTextList.ForEach(x => x.gameObject.transform.SetAsLastSibling());
        yield return new WaitUntil(() => SpineMystery.transform.rotation.y == 0f);
        var spineObj = obj.GetComponent<SkeletonGraphic>();
        spineObj.AnimationState.SetAnimation(0, spineObj.SkeletonDataAsset.GetSkeletonData(true).Animations.Items[0].name, true);
        yield return new WaitForSeconds(spineObj.SkeletonDataAsset.GetSkeletonData(true).Animations.Items[0].duration -1f);
        spineObj.AnimationState.SetAnimation(0, spineObj.SkeletonDataAsset.GetSkeletonData(true).Animations.Items[1].name, true);
        yield return new WaitUntil(() => spineObj.startingAnimation == spineObj.SkeletonDataAsset.GetSkeletonData(true).Animations.Items[1].name);
        yield return null;
        spineObj.freeze = true;
        yield return new WaitUntil(() => !ViewUtility.Find<TdFreeSpinView>().IsVisible);
        Destroy(obj);
    }

    private void AllFreezeOff()
    {
        SpineList.ForEach(x => x.freeze = false);
    }

    private void AllStartingLoop()
    {
        SpineList.ForEach(x => x.startingLoop = true);
    }
}
