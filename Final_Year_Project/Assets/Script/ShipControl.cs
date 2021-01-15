using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipControl : MonoBehaviour
{
    public GameObject Bullet; //disable for mobile version
    public GameObject explo;

    public int life;
    public static ShipControl shipinstance;
    public Text LifeText;
    AudioSource playaudio;
    public AudioClip audioClip;

    // Start is called before the first frame update
    void Start()
    {
        shipinstance = this;
        playaudio = GetComponent<AudioSource>();
        life = 5;
        LifeText.text = "Life : " + life;
    }

    // Update is called once per frame
    void Update()
    {
        //Control for PC version 
        if (Input.GetKey(KeyCode.D)) {
            gameObject.transform.position += new Vector3(0.025f, 0, 0);
        }

        if (Input.GetKey(KeyCode.A)) {
            gameObject.transform.position += new Vector3(-0.025f, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            Vector3 pos = gameObject.transform.position + new Vector3(0, 0.6f, 0);
            Instantiate(Bullet, pos, gameObject.transform.rotation);
        }
        
        /*if (Input.GetTouch(0).phase == TouchPhase.Moved) //Control for mobile version
        {
            float x = Input.touches[0].deltaPosition.x * Time.deltaTime * 0.5f;
            float y = Input.touches[0].deltaPosition.y * Time.deltaTime * 0.5f;
            transform.Translate(new Vector3(x, y, 0));
        }*/
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy" || col.tag == "Correct" || col.tag == "Wrong") //minus life when Enemy attack player
        {
            Destroy(col.gameObject);
            Instantiate(explo, transform.position, transform.rotation);
            life--;
            LifeText.text = "Life : " + life;
            playaudio.clip = audioClip;
            playaudio.Play();
            if (life == 0)
            {
                Destroy(gameObject);
                GameFunction.instance.GameOver();
            }
        }
    }
}
