﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Emeny : MonoBehaviour
{
    public float health = 4f;
    public static int EnemiesAlive = 0;

    public void Start()
    {
        EnemiesAlive++;

    }
    public void Awake()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D colUInfo)
    {
        if (colUInfo.relativeVelocity.magnitude > health)
        {
            Die();
        }
    }
    public void Die()
    {
        Debug.Log("EnemiesDie");
        Debug.Log("Old:" + EnemiesAlive);
        EnemiesAlive--;
        Debug.Log(EnemiesAlive);
        //Destroy(gameObject);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<PolygonCollider2D>().enabled = false;
        if (EnemiesAlive <=0)
        {
            StartCoroutine(Wait());
            //BirdFunction.instance.GameWin();
            
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        BirdFunction.instance.GameWin();
    }
}