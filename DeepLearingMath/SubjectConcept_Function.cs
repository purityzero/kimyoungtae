using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SubjectConcept
{
    public static class SubjectConcept_Function
    {
        public delegate void FuncDelegate();
        public static FuncDelegate mFuncDelegate;

        public static bool IsFlashEffect = true;
        public static bool isFadeOutStart = true;
        public static bool isFadeInStart = true;

        public static IEnumerator FlashEffect(GameObject obj)
        {
            WaitForSeconds tSce = new WaitForSeconds(0.5f);
            yield return tSce;
            IsFlashEffect = true;
            if (obj.GetComponent<UISprite>() == true)
            {
                for (int i = 0; i < 2; i++)
                {
                    obj.GetComponent<UISprite>().alpha = 0.5f;
                    yield return tSce;
                    obj.GetComponent<UISprite>().alpha = 1;
                    yield return tSce;
                }
                IsFlashEffect = false;
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    obj.GetComponent<UILabel>().alpha = 0.5f;
                    yield return new WaitForSeconds(0.5f);
                    obj.GetComponent<UILabel>().alpha = 1f;
                    yield return new WaitForSeconds(0.5f);
                }
                IsFlashEffect = false;
            }
        }

        public static IEnumerator FlashStop(float sec)
        {
            yield return new WaitForSeconds(sec);
            IsFlashEffect = false;
        }

        public static IEnumerator FadeIn(GameObject obj)
        {
            Debug.Log("Obj" + obj);
            WaitForSeconds tSce = new WaitForSeconds(0.1f);
            if (obj.GetComponent<UISprite>() == true)
            {
                Debug.Log("FadeIn UISprite");
                while (isFadeInStart == true)
                {
                    yield return tSce;
                    obj.GetComponent<UISprite>().alpha = obj.GetComponent<UISprite>().alpha + 0.1f;
                    if (obj.GetComponent<UISprite>().alpha >= 1)
                    {
                        isFadeInStart = false;
                        Debug.Log("FadeIn Complete");
                    }
                }
            }
            else if (obj.GetComponent<UILabel>() == true)
            {
                Debug.Log("FadeIn UILabel");
                while (isFadeInStart == true)
                {
                    yield return tSce;
                    obj.GetComponent<UILabel>().alpha = obj.GetComponent<UILabel>().alpha + 0.1f;
                    if (obj.GetComponent<UILabel>().alpha >= 1)
                    {
                        isFadeInStart = false;
                        Debug.Log("FadeIn Complete");
                    }
                }
            }
            isFadeInStart = true;
        }

        public static IEnumerator FadeOut(GameObject obj)
        {
            Debug.Log("Obj" + obj);
            WaitForSeconds tSce = new WaitForSeconds(0.1f);

            if (IsFlashEffect == true)
            {
                yield return new WaitForSeconds(0.5f);
            }
            if (obj.GetComponent<UISprite>() == true)
            {
                Debug.Log("FadeOut UISprite");
                while (isFadeOutStart == true)
                {
                    yield return tSce;
                    obj.GetComponent<UISprite>().alpha = obj.GetComponent<UISprite>().alpha - 0.1f;
                    if (obj.GetComponent<UISprite>().alpha <= 0)
                    {
                        isFadeOutStart = false;
                        Debug.Log("FadeOut Complete");
                    }
                }
            }
            else if (obj.GetComponent<UILabel>() == true)
            {
                Debug.Log("FadeOut UILabel");
                while (isFadeOutStart == true)
                {
                    yield return tSce;
                    obj.GetComponent<UILabel>().alpha = obj.GetComponent<UILabel>().alpha - 0.1f;
                    if (obj.GetComponent<UILabel>().alpha <= 0)
                    {
                        isFadeOutStart = false;
                        Debug.Log("FadeOut Complete");
                    }
                }
            }
            isFadeOutStart = true;
        }

        //How to use
        //SubjectConcept_Function.mFuncDelegate += Func;
        //StartCoroutine(SubjectConcept_Function.SetDelayFuc(sec, SubjectConcept_Function.mFuncDelegate));
        public static IEnumerator SetDelayFuc(float sec, FuncDelegate func)
        {
            yield return new WaitForSeconds(sec);
            func();
            func = null;
        }

    }
}
