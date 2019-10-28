using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BSS;

public class QO_FlarescapeFlare : MonoBehaviour
{
    private TweenPlayer tweener;

    private void Start()
    {
        tweener = GetComponent<TweenPlayer>();
    }
    void Update()
    {
        if (!tweener.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
