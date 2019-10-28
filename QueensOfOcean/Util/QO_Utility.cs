using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QO_Utility : MonoBehaviour
{
    public static Sprite GetSymbolSprite(int reelSetID, int reelIndex)
    {
        int symbolID = reelSetID / 1000;
        int imageID = reelSetID / 10;

        var Symbol = QO_SymbolData.GetSymbol(imageID.ToString());
        return Symbol.Sprite;
    }

    public static List<Vector2Int> FindPositionsInSymbols(Predicate<int> symbolPredicate)
    {
        var list = new List<Vector2Int>();

        for (int i = 0; i < NbSlotBaseData.baseMachine.reels.Count; i++)
            for (int j = 0; j < NbSlotBaseData.baseMachine.slotSize.y; j++)
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

}
