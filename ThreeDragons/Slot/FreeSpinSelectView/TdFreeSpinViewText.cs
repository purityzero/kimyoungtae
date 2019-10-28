using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BSS;

[System.Serializable]
public class SpinCntText
{
    public Text text;
    public Image image;

    public void SHow()
    {
        text.gameObject.SetActive(true);
        image.gameObject.SetActive(true);
    }

    public void Hide()
    {
        text.gameObject.SetActive(false);
        image.gameObject.SetActive(false);
    }
}

public class TdFreeSpinViewText : MonoBehaviour
{
    public SpinCntText Blue;
    public SpinCntText Red;
    public SpinCntText Green;
    public SpinCntText Mystery;
    public Image MysteryChoice;
    public Text MysteryText;

    public TweenElement FreePopupFadeIn;
    public TweenElement FreePopupFadeOut;


    private void Start()
    {
        StartCoroutine(CoMysteryTextFadeOut());
    }

    public void Init()
    {
        MysteryChoice.gameObject.SetActive(true);

        Mystery.text.text = "99";
        Mystery.text.color = new Color(Mystery.text.color.r, Mystery.text.color.g, Mystery.text.color.b, 0.01f);
        MysteryText.text = "X?, X? or X?";
    }

    public void MysteryTextSet(string spinCnt)
    {
        Mystery.text.text = spinCnt;
    }

    public void MysteryChoiceFadeOut()
    {
        var mysterychoice = MysteryChoice.GetComponent<TweenPlayer>();
        mysterychoice.Play(FreePopupFadeOut);
        StartCoroutine(coChoiceFadeOut());
    }

    private IEnumerator coChoiceFadeOut()
    {
        yield return new WaitUntil(() => !MysteryChoice.GetComponent<TweenPlayer>().isPlaying);
        MysteryChoice.gameObject.SetActive(false);
    }

    public void MysteryFreeGameFadeIn()
    {

        Mystery.image.gameObject.SetActive(true);
        Mystery.text.gameObject.SetActive(true);

        var mystery = Mystery.image.GetComponent<TweenPlayer>();
        mystery.Play(FreePopupFadeIn);
    }

    public IEnumerator SetMysteryMultipleText(List<int> multiplierList)
    {
        MysteryText.text = $"X{multiplierList[0]}, X? or X?";
        yield return new WaitForSeconds(1.0f);
        MysteryText.text = $"X{multiplierList[0]}, X{multiplierList[1]} or X?";
        yield return new WaitForSeconds(1.0F);
        MysteryText.text = $"X{multiplierList[0]}, X{multiplierList[1]} or X{multiplierList[2]}";
    }

    public IEnumerator CoMysteryTextFadeOut()
    {
        while (Mystery.text.color.a < 1) {
            yield return null;
            Mystery.text.color = new Color(Mystery.text.color.r, Mystery.text.color.g, Mystery.text.color.b, Mystery.text.color.a + 0.03f);
        }
    }

    


}
