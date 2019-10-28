using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BSS;

public class QO_Sound : PassiveSingleton<QO_Sound>
{
    [SerializeField]
    private List<AudioClip> SoundList;

    public static AudioClip GetSound(string name)
    {
        var sound = instance.SoundList.Find(x => x.name == name);
        return sound;
    }
}
