using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SubjectConcept
{
    public class Age8_12Weeks_Ep1 : LearningUI
    {
        public GameObject KangarooBtn;
        public GameObject[] Slot;
        [HideInInspector]
        public bool IsGameStart;


        public void Execute()
        {
            Setting();
        }

        private void Setting()
        {
            IsGameStart = true;
            SetUIMedalCount(0);
            SetTopUI("1일차", "덧셈과 뺄셈", null, "뽑기 공을 눌러서 공을 열어 보세요.");
            KangarooBtn.GetComponent<UISprite>().spriteName = "Kangaroo_01";
            KangarooBtn.GetComponent<UISprite>().SetDimensions(76, 106);
            KangarooBtn.transform.localPosition = new Vector2(-400, 0);
            KangarooBtn.GetComponent<BoxCollider>().enabled = true;

            for (int i = 0; i < Slot.GetLength(0); i++)
            {
                if (Slot[i].GetComponent<UISprite>() == true)
                {
                    if (i != 0)
                    {
                        Slot[i].GetComponent<UISprite>().alpha = 0.001f;
                    }
                }
                else
                {
                    Slot[i].GetComponent<UILabel>().alpha = 0.001f;
                }
            }
            StartCoroutine(SubjectConcept_Function.FlashEffect(KangarooBtn));
            StartCoroutine(SubjectConcept_Function.FlashStop(4.0f));
        }

        public void FadeOutTextureMask()
        {
            SetTopUI("1일차", "덧셈과 뺄셈", null, "인형 0개와 3개를 더하면 모두 3개입니다.");
        }

        public override void TopUIPushBackBtn()
        {
            IsGameStart = false;
            gameObject.GetComponentInParent<Age9_10Weeks_WeeksManager>().LearningBackPush();
            gameObject.SetActive(false);
        }
    }
}

