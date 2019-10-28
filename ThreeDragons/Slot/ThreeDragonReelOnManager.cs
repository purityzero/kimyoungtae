using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BSS;
using Sirenix.OdinInspector;

public class ThreeDragonReelOnManager : PassiveSingleton<ThreeDragonReelOnManager>
{
    public List<GameObject> ReelOnList;
    public List<Image> DragonSwitchList;
    public Sprite ReelOn;
    public Sprite ReelOff;

    private int betLevel => ThreeDragonManager.instance.BetLevel;
    public static int ReelOnLevel {
        get {
            return instance.SetReelOnLevel();
        }
    }

    private int beforeReelOnLv = 0;

    public void ReelOnUpdate()
    {
        for (int i = 0; i <= DragonSwitchList.Count - 1; i++)
        {
            bool isActive = ReelOnLevel >= i;
            ReelOnOnOff(i, isActive);
            DragonSwitchOnOff(i, isActive);
        }
    }

    private void ReelOnOnOff(int index, bool isOnoff)
    {
        ReelOnList[index].SetActive(isOnoff);
    }

    private void DragonSwitchOnOff(int index, bool isOnoff)
    {
        if (isOnoff) DragonSwitchList[index].sprite = ReelOn;
         else DragonSwitchList[index].sprite = ReelOff;

        DragonSwitchList[index].SetNativeSize();

        if (ReelOnLevel != beforeReelOnLv)
        {
            if (ReelOnLevel > beforeReelOnLv)
            {
                SoundSystem.PlayOnce(ThreeDragonUtility.TD_REEL_LIGHT_ON);
            }
            beforeReelOnLv = ReelOnLevel;
        }
    }

    private int SetReelOnLevel()
    {
        if (betLevel >= 2 && betLevel <= 4)
        {
            return 1;
        }
        else if (betLevel >= 5 && betLevel <= 7)
        {
            return 2;
        }
        else return 0;
    }
}
