using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour
{
    public static Invader instance;
    AudioSource playaudio;
    public AudioClip audioClip;
    public GameObject explo;

    public float BulletTime;
    public GameObject Bullet;
    public float EnemyFlightSpeed = -0.005f;
    public float EnemyShootSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        playaudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += new Vector3(0, EnemyFlightSpeed, 0);
        BulletTime += Time.deltaTime;
        if (BulletTime > EnemyShootSpeed) {
            Vector3 Bullet_pos = transform.position + new Vector3(0, -0.6f, 0);
            Instantiate(Bullet, Bullet_pos, transform.rotation);
            BulletTime = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Bullet")
        {
            Debug.Log("Destroy Emeny");
            playaudio.clip = audioClip;
            playaudio.Play();
            Instantiate(explo, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        
    }
}
