using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BSS;
using BSS.View;

public class QO_FreeSpinEndPopup : BaseView
{
    public Button OKBtn;
    public Text MoneyText;
    public TweenElement FreeSpinPopup;
    public TweenPlayer ImageGroup;

    private void Start()
    {
        OKBtn.onClick.AddListener(Close);
    }

    public override void Show()
    {
        base.Show();
        MoneyText.text = NbSlotBaseData.freeSpinInfo.totalWin.ToString("#,###"/*SlotFrameworkDefine.NUMBER_TEXT*/) + " TRX";
        ImageGroup.Play(FreeSpinPopup);
    }
}
