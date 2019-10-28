using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csEnemyCoin : MonoBehaviour
{
    GameObject soundmanager = null;
    GameObject manager;
    GameObject Player;
    bool Get = false;
    float PlayerRange = 2.0f;
    float Range = 0.5f;
    

    void Start()
    {
        manager = GameObject.Find("GameManager");
        Player = GameObject.Find("Player");
        soundmanager = GameObject.Find("Sound_PopUpManager");

        csSetting script2 = soundmanager.GetComponent<csSetting>();
        script2.golddrop();
    }

    void Update()
    {
        float distance = (Player.transform.position - this.transform.position).magnitude;
        if (distance < PlayerRange)
        {
            transform.LookAt(Player.transform);
            transform.Translate(Vector3.forward * 5.0f * Time.deltaTime);

            if (distance < Range)
            {
                Get = true;
            }

            if (Get == true)
            {
                csSetting script1 = soundmanager.GetComponent<csSetting>();
                script1.goldgemget();
                csGameManager script = manager.GetComponent<csGameManager>();
                script.getGold();
                Destroy(gameObject);
                Get = false;
            }
        }
    }
}
