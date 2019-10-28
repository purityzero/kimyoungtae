using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csRangeCannon : MonoBehaviour
{
    public float power = 200f;
    public GameObject myParticle;
    Vector3 StartPosition;
    public float Range = 7.0f;

    GameObject player = null;

    void Awake()
    {
        StartPosition = transform.position;
    }

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

        if(other.tag == "Guard")
        {
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        float RangeDistance = (StartPosition - transform.position).magnitude;
        //Debug.Log(RangeDistance);
        if (RangeDistance >= Range)
        {
            Destroy(this.gameObject);
        }
    }

}