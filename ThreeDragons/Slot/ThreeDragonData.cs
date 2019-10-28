using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using BSS;
using System.Threading.Tasks;

public class ThreeDragonData : PassiveSingleton<ThreeDragonData>
{
    public SkeletonGraphic BonusIn;
    public SkeletonAnimation MWEffect;
    public List<Sprite> SymbolIcons;
    public List<SkeletonAnimation> SpineSymbolPrefabs;
    public List<GameObject> TweenSymbolPrefabs;

}
