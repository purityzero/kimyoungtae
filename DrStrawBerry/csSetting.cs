using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csSetting : MonoBehaviour
{
    /* Setting */
    GameObject UIManager;
    GameObject SettingManager;
    GameObject JoystickManager;
    GameObject GameManager;

    /* Sound */
    int SoundOnOff = 0;

    /* Player Sound Clip */
    public AudioClip PlayerHurt;
    public AudioClip PlayerShot;
    public AudioClip PlayerSkill;
    public AudioClip PlayerJump;

    /* Monster Sound Clip */
    public AudioClip RMonsterShot;
    public AudioClip TMonsterShot;
    public AudioClip TMonsterDead;
    public AudioClip MonsterDead;

    public AudioClip Boss1_2_3_10Shot;
    public AudioClip Boss4_7_8_9Shot;
    public AudioClip Boss5_6Shot;
    public AudioClip BossDead;
    public AudioClip BossHit;


    /* Etc Sound Clip */

        // 1. Gold And Gem
    public AudioClip GoldGemGetSound;
    public AudioClip GoldDropSound;
    public AudioClip GemDropSound;

        // 2. GameScene
    public AudioClip GameOverSound;
    public AudioClip WayPointSound;

        // 3. Store
    public AudioClip UpgradeSucess;
    public AudioClip UpgradeFailure;


    /* Audio Source Player */
    AudioSource PlayerSound;
    AudioSource MonsterSound;
    AudioSource EtcSound;
    AudioSource BGM;

    void Start()
    {
        JoystickManager = GameObject.Find("JoystickManager");
        UIManager = GameObject.Find("UIManager");
        SettingManager = GameObject.Find("SettingManager");
        GameManager = GameObject.Find("GameManager");


        SettingManager.SetActive(false);

        PlayerSound = GetComponent<AudioSource>();
        MonsterSound = GetComponent<AudioSource>();
        EtcSound = GetComponent<AudioSource>();
        BGM = GetComponent<AudioSource>();

        SoundOnOff = PlayerPrefs.GetInt("SoundOnOff");

        if (SoundOnOff == 0)
        {
            BGM.Play();
        }
        else if(SoundOnOff == 1)
        {
            BGM.Play();
            BGM.Pause();
        }
    }

    /* Setting */
    

    public void SettingOn()
    {
        Time.timeScale = 0;
        SettingManager.SetActive(true);
    }

    public void SettingOff()
    {
        Time.timeScale = 1;
        SettingManager.SetActive(false);
    }
   
    /* Sound Menu */

    // 1. BGM
    public void SoundOn()
    {
        BGM.UnPause();
        SoundOnOff = 0;
        PlayerPrefs.SetInt("SoundOnOff", SoundOnOff);
        PlayerPrefs.SetInt("TitleSoundOnOff", 0);
    }

    public void SoundOff()
    {
        BGM.Pause();
        SoundOnOff = 1;
        PlayerPrefs.SetInt("SoundOnOff", SoundOnOff);
        PlayerPrefs.SetInt("TitleSoundOnOff", 1);
    }

    // 2. Player
    public void playerhurt()
    {
        if (SoundOnOff == 0)
        {
            PlayerSound.PlayOneShot(PlayerHurt, 0.7f);
        }
    }

    public void playerskill()
    {
        if (SoundOnOff == 0)
        {
            PlayerSound.PlayOneShot(PlayerSkill, 0.7f);
        }
    }

    public void playershot()
    {
        if (SoundOnOff == 0)
        {
            PlayerSound.PlayOneShot(PlayerShot, 0.7f);
        }
    }

    public void playerjump()
    {
        if (SoundOnOff == 0)
        {
            PlayerSound.PlayOneShot(PlayerJump, 0.7f);
        }
    }

    // 3. Monster
    public void Rmonstershot()
    {
        if (SoundOnOff == 0)
        {
            MonsterSound.PlayOneShot(RMonsterShot, 0.7f);
        }
    }

    public void Tmonstershot()
    {
        if (SoundOnOff == 0)
        {
            MonsterSound.PlayOneShot(TMonsterShot, 0.7f);
        }
    }

    public void TmonsterDead()
    {
        if (SoundOnOff == 0)
        {
            MonsterSound.PlayOneShot(TMonsterDead, 0.7f);
        }
    }

    public void monsterDead()
    {
        if (SoundOnOff == 0)
        {
            MonsterSound.PlayOneShot(MonsterDead, 0.7f);
        }
    }

    public void boss1_2_3_10shot()
    {
        if (SoundOnOff == 0)
        {
            MonsterSound.PlayOneShot(Boss1_2_3_10Shot, 0.7f);
        }
    }

    public void boss4_7_8_9shot()
    {
        if (SoundOnOff == 0)
        {
            MonsterSound.PlayOneShot(Boss4_7_8_9Shot, 0.7f);
        }
    }

    public void boss5_6shot()
    {
        if (SoundOnOff == 0)
        {
            MonsterSound.PlayOneShot(Boss5_6Shot, 0.7f);
        }
    }

    public void bossdead()
    {
        if (SoundOnOff == 0)
        {
            MonsterSound.PlayOneShot(BossDead, 1.0f);
        }
    }

    // 4. Etc
    public void goldgemget()
    {
        if (SoundOnOff == 0)
        {
            EtcSound.PlayOneShot(GoldGemGetSound, 0.7f);
        }
    }

    public void gemdrop()
    {
        if (SoundOnOff == 0)
        {
            EtcSound.PlayOneShot(GemDropSound, 0.7f);
        }
    }

    public void golddrop()
    {
        if (SoundOnOff == 0)
        {
            EtcSound.PlayOneShot(GoldDropSound, 0.7f);
        }
    }

    public void gameoversound()
    {
        if(SoundOnOff == 0)
        {
            EtcSound.PlayOneShot(GameOverSound, 0.7f);
        }
    }

    public void waypointsound()
    {
        if(SoundOnOff == 0)
        {
            EtcSound.PlayOneShot(WayPointSound, 0.7f);
        }
    }

    //HG 작업용
    public void upgradesuccess()
    {
        if(SoundOnOff == 0)
        {
            EtcSound.PlayOneShot(UpgradeSucess, 0.7f);
        }
    }

    public void upgradefailure()
    {
        if (SoundOnOff == 0)
        {
            EtcSound.PlayOneShot(UpgradeFailure, 0.7f);
        }
    }
    //HG작업용 끝
}
