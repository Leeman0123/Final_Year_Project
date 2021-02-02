using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Bird : MonoBehaviour
{
    public Rigidbody2D rb;
    public Rigidbody2D hook;
    public float releaseTime = .15f;
    public float maxDragDistance = 3f;
    public float health = 4f;
    private bool isPressed = false;
    public GameObject nextbird;


    void Start()
    {

    }
    
    void Update()
    {
        
        if (isPressed)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Vector3.Distance(mousePos, hook.position) > maxDragDistance)
                rb.position = hook.position+(mousePos - hook.position).normalized * maxDragDistance;
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
    }
    void OnCollisionEnter2D(Collision2D colUInfo)
    {
        if (colUInfo.relativeVelocity.magnitude > health)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
        if (nextbird != null)
        {
            nextbird.SetActive(true);
            BirdFunction.instance.life();
        }
        else
        {
            BirdFunction.instance.life();
            BirdFunction.instance.GameOver();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    /*void updatelife()
    {
        life_view.text = "life : " + life;
    }*/

    IEnumerator Relesase()
    {
        yield return new WaitForSeconds(releaseTime);

        GetComponent<SpringJoint2D>().enabled = false;
        this.enabled = false;

        yield return new WaitForSeconds(2f);

        if (nextbird != null)
        {
            nextbird.SetActive(true);
        }else
        {
            GameFunction.instance.GameOver();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
