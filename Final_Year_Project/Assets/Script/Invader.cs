using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour
{
    AudioSource playaudio;
    public AudioClip audioClip;
    public GameObject explo;

    public float BulletTime;
    public GameObject Bullet;

    // Start is called before the first frame update
    void Start()
    {
        playaudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += new Vector3(0, -0.01f, 0);
        BulletTime += Time.deltaTime;
        if (BulletTime > 1f) {
            Vector3 Bullet_pos = transform.position + new Vector3(0, 0.6f, 0);
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
            Destroy(col.gameObject);
            Destroy(gameObject);
            GameFunction.instance.AddScore();
        }

        
    }
}
