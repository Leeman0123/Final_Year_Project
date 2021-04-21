using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using MoreMountains.Tools;
using MoreMountains.CorgiEngine;
using UnityEngine.SceneManagement;

public class CheckPlayerDie : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] GameObject GameOverUI;
    public int playerHealth;
    public bool GameOver = false;
    public bool playing = true;

    private void Awake()
    {

    }
    // Update is called once per frame
    void Update()
    {
        Health health = Player.GetComponent<Health>();
        playerHealth = health.CurrentHealth;
        if (playerHealth <= 0 && playing == true)
        {
            GameOver = true;
            Debug.Log(GameOver + "gameover");
            Debug.Log(playing + "playing");
            Debug.Log(playerHealth + "   Health");
            if (GameOver)
            {
                GameOverUI.SetActive(true);
                Player.SetActive(false);
                GameOver = false;
                Debug.Log(GameOver + "gameover");
                Debug.Log(playing + "playing");
            }
        }

    }
}