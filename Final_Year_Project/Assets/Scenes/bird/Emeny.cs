using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emeny : MonoBehaviour
{
    public float health = 4f;

    void OnCollisionEnter2D(Collision2D colUInfo)
    {
        if(colUInfo.relativeVelocity.magnitude > health)
        {
            Die();
            BirdFunction.instance.AddScore();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }
}
