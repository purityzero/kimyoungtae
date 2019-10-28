using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using System;
using BSS;
using BSS.View;
using Spine;

public class ThreeDragonUtility : PassiveSingleton<ThreeDragonUtility>
{
    public const int FREE_MYSTERY_DRAGON = 1;
    public const int FREE_RED_DRAGON = 2;           // ID 1010
    public const int FREE_BLUE_DRAGON = 4;          // ID 1011
    public const int FREE_GREEN_DRAGON = 3;         // ID 1012

    public const float REEL_SPEED = 3f;
    public const float REEL_DELAY = 0.5f;

    #region SOUND
    public const string TD_INTRO_SOUND = "1_intro";
    public const string TD_MAIN_BGM = "2_main_bgm";
    public const string TD_REEL_STOP = "3_reel stop";
    public const string TD_WIN_COUNT = "4_win_count";
    public const string TD_SCATTER_SYMBOL = "5_scatter symbol";
    public const string TD_BONUS_SYMBOL = "6_jackpot_symbol";
    public const string TD_FREE_GAME_POPUP = "7_free_game_popup";
    public const string TD_FREE_GAME_BGM = "8_free_game_bgm";
    public const string TD_FREE_SELECT_POPUP = "9_free_select_popup";
    public const string TD_FREE_SELECT_BGM = "10_free_select_bgm";
    public const string TD_REEL_ACC = "12_reel acc";
    public const string TD_TOTAL_WIN_POPUP = "13_total_win_popup";
    public const string TD_BONUS_GAME_BGM = "14_bonus_bgm";
    public const string TD_EVENT_GAME_EFFECT_SOUND = "15_free_intro";
    public const string TD_MULTIPLIER_WILD_PRE = "16_multiplier_wild_pre";
    //public const string MULTIPLIER_WILD             = "17_multiplier_wild";               // 사용하지 않음.
    public const string TD_WIN_COUNT_STOP = "18_win_count_stop";
    public const string TD_BONUS_SELECT = "19_jackpot_select";
    public const string TD_PAY_LINE = "20_pay_line";
    public const string TD_FREE_SELECT = "21_free_select";
    public const string TD_REEL_LIGHT_ON = "22_reel_light_on";
    public const string TD_MULTIPLIER_WILD_SETTING = "23_multiplier_wild_setting";
    public const string TD_MULTIPLIER_WILD_FIX = "24_multiplier_wild_fix";
    public const string TD_MULTIPLIER_WILD_COLLECT = "25_multiplier_wild_collect";
    public const string TD_BONUS_REEL_ACC = "bonus_reel_acc";
    public const string TD_DRAGON_REEL_ACC = "reel_acc";
    public const string TD_WIN_BGM = "win_bgm";
    public const string TD_JACK_POT_WIN_POPUP = "13_jackpot_popup";
    public const string TD_JACK_POT_BGM = "15_jackpot_bgm";
    public const string TD_FREE_ALARM = "16_free_alarm";
    #endregion


    private int RED_SYMBOL_ID = 1010;
    private int BLUE_SYMBOL_ID = 1011;
    private int GREEN_SYMBOL_ID = 1012;

    // 함수 더 좋은쪽으로 찾아 봅시다.
    private int ConvertReelOnLv()
    {
        if (ThreeDragonReelOnManager.ReelOnLevel == 0) {
            return 3;
        } else if (ThreeDragonReelOnManager.ReelOnLevel == 1) {
            return 2;
        } else {
            return 1;
        }
    }

    public static List<string> GetMWMultiList(string multiplierStr)
    {
        List<string> multiList;

            if (NbSlotBaseData.spinType == SpinType.Normal)
            {
                List<string> emptyList = new List<string>() { "2", "3", "4", "5", "6", "7", "8", "9", "10" };
                emptyList.Remove(multiplierStr);

                var firstPeek = emptyList.RandomPeek();
                emptyList.Remove(firstPeek);

                var secondPeek = emptyList.RandomPeek();
                emptyList.Remove(secondPeek);

                multiList = new List<string>() { firstPeek, secondPeek, NbSlotBaseData.spinResponce.sumMultiple.ToString() };
            } else  {
                multiList = new List<string>();
                var factorList = ViewUtility.Find<TdFreeSpinView>().FreeSpinFactor;
                factorList.ForEach(x => multiList.Add(x.ToString()));
            }
        return multiList;
    }

