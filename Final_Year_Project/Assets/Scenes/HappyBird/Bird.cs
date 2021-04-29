using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Bird : MonoBehaviour
{
    public AudioClip shootbird;
    AudioSource shootingbird;
    public Rigidbody2D rb;
    public Rigidbody2D hook;
    public float releaseTime = .15f;
    public float maxDragDistance = 3f;
    private bool isPressed = false;
    public GameObject nextbird;

    void Start()
    {
        shootingbird = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (isPressed)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Vector3.Distance(mousePos, hook.position) > maxDragDistance)
                rb.position = hook.position + (mousePos - hook.position).normalized * maxDragDistance;
            else
                rb.position = mousePos;
        }
    }
    void OnMouseDown()
    {
        isPressed = true;
        rb.isKinematic = true;
    }
    void OnMouseUp()
    {
        isPressed = false;
        rb.isKinematic = false;
        StartCoroutine(Relesase());
        shootingbird.clip = shootbird;
        shootingbird.Play();
        BirdFunction.instance.Minuslife();
    }
    IEnumerator Relesase()
    {
        yield return new WaitForSeconds(releaseTime);

        GetComponent<SpringJoint2D>().enabled = false;
        this.enabled = false;

        yield return new WaitForSeconds(2f);

        if (nextbird != null)
        {
            nextbird.SetActive(true);
            
            Debug.Log("RespawnBird");
        }
        else
        {
            BirdFunction.instance.GameOver();
        }
    }
}

