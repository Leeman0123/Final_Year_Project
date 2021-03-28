using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if (!GameFunction.instance.BossSpawn) {
            if (collision.CompareTag("Enemy")) {
                GameFunction.instance.AddScore(10);
                Debug.Log("Kill Normal Enemy");
                GameObject.Destroy(gameObject);
            } else if (collision.CompareTag("Correct")) {
                GameFunction.instance.AddScore(20);
                Debug.Log("Kill Correct Enemy");
                GameObject.Destroy(gameObject);
            } else if (collision.CompareTag("Wrong")) {
                GameFunction.instance.MinusScore(10);
                Debug.Log("Kill Wrong Enemy");
                GameObject.Destroy(gameObject);
            }
        }
    }
}
