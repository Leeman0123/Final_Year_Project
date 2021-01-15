using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    AudioSource playaudio;
    public AudioClip audioClip;
    public GameObject explo;

    public GameObject[] surroundShip = new GameObject[4];

    public float EnemyFlightSpeed = -0.005f;
    public int shoulddestroyship;
    public int score = 100;
    public bool cankill;
    public bool executeonce = false;

    // Start is called before the first frame update
    void Start()
    {
        shoulddestroyship = Random.Range(0, 4);
        Debug.Log(shoulddestroyship);
        cankill = false;
        playaudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y >= 4) {
            gameObject.transform.position += new Vector3(0, EnemyFlightSpeed, 0);
        } else if (executeonce == false){
            for (int i = 0;i < 4;i++) {
                transform.GetChild(i).GetComponent<SurroundBoss>().canshoot = true;
                executeonce = true;
            }
        }

        if (surroundShip[shoulddestroyship] == null && cankill == false) {
            Debug.Log("You can kill boss now");
            foreach (Transform child in transform) {
                GameObject.Destroy(child.gameObject);                
            }
            cankill = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Bullet") {
            if (cankill == true) {
                Debug.Log("Kill Boss");
                playaudio.clip = audioClip;
                playaudio.Play();
                Instantiate(explo, transform.position, transform.rotation);
                Destroy(gameObject);
                GameFunction.instance.AddScore(score);
                GameFunction.instance.GameOver();
            } else {
                Destroy(collision.gameObject);
            }
        }
    }
}
