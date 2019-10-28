using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BSS.View;
using BSS;
using UnityEngine.UI;

public class QO_FreeSpinStartPopup : BaseView
{
    public Text FreeSpinInitCount;
    public Button OKBtn;
    public TweenElement FreeSpinPopup;
    public TweenPlayer TweenImage;

    private void Start()
    {
        OKBtn.onClick.AddListener(Close);
    }

    public override void Show()
    {
        base.Show();
        FreeSpinInitCount.text = NbSlotBaseData.freeSpinInfo.remainCount.ToString();
        TweenImage.Play(FreeSpinPopup);
        SoundSystem.PlayOnce(QO_Sound.GetSound(QO_Define.QO_FS_POPUP));
    }

    public override void Close()
    {
        base.Close();
    }
}
