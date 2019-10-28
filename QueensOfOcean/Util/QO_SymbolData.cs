using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BSS;
using System;

public class QO_SymbolData : PassiveSingleton<QO_SymbolData>
{
    [System.Serializable]
    public class Symbol
    {
        public string ID;
        public Sprite Sprite;
    }

    [System.Serializable]
    public class WildSymbol
    {
        public int multiple;
        public SpriteAnimator spriteRender;

        public void Play()
        {
            spriteRender.Play(spriteRender.animations[0]);
            if (spriteRender.GetComponentInChildren<SpriteAnimator>() != null)
            {
                var multipleAnim = spriteRender.GetComponentInChildren<SpriteAnimator>();
                multipleAnim.Play(multipleAnim.animations[0]);
            }
        }
    }


    [SerializeField]
    private List<Symbol> SymbolList;
    [SerializeField]
    private List<SpriteAnimator> SymbolAnimatiorList;
    [SerializeField]
    private List<WildSymbol> WildSymbolList;

    public static bool ExistSymbolAnimator(string ID)
    {
        return instance.SymbolAnimatiorList.Exists(x => x.name == ID);
    }

    public static SpriteAnimator GetSymbolAnimator(string ID)
    {
        var findSymbolAnim = instance.SymbolAnimatiorList.Find(symbol => symbol.name == ID);
        return findSymbolAnim;
    }

    public static Symbol GetSymbol(SpriteRenderer sprRenderer)
    {
        var symbol = instance.SymbolList.Find(x => x.Sprite == sprRenderer.sprite);
        return symbol;
    }

    public static Symbol GetSymbol(string ID)
    {
        var symbol = instance.SymbolList.Find(x => x.ID == ID);
        return symbol;
    }

    public static WildSymbol GetMWSymobl(int multiple)
    {
        var MWSymbol = instance.WildSymbolList.Find(x => x.multiple == multiple);
        return MWSymbol;
    }

    public static Vector2 CorrectSymbolPosition(string ID)
    {
        Vector2 pos;
        if (ID == "2000")
            pos = new Vector2(-0.07f, +0.35f);
        else if (ID == "7100")
            pos = new Vector2(+0.04f, +0.08f);
        else if (ID == "7200")
            pos = new Vector2(+0.0525f, -0.05f);
        else if (ID == "7300")
            pos = new Vector2(0, +0.025f);
        else if (ID == "7500")
            pos = new Vector2(0f, -0.04f);
        else
            pos = Vector2.zero;
        return pos;
    }

}