    public static bool IsSpineAnim(SpriteRenderer sprRender)
    {
        return ThreeDragonData.instance.SpineSymbolPrefabs.Exists(x => x.name == sprRender.sprite.name);
    }

    public static Sprite GetSymbolSprite(int reelSetID, int reelIndex)
    {
        int symbolID = reelSetID / 1000;
        int imageID = reelSetID / 10;
        if (imageID == 1000 && ThreeDragonManager.instance.DragonLevel > 1)
        {
            if (FREE_RED_DRAGON == ThreeDragonManager.instance.DragonLevel)
            {
                imageID = instance.RED_SYMBOL_ID;
            }
            else if (FREE_BLUE_DRAGON == ThreeDragonManager.instance.DragonLevel)
            {
                imageID = instance.BLUE_SYMBOL_ID;
            }
            else if (FREE_GREEN_DRAGON == ThreeDragonManager.instance.DragonLevel)
            {
                imageID = instance.GREEN_SYMBOL_ID;
            }
        }
        else if (imageID == 1000 && ThreeDragonManager.instance.RandomDragonLevel != 0)
        {
            if (FREE_RED_DRAGON == ThreeDragonManager.instance.RandomDragonLevel)
            {
                imageID = instance.RED_SYMBOL_ID;
            }
            else if (FREE_GREEN_DRAGON == ThreeDragonManager.instance.RandomDragonLevel)
            {
                imageID = instance.GREEN_SYMBOL_ID;
            }
            else if (FREE_BLUE_DRAGON == ThreeDragonManager.instance.RandomDragonLevel)
            {
                imageID = instance.BLUE_SYMBOL_ID;
            }
        }

        if (reelSetID == 10001 && reelIndex == instance.ConvertReelOnLv() && ThreeDragonManager.IsMWEvent)
        {
            if (NbSlotBaseData.spinResponce != null)
            {
                if (NbSlotBaseData.spinResponce.GetMWEvent() != null)  imageID = 1013;
            }
        }
        return ThreeDragonManager.instance.SymbolIcons.Find(x => x.name == imageID.ToString());
    }

    public static IEnumerator ShowMWEffectOnce()
    {
        ThreeDragonData.instance.MWEffect.gameObject.SetActive(true);
        ThreeDragonData.instance.MWEffect.AnimationState.SetAnimation(0, ThreeDragonData.instance.MWEffect.SkeletonDataAsset.GetSkeletonData(true).animations.Items[0].name, false);
        yield return new WaitForSeconds(ThreeDragonData.instance.MWEffect.SkeletonDataAsset.GetSkeletonData(true).animations.Items[0].duration);
        ThreeDragonData.instance.MWEffect.gameObject.SetActive(false);
    }

    public static SkeletonAnimation CreateSpineAnimation(SkeletonAnimation animation, int cnt = 0, bool isLoop = false, Action endAct = null)
    {
        var spine = Instantiate(animation);
        spine.loop = isLoop;
        if (cnt != 0)
        {
            instance.StartCoroutine(instance.coStartSpineRepeat(spine, cnt, endAct));
        }
        else
        {
            endAct?.Invoke();
        }
        return spine;
    }

    private IEnumerator coStartSpineRepeat(SkeletonAnimation animation, int cnt, Action endAct)
    {
        bool isPlaying = true;
        for (int i = 0; i < cnt - 1; i++)
        {
            yield return new WaitForSeconds(animation.SkeletonDataAsset.GetSkeletonData(true).animations.Items[0].duration);
            animation.AnimationState.SetAnimation(0, animation.SkeletonDataAsset.GetSkeletonData(true).animations.Items[0].name, false);
            if (i == cnt - 1) isPlaying = false;
        }
        yield return new WaitUntil(() => !isPlaying);
        endAct?.Invoke();
    }

    public static SkeletonAnimation SpineWinAnim(string name)
    {
        var findSpine = ThreeDragonData.instance.SpineSymbolPrefabs.Find(x => x.name == name);
        var winSpine = CreateSpineAnimation(findSpine);

        return winSpine;
    }

    public static GameObject TweenWinAnim(string name)
    {
        var fineTween = ThreeDragonData.instance.TweenSymbolPrefabs.Find(x => x.name == name);
        var winTween = Instantiate(fineTween);
        return winTween;
    }
}

  
