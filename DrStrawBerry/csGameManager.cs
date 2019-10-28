/* 2017.01.20 
 * by HG17
 * UI 로직
 * 2017.02.01
 * by 꿀빵
 * 재화 로직 수정
 * 2017.02.05
 * KBS 구글 업적 연동 수정
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class csGameManager : MonoBehaviour
{
    GameObject soundmanager = null;

    int Gold;
    public int SceneMinGold = 5;
    public int SceneMaxGold = 12;
    int Gem;
    public int SceneMinGem = 1;
    public int SceneMaxGem = 3;

    public int meleeInStage;
    public int towerInStage;
    public int rangeInStage;
    public int objectInStage;
    public int bossInStage;

    public GameObject[] meleeEnemies;
    public GameObject[] towerEnemies;
    public GameObject[] rangeEnemies;
    public GameObject[] objects;
    public GameObject[] bossEnemies;

    public Transform[] MmobPoints;
    public Transform[] TmobPoints;
    public Transform[] RmobPoints;
    public Transform[] ObjectPoints;
    public Transform[] BossmobPoints;

    GameObject gameOver_Window = null;
    GameObject btnRestart = null;
    GameObject txtGameOver = null;
    GameObject Player = null;
    GameObject loadingCanvas = null;
    GameObject portal = null;

    Text panelty_Text = null;
    Text txtGold = null;
    Text txtGem = null;
    Text gold_Board;
    Text gem_Board;
    Text txtClear = null;

    int sceneNum = 0;
    int enemyCount = 0;
    bool donotEnter = false;
    bool access = true;

    int gold;
    int gem;

    // SMOOTH SCENE TRANSLATE
    GUITexture Black_screen;
    public float Fade_Time = 2f;
    private float Fade_Max = 0.5f;
    private float _time;
    private bool FadeIn_ing = true;
    private bool FadeOut_ing;
    
    // 랜덤 배치 로직 
    void Awake()
    {
        int randomSeeds;
        randomSeeds = (int)System.DateTime.Now.Ticks;
        Random.seed = randomSeeds;

        if (meleeInStage != 0)
        {
            int[] alreadyIndex = new int[meleeInStage];

            for (int i = 0; i < meleeInStage; i++)
            {
                int spawnIndex;
                int monsterIndex = Random.Range(1, meleeEnemies.Length + 1);

                while (true)
                {
                    spawnIndex = Random.Range(1, MmobPoints.Length + 1);
                    alreadyIndex[i] = spawnIndex;
                    int breakLoop = 0;

                    for (int j = 0; j <= i; j++)
                    {
                        if (alreadyIndex[j] == spawnIndex)
                        {
                            break;
                        }
                        else
                        {
                            breakLoop++;
                        }

                    }

                    if (breakLoop >= i)
                        break;
                }
                Instantiate(meleeEnemies[monsterIndex - 1], MmobPoints[spawnIndex - 1].transform.position, MmobPoints[spawnIndex - 1].transform.rotation);
            }
        }

        if (towerInStage != 0)
        {
            int[] alreadyIndex = new int[towerInStage];

            for (int i = 0; i < towerInStage; i++)
            {
                int spawnIndex;
                int monsterIndex = Random.Range(1, towerEnemies.Length + 1);

                while (true)
                {
                    spawnIndex = Random.Range(1, TmobPoints.Length + 1);
                    alreadyIndex[i] = spawnIndex;
                    int breakLoop = 0;

                    for (int j = 0; j <= i; j++)
                    {
                        if (alreadyIndex[j] == spawnIndex)
                        {
                            break;
                        }
                        else
                        {
                            breakLoop++;
                        }

                    }

                    if (breakLoop >= i)
                        break;
                }
                Instantiate(towerEnemies[monsterIndex - 1], TmobPoints[spawnIndex - 1].transform.position, TmobPoints[spawnIndex - 1].transform.rotation);
            }
        }

        if (rangeInStage != 0)
        {
            int[] alreadyIndex = new int[rangeInStage];

            for (int i = 0; i < rangeInStage; i++)
            {
                int spawnIndex;
                int monsterIndex = Random.Range(1, rangeEnemies.Length + 1);

                while (true)
                {
                    spawnIndex = Random.Range(1, RmobPoints.Length + 1);
                    alreadyIndex[i] = spawnIndex;
                    int breakLoop = 0;

                    for (int j = 0; j <= i; j++)
                    {
                        if (alreadyIndex[j] == spawnIndex)
                        {
                            break;
                        }
                        else
                        {
                            breakLoop++;
                        }

                    }

                    if (breakLoop >= i)
                        break;
                }
                Instantiate(rangeEnemies[monsterIndex - 1], RmobPoints[spawnIndex - 1].transform.position, RmobPoints[spawnIndex - 1].transform.rotation);
            }
        }

        if (objectInStage != 0)
        {
            int[] alreadyIndex = new int[objectInStage];

            for (int i = 0; i < objectInStage; i++)
            {
                int spawnIndex;
                int objectIndex = Random.Range(1, objects.Length + 1);

                while (true)
                {
                    spawnIndex = Random.Range(1, ObjectPoints.Length + 1);
                    alreadyIndex[i] = spawnIndex;
                    int breakLoop = 0;

                    for (int j = 0; j <= i; j++)
                    {
                        if (alreadyIndex[j] == spawnIndex)
                        {
                            break;
                        }
                        else
                        {
                            breakLoop++;
                        }

                    }

                    if (breakLoop >= i)
                        break;
                }
                Instantiate(towerEnemies[objectIndex - 1], ObjectPoints[spawnIndex - 1].transform.position, ObjectPoints[spawnIndex - 1].transform.rotation);
            }
        }

        if (bossInStage != 0)
        {
            int bossIndex = Random.Range(0, bossEnemies.Length);

            Instantiate(bossEnemies[bossIndex], BossmobPoints[0].transform.position, BossmobPoints[0].transform.rotation);
        }
    }

    void Start()
    {
        gem = PlayerPrefs.GetInt("gem");
        gold = PlayerPrefs.GetInt("gold");

        soundmanager = GameObject.Find("Sound_PopUpManager");

        Black_screen = GetComponent<GUITexture>();

        loadingCanvas = GameObject.Find("loadingCanvas");
        loadingCanvas.SetActive(false);
        gameOver_Window = GameObject.Find("GameOverWindow");
        btnRestart = GameObject.Find("btnRestart");
        txtGameOver = GameObject.Find("txtGameOver");
        txtClear = GameObject.Find("txtClear").GetComponent<Text>();
        gold_Board = GameObject.Find("Goldtxt").GetComponent<Text>();
        gem_Board = GameObject.Find("Gemtxt").GetComponent<Text>();
        panelty_Text = GameObject.Find("Panelty_Text").GetComponent<Text>();
        txtGold = GameObject.Find("txtGold").GetComponent<Text>();
        txtGem = GameObject.Find("txtGem").GetComponent<Text>();
        Player = GameObject.Find("Player");
        portal = GameObject.Find("Portal");

        gameOver_Window.SetActive(false);
        //btnRestart.SetActive(false);
        //txtGameOver.SetActive(false);
        txtClear.gameObject.SetActive(false);
        portal.SetActive(false);

        /* 업적 관련 (2017.02.13 - KBS)*/
        sceneNum = Application.loadedLevel;
        if(sceneNum == 2)
        {
            string unLock_id = "CgkIvs7igpgJEAIQAQ";
            Social.ReportProgress(unLock_id, 100.0f, (bool success) => { });
        }
        else if(sceneNum == 6)
        {
            string unLock_id = "CgkIvs7igpgJEAIQAg";
            Social.ReportProgress(unLock_id, 100.0f, (bool success) => { });
        }
        else if (sceneNum == 10)
        {
            string unLock_id = "CgkIvs7igpgJEAIQAw";
            Social.ReportProgress(unLock_id, 100.0f, (bool success) => { });
        }
        else if (sceneNum == 14)
        {
            string unLock_id = "CgkIvs7igpgJEAIQBA";
            Social.ReportProgress(unLock_id, 100.0f, (bool success) => { });
        }
        else if (sceneNum == 18)
        {
            string unLock_id = "CgkIvs7igpgJEAIQBQ";
            Social.ReportProgress(unLock_id, 100.0f, (bool success) => { });
        }
        else if (sceneNum == 22)
        {
            string unLock_id = "CgkIvs7igpgJEAIQBg";
            Social.ReportProgress(unLock_id, 100.0f, (bool success) => { });
        }

        if(PlayerPrefs.HasKey("kill"))
        {
            if(PlayerPrefs.GetInt("kill")>=10)
            {
                string unLock_id = "CgkIvs7igpgJEAIQBw";
                Social.ReportProgress(unLock_id, 100.0f, (bool success) => { });
            }
            if (PlayerPrefs.GetInt("kill") >= 30)
            {
                string unLock_id = "CgkIvs7igpgJEAIQCA";
                Social.ReportProgress(unLock_id, 100.0f, (bool success) => { });
            }
            if (PlayerPrefs.GetInt("kill") >= 50)
            {
                string unLock_id = "CgkIvs7igpgJEAIQCQ";
                Social.ReportProgress(unLock_id, 100.0f, (bool success) => { });
            }
            if (PlayerPrefs.GetInt("kill") >= 80)
            {
                string unLock_id = "CgkIvs7igpgJEAIQCg";
                Social.ReportProgress(unLock_id, 100.0f, (bool success) => { });
            }
            if (PlayerPrefs.GetInt("kill") >= 100)
            {
                string unLock_id = "CgkIvs7igpgJEAIQCw";
                Social.ReportProgress(unLock_id, 100.0f, (bool success) => { });
            }
        }
        else
        {
            PlayerPrefs.SetInt("kill", 0);
        }

        if (PlayerPrefs.GetInt("gold") >= 50)
        {
            string unLock_id = "CgkIvs7igpgJEAIQDA";
            Social.ReportProgress(unLock_id, 100.0f, (bool success) => { });
        }
        else if (PlayerPrefs.GetInt("gold") >= 100)
        {
            string unLock_id = "CgkIvs7igpgJEAIQDQ";
            Social.ReportProgress(unLock_id, 100.0f, (bool success) => { });
        }
        else if (PlayerPrefs.GetInt("gold") >= 200)
        {
            string unLock_id = "CgkIvs7igpgJEAIQDg";
            Social.ReportProgress(unLock_id, 100.0f, (bool success) => { });
        }
        else if (PlayerPrefs.GetInt("gold") >= 500)
        {
            string unLock_id = "CgkIvs7igpgJEAIQDw";
            Social.ReportProgress(unLock_id, 100.0f, (bool success) => { });
        }

        if(PlayerPrefs.HasKey("death"))
        {
            if(PlayerPrefs.GetInt("death")==1)
            {
                string unLock_id = "CgkIvs7igpgJEAIQEA";
                Social.ReportProgress(unLock_id, 100.0f, (bool success) => { });
            }
            else if(PlayerPrefs.GetInt("death")==10)
            {
                string unLock_id = "CgkIvs7igpgJEAIQEQ";
                Social.ReportProgress(unLock_id, 100.0f, (bool success) => { });
            }
            else if (PlayerPrefs.GetInt("death") == 15)
            {
                string unLock_id = "CgkIvs7igpgJEAIQEg";
                Social.ReportProgress(unLock_id, 100.0f, (bool success) => { });
            }
            else if (PlayerPrefs.GetInt("death") == 20)
            {
                string unLock_id = "CgkIvs7igpgJEAIQEw";
                Social.ReportProgress(unLock_id, 100.0f, (bool success) => { });
            }
            else if (PlayerPrefs.GetInt("death") == 50)
            {
                string unLock_id = "CgkIvs7igpgJEAIQFA";
                Social.ReportProgress(unLock_id, 100.0f, (bool success) => { });
            }
        }

        else
        {
            PlayerPrefs.SetInt("death", 0);
        }

        if(sceneNum==17&& PlayerPrefs.GetInt("hp") == PlayerPrefs.GetInt("maxhp"))
        {
            string unLock_id = "CgkIvs7igpgJEAIQFw";
            Social.ReportProgress(unLock_id, 100.0f, (bool success) => { });
        }

        if(PlayerPrefs.GetInt("multi")==5)
        {
            string unLock_id = "CgkIvs7igpgJEAIQGA";
            Social.ReportProgress(unLock_id, 100.0f, (bool success) => { });
        }

        if(PlayerPrefs.GetInt("maxhp")>=6)
        {
            string unLock_id = "CgkIvs7igpgJEAIQGQ";
            Social.ReportProgress(unLock_id, 100.0f, (bool success) => { });
        }

        if(PlayerPrefs.HasKey("shield"))
        {
            if(PlayerPrefs.GetInt("shield")==20)
            {
                string unLock_id = "CgkIvs7igpgJEAIQGg";
                Social.ReportProgress(unLock_id, 100.0f, (bool success) => { });
            }
        }
        else
        {
            PlayerPrefs.SetInt("shield", 0);
        }

        if(PlayerPrefs.HasKey("adv_time"))
        {
            if(PlayerPrefs.GetInt("adv_time")>=10)
            {
                string unLock_id = "CgkIvs7igpgJEAIQGw";
                Social.ReportProgress(unLock_id, 100.0f, (bool success) => { });
            }
        }

        else
        {
            PlayerPrefs.SetInt("adv_time", 0);
        }

        if(PlayerPrefs.HasKey("rnd_down"))
        {
            if(PlayerPrefs.GetInt("rnd_down")>=1)
            {
                string unLock_id = "CgkIvs7igpgJEAIQFQ";
                Social.ReportProgress(unLock_id, 100.0f, (bool success) => { });
            }
        }
        else
        {
            PlayerPrefs.SetInt("rnd_down", 0);
        }
        enemyCount = meleeInStage + rangeInStage + towerInStage + bossInStage;

        txtGold.text = gold.ToString();
        txtGem.text = gem.ToString();
    }

    void Update()
    {
        if (gold_Board.IsActive() == true)
        {
            gold_Board.text = GameObject.Find("txtGold").GetComponent<Text>().text;
        }
        if(gem_Board.IsActive()==true)
        {
            gem_Board.text = PlayerPrefs.GetInt("gem").ToString();
        }
        if (enemyCount == 0 && donotEnter == false)
        {
            txtClear.gameObject.SetActive(true);
            donotEnter = true;
            portal.SetActive(true);
            txtClear.text = "<color=#000000ff>" + "<b>" + "포탈로 이동하세요." + "</b>" + "</color>";
            StartCoroutine("Clear");
        }

        if (FadeIn_ing)
        {
            _time += Time.deltaTime;
            Black_screen.color = Color.Lerp(new Color(0, 0, 0, Fade_Max), new Color(0, 0, 0, 0), _time / Fade_Time);
        }

        if (FadeOut_ing)
        {
            _time += Time.deltaTime;
            Black_screen.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, Fade_Max), _time / Fade_Time);
        }

        if (_time >= Fade_Time)
        {
            _time = 0;
            FadeIn_ing = false;
            FadeOut_ing = false;
        }
    }

    public void RestartScene()
    {
        FadeOut_ing = true;
        sceneNum = 0;
        PlayerPrefs.DeleteKey("hp");
        StartCoroutine(TransScene(sceneNum, Fade_Time));
    }

    public void NextScene()
    {
        csPlayerHP hpscript = Player.GetComponent<csPlayerHP>();
        //csPlayerController controll_script = Player.GetComponent<csPlayerController>();
        //PlayerPrefs.SetFloat("mps", controll_script.walkSpeed);

        // 게임씬 맨 마지막에 도달했을 때
        if (sceneNum + 2 == Application.levelCount)
        {
            StartCoroutine(TransScene(sceneNum+1, Fade_Time));
        }

        if (donotEnter && access)
        {
            access = false;
            csSetting script1 = soundmanager.GetComponent<csSetting>();
            script1.waypointsound();
            FadeOut_ing = true;
            
            // 보스맵일경우
            if(bossInStage >= 1 )
            {
                Debug.Log("in bossmap");
                int loadEnd = PlayerPrefs.GetInt("pEnd");
                loadEnd++;
                PlayerPrefs.SetInt("pEnd", loadEnd);
                PlayerPrefs.SetInt("savescene", sceneNum + 1);
                PlayerPrefs.SetInt("gold", gold);
                StartCoroutine(TransScene(1, Fade_Time));
            }

            else
            {
                PlayerPrefs.SetInt("hp", hpscript.hp);
                PlayerPrefs.SetInt("gold", gold);
                StartCoroutine(TransScene(sceneNum + 1, Fade_Time));
            }
        }
    }

    //ShopScene & TitleScene 2017.02.07 KBS
    public void ShopScene()
    {
        FadeOut_ing = true;
        Time.timeScale = 1;
        //PlayerPrefs.SetInt("gold", 0);
        StartCoroutine(TransScene(1, Fade_Time));
    }

    public void TitleScene()
    {
        FadeOut_ing = true;
        Time.timeScale = 1;
        //PlayerPrefs.SetInt("gold", 0);
        StartCoroutine(TransScene(0, Fade_Time));
    }

    public void showRestartButton()
    {
        // 현재 멀티샷 구현 안됬으므로 4까지만
        int index = Random.Range(1, 6);
        
        if (index == 1)
        {
            if(PlayerPrefs.GetInt("maxhp") > 0)
            {
                panelty_Text.rectTransform.localPosition = new Vector3(210, 71, 0);
                PlayerPrefs.SetInt("rnd_down", PlayerPrefs.GetInt("rnd_down")+1);
                int stat = PlayerPrefs.GetInt("maxhp");
                stat--;
                PlayerPrefs.SetInt("maxhp", stat);
            }
            else
            {
                panelty_Text.text = "";
            }
        }
        else if (index == 2)
        {
            if(PlayerPrefs.GetInt("dps") > 0)
            {
                panelty_Text.rectTransform.localPosition = new Vector3(210, 33, 0);
                PlayerPrefs.SetInt("rnd_down", PlayerPrefs.GetInt("rnd_down") + 1);
                int stat = PlayerPrefs.GetInt("dps");
                stat--;
                PlayerPrefs.SetInt("dps", stat);
            }
            else
            {
                panelty_Text.text = "";
            }
        }
        else if (index == 3)
        {
            if(PlayerPrefs.GetInt("multi") > 0)
            {
                panelty_Text.rectTransform.localPosition = new Vector3(210, -4, 0);
                PlayerPrefs.SetInt("rnd_down", PlayerPrefs.GetInt("rnd_down") + 1);
                int stat = PlayerPrefs.GetInt("multi");
                stat--;
                PlayerPrefs.SetInt("multi", stat);
            }
            else
            {
                panelty_Text.text = "";
            }
        }
        else if (index == 4)
        {
            if(PlayerPrefs.GetInt("mps") > 0)
            {
                panelty_Text.rectTransform.localPosition = new Vector3(210, -41, 0);
                PlayerPrefs.SetInt("rnd_down", PlayerPrefs.GetInt("rnd_down") + 1);
                int stat = PlayerPrefs.GetInt("mps");
                stat--;
                PlayerPrefs.SetInt("mps", stat);
            }
            else
            {
                panelty_Text.text = "";
            }
        }
        else if (index == 5)
        {
            if(PlayerPrefs.GetInt("range") > 0)
            {
                panelty_Text.rectTransform.localPosition = new Vector3(210, -78, 0);
                PlayerPrefs.SetInt("rnd_down", PlayerPrefs.GetInt("rnd_down") + 1);
                int stat = PlayerPrefs.GetInt("range");
                stat--;
                PlayerPrefs.SetInt("range", stat);
            }
            else
            {
                panelty_Text.text = "";
            }
        }
        
        Time.timeScale = 0;
        PlayerPrefs.SetInt("gold", gold);

        csSetting script = soundmanager.GetComponent<csSetting>();
        script.gameoversound();

        gameOver_Window.SetActive(true);
        
    }

    public void getGold()
    {
        Gold = Random.Range(SceneMinGold, SceneMaxGold);
        gold += Gold;

        txtGold.text = gold.ToString();
        
    }

    public void getGem()
    {
        Gem = Random.Range(SceneMinGem, SceneMaxGem);
        gem += Gem;

        PlayerPrefs.SetInt("gem", gem);

        txtGem.text = gem.ToString();
    }

    private IEnumerator TransScene(int scene, float interval)
    {
        loadingCanvas.SetActive(true);
        Time.timeScale = 1;
        yield return new WaitForSeconds(interval);
        Application.LoadLevel(scene);
    }

    public void killCount()
    {
        enemyCount--;
    }

    IEnumerator Clear()
    {
        int Count = 0;
        while (Count < 99)
        {
            txtClear.text = "<color=#000000ff>" + "<b>" + "포 탈 로\n  이 동 하 세 요 ." + "</b>" + "</color>";
            txtClear.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            txtClear.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            txtClear.text = "<color=#ff0000ff>" + "<b>" + "포 탈 로\n  이 동 하 세 요 ." + "</b>" + "</color>";
            txtClear.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            txtClear.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }

    }
}
