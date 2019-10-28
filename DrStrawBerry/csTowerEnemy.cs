using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csTowerEnemy : MonoBehaviour
{
    GameObject soundmanager = null;

    public float hp = 5.0f;
    private float damage;

    float shotTime;
    bool dead = false;
    bool playonshot = true;

    public Transform[] firePos;
    public GameObject cannon;

    GameObject player;
    GameObject manager = null;
    public GameObject Gold;
    public GameObject Gem;

    public GameObject MParticleHurt;
    public GameObject MParticleDead;

    public int GemRate = 20;

    Animator anim;

    void Start()
    {
        soundmanager = GameObject.Find("Sound_PopUpManager");
        manager = GameObject.Find("GameManager");
        player = GameObject.Find("Player");

        anim = GetComponent<Animator>();
        shotTime = 0.0f;
        
        if (PlayerPrefs.HasKey("dps"))
        {
            damage = setDamage(PlayerPrefs.GetInt("dps"));
        }

        else
        {
            damage = 1.0f;
        }
    }

    void Update()
    {
        if (dead == true)
        {
            return;
        }

        Vector3 targetposition;
        targetposition = player.transform.position;
        targetposition.y = 0.5f;

        transform.LookAt(targetposition);
        shotTime += Time.deltaTime;

        if (hp <= 0)
        {
            dead = true;
            PlayerPrefs.SetInt("kill", PlayerPrefs.GetInt("kill") + 1);
            StartCoroutine(DeadProcess());
        }

        if (dead == false)
        {
            if ((shotTime >= 1.0f))
            {
                anim.SetInteger("animation", 3);

                csSetting script = soundmanager.GetComponent<csSetting>();
                script.Tmonstershot();
                shotTime = 0.0f;
                for (int i = 0; i < firePos.Length; i++)
                {
                    Instantiate(cannon, firePos[i].position, firePos[i].rotation);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerCannon")
        {
            anim.SetInteger("animation", 7);
            Instantiate(MParticleHurt, this.transform.position, this.transform.rotation);
            hp = hp - damage;
        }
    }

    IEnumerator DeadProcess()
    {
        anim.SetInteger("animation", 12);
        GetComponent<BoxCollider>().enabled = false;
        if(playonshot == true)
        {
            csSetting script1 = soundmanager.GetComponent<csSetting>();
            script1.TmonsterDead();
            Instantiate(MParticleDead, this.transform.position, this.transform.rotation);
            playonshot = false;
        }

        yield return new WaitForSeconds(2.0f);
        int rand = Random.Range(0, 100);
        if (rand <= GemRate)
        {
            Instantiate(Gem, this.transform.position, this.transform.rotation);
        }
        else if (rand > GemRate)
        {
            Instantiate(Gold, this.transform.position, this.transform.rotation);
        }
        csGameManager script = manager.GetComponent<csGameManager>();
        script.killCount();

        Destroy(gameObject);
    }

    float setDamage(int level)
    {
        if (level == 1)
            return 1.5f;

        else if (level == 2)
            return 2.0f;

        else if (level == 3)
            return 2.5f;

        else if (level == 4)
            return 3.0f;

        else if (level == 5)
            return 3.5f;

        else
            return 1.0f;
    }
}
