using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SubjectConcept
{
    public class Age8_12Weeks_Btn : MonoBehaviour
    {
        private Age8_12Weeks_Ep1 UIManager;
        public void Start()
        {
            UIManager = gameObject.GetComponentInParent<Age8_12Weeks_Ep1>();
        }
        public void OnPress(bool pressed)
        {
            if (pressed == true)
            {

            }
            else
            {
                SubjectConcept_Function.IsFlashEffect = false;
                gameObject.GetComponent<UISprite>().enabled = true;
                gameObject.GetComponent<BoxCollider>().enabled = false;
                gameObject.transform.DOShakePosition(5, 10, 10, 90, false, true).OnComplete(() =>
                    {
                        gameObject.GetComponent<UISprite>().spriteName = "Kangaroo_02";
                        gameObject.GetComponent<UISprite>().SetDimensions(142, 88);
                        gameObject.transform.localPosition = new Vector2(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y -20);
                        StartCoroutine(FadeIn());
                        UIManager.FadeOutTextureMask();
                    });

            }
        }

        public IEnumerator FadeIn()
        {
            for (int i = 0; i < UIManager.Slot.GetLength(0); i++)
            {
                yield return new WaitForSeconds(1.0f);
                StartCoroutine(SubjectConcept_Function.FadeIn(UIManager.Slot[i]));
            }

        }
    }
}
