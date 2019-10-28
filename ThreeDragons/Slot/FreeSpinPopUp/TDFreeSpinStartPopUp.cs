using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BSS.View;
using SlotGame;

public class TDFreeSpinStartPopUp : BaseView
{
    public Text FreeCnt;
    public Button OkBtn;

    public float AutoCloseTime = 4f;

    private void Start() {
        OkBtn.onClick.AddListener(Close);
    }

    public override void Show()
    {
        base.Show();
        FreeCnt.text = NbSlotBaseData.freeSpinInfo.initCount.ToString();
    }

    public override void Close()
    {
        base.Close();
    }

    private IEnumerator coAutoClose()
    {
        yield return new WaitForSeconds(AutoCloseTime);
        if(gameObject.activeSelf)
        {
            Close();
        }
    }
}
