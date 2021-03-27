using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipControl : MonoBehaviour
{

    public static ShipControl shipinstance;

    public GameObject explo;
    public GameObject playerShip;

    public int life = 5;
    public Text LifeText;
    AudioSource playaudio;
    public AudioClip getHitAudio, invincibleAudio;

    private float lastTapTime;
    private const float DOUBLE_TAP_TIME = 1f;

    // Start is called before the first frame update
    void Start()
    {
        shipinstance = this;
        playaudio = GetComponent<AudioSource>();
        LifeText.text = "Life : " + life;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameFunction.instance.IsPlaying) {
            //Control for PC version 
            if (Input.GetKey(KeyCode.D)) {
                gameObject.transform.position += new Vector3(0.25f, 0, 0);
            }

            if (Input.GetKey(KeyCode.A)) {
                gameObject.transform.position += new Vector3(-0.25f, 0, 0);
            }

            if (Input.GetKey(KeyCode.C)) {
                StartCoroutine(Invincible());
            }

            if (Input.touchCount.Equals(1)) //Control for mobile version
            {
                Touch touch = Input.GetTouch(0);
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                touchPosition.z = -1f;
                transform.position = touchPosition;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended) {
                float timeSinceLastTap = Time.time - lastTapTime;

                if (timeSinceLastTap <= DOUBLE_TAP_TIME) {
                    StartCoroutine(Invincible());
                }

                lastTapTime = Time.time;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy") || col.CompareTag("Correct") || col.CompareTag("Wrong") || col.CompareTag("EnemyBullet")) //minus life when Enemy attack player
        {
            if (!col.CompareTag("EnemyBullet")) {
                Invader.EnemiesAlive--;
            }
            Destroy(col.gameObject);
            Instantiate(explo, transform.position, transform.rotation);
            life--;
            LifeText.text = "Life : " + life;
            playaudio.clip = getHitAudio;
            playaudio.Play();
            if (life == 0)
            {
                Destroy(gameObject);
                GameFunction.instance.GameOver();
            }
        }
    }

    private IEnumerator Invincible() {
        if (ItemSelector.itemInstance.itemEnable[0] && GameFunction.instance.IsPlaying) {
            BoxCollider2D boxCollider2D = playerShip.GetComponent<BoxCollider2D>();
            boxCollider2D.enabled = false;
            Debug.Log("Invincible now");
            playaudio.clip = invincibleAudio;
            playaudio.loop = true;
            playaudio.Play();
            ItemSelector.itemInstance.itemEnable[0] = false;
            yield return new WaitForSeconds(10);
            Debug.Log("Invincible end");
            boxCollider2D.enabled = true;
            playaudio.loop = false;
            playaudio.Stop();
        }
    }
}
