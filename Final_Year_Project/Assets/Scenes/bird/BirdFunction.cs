using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BirdFunction : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject GameTitle, GameOverTitle;
    public GameObject QuestionText, AnsText1, AnsText2,AnsText3;
    public GameObject PlayButton, RestartButton;
    public static BirdFunction instance;
    public Text ScoreText;
    public float time;
    public int Score;
    public int Birdlife;
    public Text LifeText;
    public bool IsPlaying;

    /*public float BulletTime; //disable this 3 variable for PC version
    public GameObject Ship;
    public GameObject Bullet;*/
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Birdlife = 3;
        Score = 0;
        IsPlaying = false;
        GameTitle.SetActive(true);
        GameOverTitle.SetActive(false);
        RestartButton.SetActive(false);
        AnsText1.SetActive(false);
        AnsText2.SetActive(false);
        AnsText3.SetActive(false);
        QuestionText.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }

    public void AddScore()
    {
        Score += 10;
        ScoreText.text = "Score : " + Score;
    }

    public void life()
    {
        Birdlife--;
        LifeText.text = "Life : " + Birdlife;
    }

    public void GameStart()
    {
        IsPlaying = true;
        GameTitle.SetActive(false);
        PlayButton.SetActive(false);
        QuestionText.SetActive(true);
        AnsText1.SetActive(true);
        AnsText2.SetActive(true);
        AnsText3.SetActive(true);
    }

    public void GameOver()
    {
        IsPlaying = false;
        GameOverTitle.SetActive(true);
        RestartButton.SetActive(true);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Restart the game
    }
}
