using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csMeleeEnemy : MonoBehaviour
{
    enum MeleeEnemySTATE
    {
        NONE = -1,
        IDLE = 0,
        MOVE,
        ATTACK,
        DAMAGE,
        DEAD
    }

    MeleeEnemySTATE meleeState = MeleeEnemySTATE.IDLE;

    float stateTime = 0.0f;
    public float idleStateMaxTime = 1.0f;
    public float attackStateMaxTime = 1.0f;
    public float attackDelay = 1.0f;

    public float Speed = 3.0f;
    public float attackableRange = 1.0f;

    public float hp = 7.0f;
    //public int Gold = 5;

    bool isDead = false;

    private float damage;

    Animator anim;

    GameObject player;
    GameObject manager;
    GameObject soundmanager = null;
    public GameObject Gold;
    public GameObject Gem;

    public GameObject MParticleHurt;
    public GameObject MParticleDead;

    public int GemRate = 20;

    void Awake()
    {
        InitMonster();
    }

    void Start()
    {
        soundmanager = GameObject.Find("Sound_PopUpManager");
        manager = GameObject.Find("GameManager");
        player = GameObject.Find("Player");
        anim = GetComponent<Animator>();

        if (PlayerPrefs.HasKey("dps"))
        {
            damage = setDamage(PlayerPrefs.GetInt("dps"));
        }

        else
        {
            damage = 1.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerCannon")
        {
            meleeState = MeleeEnemySTATE.DAMAGE;
        }
    }

    void InitMonster()
    {
        meleeState = MeleeEnemySTATE.IDLE;
    }

    void Update()
    {
        Vector3 playerposition;
        playerposition = player.transform.position;
        playerposition.y = 0.5f;

        switch (meleeState)
        {
            case MeleeEnemySTATE.IDLE:
                stateTime += Time.deltaTime;
                if (stateTime > idleStateMaxTime)
                {
                    anim.SetInteger("animation", 8);
                    stateTime = 0.0f;
                    meleeState = MeleeEnemySTATE.MOVE;
                }
                break;

            case MeleeEnemySTATE.MOVE:
                anim.SetInteger("animation", 13);

                float distance = (playerposition - transform.position).magnitude;

                if (distance < attackableRange)
                {
                    //transform.Translate(transform.position);
                    transform.Translate(Vector3.forward * Speed * Time.deltaTime * 0);
                    stateTime = 0.0f;
                    meleeState = MeleeEnemySTATE.ATTACK;
                }

                else
                {
                    transform.LookAt(playerposition);
                    anim.SetInteger("animation", 13);
                    transform.Translate(Vector3.forward * Speed * Time.deltaTime);
                }

                break;

            case MeleeEnemySTATE.ATTACK:
                stateTime += Time.deltaTime;

                float distance1 = (playerposition - transform.position).magnitude;

                if (stateTime > attackDelay)
                {
                    stateTime = 0.0f;
                    anim.SetInteger("animation", 2);

                    // 플레이어 공격받음
                    csPlayerHP hpscript = player.GetComponent<csPlayerHP>();
                    hpscript.setHpDown();
                }

                if (distance1 > attackableRange)
                {
                    stateTime = 0.0f;
                    meleeState = MeleeEnemySTATE.MOVE;
                }


                break;

            case MeleeEnemySTATE.DAMAGE:
                hp = hp - damage;

                if (hp <= 0)
                {
                    GetComponent<BoxCollider>().enabled = false;
                    meleeState = MeleeEnemySTATE.DEAD;
                }

                else
                {
                    anim.SetInteger("animation", 7);
                    Instantiate(MParticleHurt, this.transform.position, this.transform.rotation);
                    stateTime = 0.0f;
                    meleeState = MeleeEnemySTATE.IDLE;
                }

                break;

            case MeleeEnemySTATE.DEAD:
                if (isDead == false)
                {
                    isDead = true;
                    anim.SetInteger("animation", 12);
                    Instantiate(MParticleDead, this.transform.position, this.transform.rotation);
                    csSetting script1 = soundmanager.GetComponent<csSetting>();
                    PlayerPrefs.SetInt("kill", PlayerPrefs.GetInt("kill") + 1);
                    script1.monsterDead();
                    StartCoroutine(DeadProcess());
                }
                break;
        }
    }

    IEnumerator DeadProcess()
    {
        yield return new WaitForSeconds(2.0f);
        int rand = Random.Range(1, 100);
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
