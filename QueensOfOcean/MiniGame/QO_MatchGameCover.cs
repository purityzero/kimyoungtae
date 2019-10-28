using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BSS;

public class QO_MatchGameCover : MonoBehaviour
{
    public TweenPlayer ChooseText;
    public TweenElement ChooseAppear;
    public TweenElement ChooseHide;
    public GameObject MatchCover;

    private void Start()
    {
        MatchCover.SetActive(false);
    }

    public void Play()
    {
        MatchCover.SetActive(true);
        StartCoroutine(CoChooseTextAppearAndHide());
    }

    private IEnumerator CoChooseTextAppearAndHide()
    {
        ChooseText.gameObject.SetActive(true);
        ChooseText.Play(ChooseAppear);
        yield return new WaitForSeconds(1f);
        ChooseText.Play(ChooseHide);
        yield return new WaitUntil(() => !ChooseText.isPlaying);
        ChooseText.gameObject.SetActive(false);
        MatchCover.SetActive(false);
    }
}
