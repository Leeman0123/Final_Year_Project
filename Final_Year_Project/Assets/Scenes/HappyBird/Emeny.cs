using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Emeny : MonoBehaviour
{
    public AudioClip dead;
    AudioSource deading;
    public float health = 4f;
    public static int EnemiesAlive = 0;

    public void Start()
    {
        EnemiesAlive++;
        Debug.Log(EnemiesAlive);
        deading = GetComponent<AudioSource>();
    }
    public void Awake()
    {
        EnemiesAlive = 0;
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
        BirdFunction.instance.AddEnemies();
        //Destroy(gameObject);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<PolygonCollider2D>().enabled = false;
        deading.clip = dead;
        deading.Play();
        if (EnemiesAlive <=0)
        {
            //StartCoroutine(Wait());
            BirdFunction.instance.GameWin();
            
        }
    }

    public void ResetEnemiesAlive()
    {
        EnemiesAlive = 0;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        BirdFunction.instance.GameWin();
    }
}
