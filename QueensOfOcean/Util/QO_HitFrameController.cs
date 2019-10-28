using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlotGame;
using System.Threading.Tasks;
using BSS;
using Sirenix.OdinInspector;

public class QO_HitFrameController : PassiveSingleton<QO_HitFrameController>
{
    [System.Serializable]
    public class HitFrame
    {
        public Vector2Int position;
        public GameObject HitObject;

        public HitFrame(GameObject obj, Vector2Int key)
        {
            HitObject = obj;
            position = key;
        }
    }

    [SerializeField]
    private GameObject HitObjectParent;
    [SerializeField]
    private GameObject HitPrefab;
    [SerializeField] [ReadOnly]
    private List<HitFrame> HitFrameList = new List<HitFrame>();

    public void HitFrameSetting()
    {
        var machine = QO_Manager.instance.BaseMachine;
        for (int i = 0; i < machine.slotSize.x; i++)
        {
            for (int j = 0; j < machine.slotSize.y; j++)
            {
                var key = new Vector2Int(i, j);
                Vector2 pos = machine.GetRender(key).transform.position;
                var hitItem = Instantiate(HitPrefab, HitObjectParent.transform);
                hitItem.transform.position = pos;
                var createHit = new HitFrame(hitItem, key);
                HitFrameList.Add(createHit);
                hitItem.SetActive(false);
            }
        }
    }

    public void HitItemShow(Vector2Int key)
    {
        HitFrameList.Find(x => x.position == key).HitObject.SetActive(true);
    }

    public void HitItemHideAll()
    {
        HitFrameList.ForEach(x => x.HitObject.SetActive(false));
    }
}
