using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameFunction : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject[] Special, Boss;
    public GameObject GameTitle, GameOverTitle, GameWinTitle;
    public GameObject PlayButton, RestartButton, QuitButton, NextLevelButton;

    public static GameFunction instance;

    public Text ScoreText;

    AudioSource playaudio;
    public AudioClip audioClip;

    public float time;
    public int Score;
    public bool IsPlaying;
    public bool BossSpawn; //check the boss is it spawn
    public float EnemySpawnSpeed = 2f; //set how long to spawn enemy
    public float EnemyFlightSpeed = -0.005f; //set enemy flight speed
    public float EnemyShootSpeed = -1f; //set enemy shoot speed
    public float ReadySpecial; //check how long to spawn special enemy
    public int SpawnSpecial = 5; // set how long to spawn special enemy
    public int SpawnBoss = 100; //set when the boss spawn

    public float BulletTime; //disable this 3 variable for PC version
    public GameObject Ship;
    public GameObject Bullet;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Score = 0;
        IsPlaying = false;
        BossSpawn = false;
        GameTitle.SetActive(true);
        GameOverTitle.SetActive(false);
        RestartButton.SetActive(false);
        GameWinTitle.SetActive(false);
        NextLevelButton.SetActive(false);
        playaudio = GetComponent<AudioSource>();
        ReadySpecial = 0;
        time = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (BossSpawn == false) {
            time += Time.deltaTime;
            if (time > EnemySpawnSpeed && IsPlaying == true) {
                Vector3 pos = new Vector3(Random.Range(-1.75f, 1.75f), 8f, -1);

                if (Score >= SpawnBoss) { //Boss fight
                    pos = new Vector3(0f, 7.3f, -1);
                    Instantiate(Boss[Random.Range(0,2)], pos, transform.rotation);
                    BossSpawn = true;
                } else if (ReadySpecial >= SpawnSpecial) { //Special Enemy
                    Instantiate(Special[Random.Range(0, 2)], pos, transform.rotation);
                    ReadySpecial = 0;
                } else { //Normal Enemy
                    Instantiate(Enemy, pos, transform.rotation);
                }
                Debug.Log("Spawn " + Enemy.name);
                Invader.instance.EnemyFlightSpeed = this.EnemyFlightSpeed;
                Invader.instance.EnemyShootSpeed = this.EnemyShootSpeed;
                time = 0;
                ReadySpecial++;
                Debug.Log("ReadySpecial: " + ReadySpecial);
            }
        }
        
        /*BulletTime += Time.deltaTime;//disable for PC version

        if (BulletTime > 0.15f && IsPlaying == true) //每隔0.15秒產生一個子彈 
        {
            Vector3 Bullet_pos = Ship.transform.position + new Vector3(0, 0.6f, 0);
            Instantiate(Bullet, Bullet_pos, Ship.transform.rotation);
            BulletTime = 0f;
        }*/
    }

    public void AddScore(int add) {
        Debug.Log("Old Score:" + Score); 
        Debug.Log("Score Add:" + add);        
        Score += add;
        ScoreText.text = "Score : " + Score;
        Debug.Log("New Score:" + Score);
    }

    public void MinusScore (int minus) {
        Score -= minus;
        ScoreText.text = "Score : " + Score;
    }

    public void GameStart()
    {
        IsPlaying = true;
        GameTitle.SetActive(false);
        PlayButton.SetActive(false);
        QuitButton.SetActive(false);
        GameWinTitle.SetActive(false);
        NextLevelButton.SetActive(false);
    }

    public void GameOver()
    {
        playaudio.clip = audioClip;
        playaudio.Play();
        IsPlaying = false;
        GameOverTitle.SetActive(true);
        RestartButton.SetActive(true);
        QuitButton.SetActive(true);
    }

    public void GameWin() {
        playaudio.clip = audioClip;
        playaudio.Play();
        IsPlaying = false;
        GameWinTitle.SetActive(true);
        RestartButton.SetActive(true);
        NextLevelButton.SetActive(true);
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

    public void GoNextLevel (string levelName) {
        SceneManager.LoadScene(levelName);
    }
}
