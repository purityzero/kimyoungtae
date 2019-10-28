using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csEnding : MonoBehaviour
{
    AudioSource EndingSong;
    public AudioClip ET; 

    public GameObject CutScene_01;
    public GameObject CutScene_02;
    public GameObject CutScene_03;
    public GameObject CutScene_04;
    public GameObject CutScene_05;
    public GameObject CutScene_06;
    public GameObject CutScene_07;
    public GameObject CutScene_08;
    public GameObject CutScene_09;
    public GameObject CutScene_10;
    public GameObject CutScene_11;
    public GameObject CutScene_12;
    public GameObject CutScene_13;

    public GameObject BtnReset;
    public GameObject BtnAbilReset;

    GameObject loadingCanvas = null;

    // Use this for initialization
    void Start ()
    {
        loadingCanvas = GameObject.Find("loadingCanvas");
        loadingCanvas.SetActive(false);
        EndingSong = GetComponent<AudioSource>();
        
        CutScene_01.SetActive(true);
        CutScene_02.SetActive(false);
        CutScene_03.SetActive(false);
        CutScene_04.SetActive(false);
        CutScene_05.SetActive(false);
        CutScene_06.SetActive(false);
        CutScene_07.SetActive(false);
        CutScene_08.SetActive(false);
        CutScene_09.SetActive(false);
        CutScene_10.SetActive(false);
        CutScene_11.SetActive(false);
        CutScene_12.SetActive(false);
        CutScene_13.SetActive(false);
        BtnReset.SetActive(false);
        BtnAbilReset.SetActive(false);

        EndingSong.Play();

        StartCoroutine("Ending");
    }

    IEnumerator Ending()
    {
        yield return new WaitForSeconds(2.0f);
        CutScene_01.SetActive(false);
        CutScene_02.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        CutScene_02.SetActive(false);
        CutScene_03.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        CutScene_03.SetActive(false);
        CutScene_04.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        CutScene_04.SetActive(false);
        CutScene_05.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        CutScene_05.SetActive(false);
        CutScene_06.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        CutScene_06.SetActive(false);
        CutScene_07.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        CutScene_07.SetActive(false);
        CutScene_08.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        CutScene_08.SetActive(false);
        CutScene_09.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        CutScene_09.SetActive(false);
        CutScene_10.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        CutScene_10.SetActive(false);
        CutScene_11.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        EndingSong.Stop();
        EndingSong.PlayOneShot(ET);
        CutScene_11.SetActive(false);
        CutScene_12.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        CutScene_12.SetActive(false);
        CutScene_13.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        BtnReset.SetActive(true);
        BtnAbilReset.SetActive(true);
    }

    public void btnreset()
    {
        loadingCanvas.SetActive(true);
        Application.LoadLevel("TitleScene");
    }

    public void AbliReset()
    {
        PlayerPrefs.DeleteKey("maxhp");
        PlayerPrefs.DeleteKey("dps");
        PlayerPrefs.DeleteKey("mps");
        PlayerPrefs.DeleteKey("range");
        PlayerPrefs.DeleteKey("multi");

        loadingCanvas.SetActive(true);
        Application.LoadLevel("TitleScene");
    }
}
