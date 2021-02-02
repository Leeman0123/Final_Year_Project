using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WrongSurroundPastParticiple : MonoBehaviour
{

    AudioSource playaudio;
    public AudioClip audioClip;
    public GameObject explo;

    public static WrongSurroundPastParticiple instance;

    public Vector3 angleToRotate;
    public bool canshoot;
    public float BulletTime;
    public GameObject Bullet;
    public float EnemyShootSpeed = 1.25f;
    public bool textchange = false;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        playaudio = GetComponent<AudioSource>();
        canshoot = false;
        if (!textchange) {
            transform.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().text = JSONReader.instance.GetPastParticipleCorrect();
        }
    }

    // Update is called once per frame
    void Update() {
        transform.RotateAround(this.transform.parent.position, new Vector3(0f, 0f, 1f), 20 * Time.deltaTime); //surround the boss
        this.transform.Rotate(angleToRotate * Time.deltaTime); //face to player
        if (canshoot == true) {
            BulletTime += Time.deltaTime;
            if (BulletTime > EnemyShootSpeed) {
                Vector3 Bullet_pos = transform.position + new Vector3(0, 0.6f, 0);
                Instantiate(Bullet, Bullet_pos, transform.rotation);
                BulletTime = 0f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Bullet")) {
            Debug.Log("Destroy Emeny");
            playaudio.clip = audioClip;
            playaudio.Play();
            Instantiate(explo, transform.position, transform.rotation);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
