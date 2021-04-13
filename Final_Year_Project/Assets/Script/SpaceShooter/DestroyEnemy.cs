using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy") || col.CompareTag("Correct") || col.CompareTag("Wrong"))
        {
            Destroy(col.gameObject);
            Invader.EnemiesAlive--;
            if (col.CompareTag("Wrong")) {
                GameFunction.instance.AddScore(20);
                Debug.Log("Wrong answer destroy by wall");
            } else if (col.CompareTag("Correct")) {
                GameFunction.instance.MinusScore(10);
                Debug.Log("Correct answer destroy by wall");
            } else {
                Debug.Log("Destroy by Wall");
            }
        }
    }
}
