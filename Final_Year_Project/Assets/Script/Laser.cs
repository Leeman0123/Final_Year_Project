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
        gameObject.transform.position += new Vector3 (0, 0.1f, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Enemy") {
            GameFunction.instance.AddScore(10);
            Debug.Log("Kill Normal Enemy");
        } else if (collision.tag == "Correct") {
            GameFunction.instance.AddScore(20);
            Debug.Log("Kill Correct Enemy");
        } else if (collision.tag == "Wrong") {
            GameFunction.instance.MinusScore(10);
            Debug.Log("Kill Wrong Enemy");
        }
    }
}
