using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameFunction : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject GameTitle, GameOverTitle;
    public GameObject PlayButton, RestartButton, QuitButton;

    public static GameFunction instance;

    public Text ScoreText;

    AudioSource playaudio;
    public AudioClip audioClip;

    public float time;
    public int Score;
    public bool IsPlaying;

    /*public float BulletTime; //disable this 3 variable for PC version
    public GameObject Ship;
    public GameObject Bullet;*/

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Score = 0;
        IsPlaying = false;
        GameTitle.SetActive(true);
        GameOverTitle.SetActive(false);
        RestartButton.SetActive(false);
        playaudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 0.75f && IsPlaying == true)
        {
            Vector3 pos = new Vector3(Random.Range(-1.75f, 1.75f), 8f, -1);
            Instantiate(Enemy, pos, transform.rotation);
            time = 0;
        }
        
        /*BulletTime += Time.deltaTime;//disable for PC version

        if (BulletTime > 0.15f && IsPlaying == true) //每隔0.15秒產生一個子彈 
        {
            Vector3 Bullet_pos = Ship.transform.position + new Vector3(0, 0.6f, 0);
            Instantiate(Bullet, Bullet_pos, Ship.transform.rotation);
            BulletTime = 0f;
        }*/
    }

    public void AddScore()
    {
        Score += 10;
        ScoreText.text = "Score : " + Score;
    }

    public void GameStart()
    {
        IsPlaying = true;
        GameTitle.SetActive(false);
        PlayButton.SetActive(false);
        QuitButton.SetActive(false);
    }

    public void GameOver()
    {
        playaudio.clip = audioClip;
        playaudio.Play();
        IsPlaying = false;
        GameOverTitle.SetActive(true);
        RestartButton.SetActive(true);
        QuitButton.SetActive(true);
        NextLevel();
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Restart the game
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void NextLevel() {
        PlayerPrefs.SetInt("levelReached", 2);
    }
}
