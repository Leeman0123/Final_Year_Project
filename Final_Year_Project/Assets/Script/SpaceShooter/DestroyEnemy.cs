using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy" || col.tag == "Correct" || col.tag == "Wrong")
        {
            Destroy(col.gameObject);
            if (col.tag == "Wrong") {
                GameFunction.instance.AddScore(20);
                Debug.Log("Wrong answer destroy by wall");
            } else if (col.tag == "Correct") {
                GameFunction.instance.MinusScore(10);
                Debug.Log("Correct answer destroy by wall");
            } else {
                Debug.Log("Destroy by Wall");
            }
        }
    }
}
