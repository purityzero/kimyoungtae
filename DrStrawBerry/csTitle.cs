using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class csTitle : MonoBehaviour
{

    public int OpeningScene = 0;
    // 화면에 보이는 것 관련
    public GameObject Title;
    
    public GameObject SettingCanvas;
    public GameObject TilteCanvas;
    public GameObject CutScene_01;
    public GameObject CutScene_02;
    public GameObject CutScene_03;
    public GameObject CutScene_04;
    public GameObject CutScene_05;
    public GameObject CutScene_06;
    public GameObject CutScene_07;
    public GameObject CutScene_08;

    public Text CutScene_06_Text;

    AudioSource Audio;
    public AudioClip ShotSound;
    GameObject loadingCanvas = null;
    GameObject tutorialCanvas = null;
    GameObject tutorialShop = null;
    GameObject tutorialGame = null;

    // 셋팅창 오디오 관련
    int TitleSoundOnOff = 0;

    int CutSceneCount = 0;

    int tutorialCount = 0;
   
    private void Start()
    {
        loadingCanvas = GameObject.Find("loadingCanvas");
        loadingCanvas.SetActive(false);

        tutorialCanvas = GameObject.Find("tutorialCanvas");
        tutorialShop = GameObject.Find("Tutorial_Shop");
        tutorialGame = GameObject.Find("Tutorial_Game");
        tutorialCanvas.SetActive(false);

        OpeningScene = PlayerPrefs.GetInt("OpeningScene");
        Audio = GetComponent<AudioSource>();
        Title.SetActive(false);

        CutScene_01.SetActive(false);
        CutScene_02.SetActive(false);
        CutScene_03.SetActive(false);
        CutScene_04.SetActive(false);
        CutScene_05.SetActive(false);
        CutScene_06.SetActive(false);
        CutScene_07.SetActive(false);
        CutScene_08.SetActive(false);
        // 시작하면 모든 창이 안보인다.
        TilteCanvas.SetActive(false);
        SettingCanvas.SetActive(false);
        // BGM을  플레이 시킬지 여부
        TitleSoundOnOff = PlayerPrefs.GetInt("TitleSoundOnOff", TitleSoundOnOff);
        //Debug.Log(TitleSoundOnOff);
        if (TitleSoundOnOff == 0)
        {
            GetComponent<AudioSource>().Play();
        }
        if (TitleSoundOnOff == 1)
        {
            GetComponent<AudioSource>().Play();
            GetComponent<AudioSource>().Pause();
        }

        StartCoroutine("CutSceneStart");
    }


    public void SceneTrans()
    {
        PlayerPrefs.SetInt("gold", 0);
        //PlayerPrefs.SetInt("gem", 10000);
        loadingCanvas.SetActive(true);
        if(PlayerPrefs.HasKey("firstStart"))
        {
            Application.LoadLevel("ShopScene");
        }
        else
        {
            PlayerPrefs.SetInt("firstStart", 1);
            PlayerPrefs.SetInt("pBegin", 1);
            Application.LoadLevel(2);
        }

    }

    public void SettingsOn()
    {
        SettingCanvas.SetActive(true);
        TilteCanvas.SetActive(false);
    }

    public void SettingBack()
    {
        SettingCanvas.SetActive(false);
        TilteCanvas.SetActive(true);
    }

    // 셋팅창 오디오 토글관련

    public void Play()
    {
        GetComponent<AudioSource>().UnPause();
        PlayerPrefs.SetInt("SoundOnOff", 0);
        PlayerPrefs.SetInt("TitleSoundOnOff", 0);
        //Debug.Log(TitleSoundOnOff);
    }

    public void Stop()
    {
       GetComponent<AudioSource>().Pause();
        PlayerPrefs.SetInt("SoundOnOff",1);
        PlayerPrefs.SetInt("TitleSoundOnOff", 1);
        //Debug.Log(TitleSoundOnOff);
    }

    public void OpeningRestart()
    {
        OpeningScene = 0;
        PlayerPrefs.SetInt("OpeningScene",OpeningScene);
        loadingCanvas.SetActive(true);
        Application.LoadLevel("TitleScene");
    }

    IEnumerator CutSceneStart()
    {
        if (OpeningScene == 0)
        {
            CutSceneCount = 0;
            CutScene_01.SetActive(true);

            while (CutSceneCount < 3)
            {
                CutScene_01.SetActive(false);
                CutScene_02.SetActive(true);
                yield return new WaitForSeconds(0.3f);
                CutScene_01.SetActive(true);
                CutScene_02.SetActive(false);
                yield return new WaitForSeconds(0.3f);
                CutSceneCount++;
            }

            CutScene_01.SetActive(false);
            CutScene_02.SetActive(false);

            CutSceneCount = 0;

            while (CutSceneCount < 3)
            {
                CutScene_03.SetActive(true);
                CutScene_04.SetActive(false);
                yield return new WaitForSeconds(0.3f);
                CutScene_03.SetActive(false);
                CutScene_04.SetActive(true);
                yield return new WaitForSeconds(0.3f);
                CutSceneCount++;
            }

            CutScene_03.SetActive(false);
            CutScene_04.SetActive(false);

            CutSceneCount = 0;

            while (CutSceneCount < 1)
            {
                CutScene_05.SetActive(true);
                yield return new WaitForSeconds(0.9f);
                CutScene_05.SetActive(false);
                CutScene_06.SetActive(true);
                yield return new WaitForSeconds(0.9f);
                if (TitleSoundOnOff == 0)
                {
                    Audio.PlayOneShot(ShotSound);
                }
                CutScene_06_Text.text = "!";
                yield return new WaitForSeconds(0.2f);
                if (TitleSoundOnOff == 0)
                {
                    Audio.PlayOneShot(ShotSound);
                }
                CutScene_06_Text.text = "!!";
                yield return new WaitForSeconds(0.2f);
                if (TitleSoundOnOff == 0)
                {
                    Audio.PlayOneShot(ShotSound);
                }
                CutScene_06_Text.text = "!!!";
                CutSceneCount++;
            }

            CutSceneCount = 0;

            yield return new WaitForSeconds(1.0f);
            CutScene_05.SetActive(false);
            CutScene_06.SetActive(false);

            while (CutSceneCount < 3)
            {
                CutScene_07.SetActive(true);
                CutScene_08.SetActive(false);
                yield return new WaitForSeconds(0.3f);
                CutScene_07.SetActive(false);
                CutScene_08.SetActive(true);
                yield return new WaitForSeconds(0.3f);
                CutSceneCount++;
            }

            TilteCanvas.SetActive(true);
            Title.SetActive(true);

            while (CutSceneCount < 99)
            {
                CutScene_07.SetActive(true);
                CutScene_08.SetActive(false);
                yield return new WaitForSeconds(0.3f);
                CutScene_07.SetActive(false);
                CutScene_08.SetActive(true);
                OpeningScene = 1;
                PlayerPrefs.SetInt("OpeningScene", OpeningScene);
                yield return new WaitForSeconds(0.3f);
            }
        }
        if (OpeningScene == 1)
        {
            TilteCanvas.SetActive(true);
            Title.SetActive(true);
            while (CutSceneCount < 99)
            {
                CutScene_07.SetActive(true);
                CutScene_08.SetActive(false);
                yield return new WaitForSeconds(0.3f);
                CutScene_07.SetActive(false);
                CutScene_08.SetActive(true);
                yield return new WaitForSeconds(0.3f);
            }

        }
    }

    public void openTutorial()
    {
        tutorialCanvas.SetActive(true);
    }

    public void nextCut()
    {
        if(tutorialCount == 0)
        {
            tutorialGame.SetActive(false);
            tutorialCount++;
        }
        else if (tutorialCount == 1)
        {
            tutorialGame.SetActive(true);
            tutorialCount = 0;
            tutorialCanvas.SetActive(false);
        }

    }
}



