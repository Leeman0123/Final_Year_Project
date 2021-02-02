using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorrectBossPastParticiple : MonoBehaviour {

    AudioSource playaudio;
    public AudioClip audioClip;
    public GameObject explo;
    public GameObject bullet;

    public GameObject[] surroundShip = new GameObject[4];

    public float bulletTime;
    public float bossShootSpeed = 1f;
    public float enemyFlightSpeed = -0.005f;
    public int shoulddestroyship;
    public int score = 100;
    public bool cankill;
    public bool executeonce = false;

    // Start is called before the first frame update
    void Start()
    {
        bulletTime = 0;
        shoulddestroyship = Random.Range(0, 4);
        Debug.Log(shoulddestroyship);
        cankill = false;
        playaudio = GetComponent<AudioSource>();
        transform.GetChild(shoulddestroyship).GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().text = JSONReader.instance.GetPastParticipleCorrect();
        transform.GetChild(shoulddestroyship).GetComponent<CorrectSurroundPastParticiple>().textchange = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y >= 4) {
            gameObject.transform.position += new Vector3(0, enemyFlightSpeed, 0);
        } else if (executeonce == false && surroundShip != null){
            for (int i = 0;i < 4;i++) {
                transform.GetChild(i).GetComponent<CorrectSurroundPastParticiple>().canshoot = true;
                executeonce = true;
            }
        }

        if (surroundShip[shoulddestroyship] == null && cankill == false) {
            Debug.Log("You can kill boss now");
            foreach (Transform child in transform) {
                GameFunction.instance.AddScore(25);
                GameObject.Destroy(child.gameObject);                
            }
            cankill = true;
        }
        bulletTime += Time.deltaTime;
        if (bulletTime > bossShootSpeed && GameFunction.instance.IsPlaying) {
            Vector3 Bullet_1_pos = transform.position + new Vector3(-0.3f, -0.6f, 0);
            Vector3 Bullet_2_pos = transform.position + new Vector3(0.3f, -0.6f, 0);
            Instantiate(bullet, Bullet_1_pos, transform.rotation);
            Instantiate(bullet, Bullet_2_pos, transform.rotation);
            bulletTime = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Bullet")) {
            if (cankill == true) {
                Debug.Log("Kill Boss");
                playaudio.clip = audioClip;
                playaudio.Play();
                Instantiate(explo, transform.position, transform.rotation);                
                GameFunction.instance.AddScore(score);
                StartCoroutine(GameFunction.instance.GameWin());
                GetComponent<SpriteRenderer>().enabled = false;
                //Destroy(gameObject);
                Debug.Log("Boss Destroy");
            } else {
                Destroy(collision.gameObject);
            }
        }
    }
}
