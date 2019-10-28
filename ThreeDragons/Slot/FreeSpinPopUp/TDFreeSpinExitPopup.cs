using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BSS.View;
using UnityEngine.UI;

public class TDFreeSpinExitPopup : BaseView
{
    public Text FreeCnt;
    public Text TotalWinMoney;
    public Button OkBtn;

    public float AutoCloseTime = 4f;

    private void Start()
    {
        OkBtn.onClick.AddListener(Close);
    }

    public override void Show()
    {
        base.Show();
        FreeCnt.text = NbSlotBaseData.freeSpinInfo.initCount.ToString();
        TotalWinMoney.text = NbSlotBaseData.freeSpinInfo.totalWin.ToString("#,###"/*SlotFrameworkDefine.NUMBER_TEXT*/) + " TRX";
    }

    public override void Close()
    {
        base.Close();
    }

    private IEnumerator coAutoClose()
    {
        yield return new WaitForSeconds(AutoCloseTime);
        if (gameObject.activeSelf)
        {
            Close();
        }
    }
}
