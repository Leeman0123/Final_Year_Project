using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Tools;
using MoreMountains.CorgiEngine;
using UnityEngine.SceneManagement;

public class PauseButtonControl : MonoBehaviour
{
    [SerializeField] Button pausebutton;
    [SerializeField] GameObject player;
    [SerializeField] GameObject pausescreen;
    [SerializeField] Button resume;
    [SerializeField] GameObject playerdie;
    private int health;

    private void Awake()
    {

    }


    private void Start()
    {
        pausebutton.onClick.AddListener(() => openpausescreen());
        resume.onClick.AddListener(() => resumefunction());
    }
    public void openpausescreen()
    {
        CheckPlayerDie aaa = playerdie.GetComponent<CheckPlayerDie>();
        aaa.playing = false;
        Health gethp = player.GetComponent<Health>();
        health = gethp.CurrentHealth;
        Debug.Log(health);
        player.SetActive(false);
        pausescreen.SetActive(true);
    }
    public void resumefunction()
    {

        Health returnhp = player.GetComponent<Health>();

        player.SetActive(true);
        returnhp.SetHealth(health, player);
        pausescreen.SetActive(false);
        CheckPlayerDie aaa = playerdie.GetComponent<CheckPlayerDie>();
        aaa.playing = true;


    }
}
