using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    public GameObject BulletPrefab;
    public Transform firePoint;

    public float BulletTime;
    public float bulletForce = 10f;
    public bool itemenable;

    public float maxSwipeTime;
    public float minSwipeDistance;

    private float swipeStartTime;
    private float swipeEndTime;
    private float swipeTime;

    private Vector2 startSwipePosition;
    private Vector2 endSwipePosition;
    private float swipeLength;
    void Start()
    {
        itemenable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space)) {
            if (itemenable)
                NormalShoot();
            else
                Item1Shoot();
        }
        if (Input.GetKeyDown(KeyCode.V) && ItemSelector.itemInstance.itemEnable[1]) {
            SwitchShoot();
        }

    }

    private void Swipe() {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) {
                swipeStartTime = Time.time;
                startSwipePosition = touch.position;
            } else if (touch.phase == TouchPhase.Ended) {
                swipeEndTime = Time.time;
                endSwipePosition = touch.position;
                swipeTime = swipeEndTime - swipeStartTime;
                swipeLength = (endSwipePosition - startSwipePosition).magnitude;
                if (swipeTime < maxSwipeTime && swipeLength > minSwipeDistance && ItemSelector.itemInstance.itemEnable[1]) {
                    SwitchShoot();
                }
            }
        }
    }

    private void SwitchShoot() {
        if (itemenable)
            itemenable = false;
        else
            itemenable = true;
    }

    private void NormalShoot() {
        if (BulletTime > 0.1f) {
            GameObject bullet = Instantiate(BulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
            BulletTime = 0f;
        }
        BulletTime += Time.deltaTime;
    }

    private void Item1Shoot() {
        if (BulletTime > 0.1f) {
            GameObject bullet1 = Instantiate(BulletPrefab, firePoint.position, firePoint.rotation);
            GameObject bullet2 = Instantiate(BulletPrefab, firePoint.position, firePoint.rotation);
            bullet1.transform.Rotate(new Vector3(0, 0, 20f));
            bullet2.transform.Rotate(new Vector3(0, 0, -20f));
            Rigidbody2D rb1 = bullet1.GetComponent<Rigidbody2D>();
            Rigidbody2D rb2 = bullet2.GetComponent<Rigidbody2D>();
            rb1.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
            rb1.AddForce(-firePoint.right * bulletForce * 0.4f, ForceMode2D.Impulse);
            rb2.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
            rb2.AddForce(firePoint.right * bulletForce * 0.4f, ForceMode2D.Impulse);
            BulletTime = 0f;
        }
        BulletTime += Time.deltaTime;
    }
}
