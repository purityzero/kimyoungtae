using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtension
{
    public static T RandomPop<T>(this List<T> list)
    {
        var getItem = list[Random.Range(0, list.Count)];
        list.Remove(getItem);
        return getItem;
    }

    public static T RandomChoose<T>(this List<T> list)
    {
        var getItem = list[Random.Range(0, list.Count)];
        return getItem;
    }

}
