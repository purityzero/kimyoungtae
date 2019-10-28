using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csTowerCannon : MonoBehaviour
{
    public float power = 200f;
    public GameObject myParticle;

    GameObject player = null;
    
	void Start () {

        player = GameObject.Find("Player");

        this.transform.Rotate(Vector3.right * 90.0f);
        
        GetComponent<Rigidbody>().AddForce(transform.up * power);
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // 플레이어 공격받음
            csPlayerHP script = player.GetComponent<csPlayerHP>();
            script.setHpDown();
            Destroy(this.gameObject);
        }

        if (other.tag == "Wall")
        {
            Destroy(this.gameObject);
        }
    }
    
}