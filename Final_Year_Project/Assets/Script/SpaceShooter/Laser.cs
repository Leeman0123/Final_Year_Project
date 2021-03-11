using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!GameFunction.instance.BossSpawn) {
            if (collision.tag == "Enemy") {
                GameFunction.instance.AddScore(10);
                Debug.Log("Kill Normal Enemy");
                GameObject.Destroy(gameObject);
            } else if (collision.tag == "Correct") {
                GameFunction.instance.AddScore(20);
                Debug.Log("Kill Correct Enemy");
                GameObject.Destroy(gameObject);
            } else if (collision.tag == "Wrong") {
                GameFunction.instance.MinusScore(10);
                Debug.Log("Kill Wrong Enemy");
                GameObject.Destroy(gameObject);
            }
        }
    }
}
