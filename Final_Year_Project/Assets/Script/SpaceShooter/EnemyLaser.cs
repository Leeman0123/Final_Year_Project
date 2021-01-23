using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    public float bulletspeed;
    // Start is called before the first frame update
    void Start()
    {
        bulletspeed = Invader.instance.EnemyFlightSpeed - 0.02f;
        //Debug.Log("Bullet Flight speed:" + bulletspeed);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += new Vector3(0, bulletspeed, 0);
    }
}
