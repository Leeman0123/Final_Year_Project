using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameFunction : MonoBehaviour
{

    public static GameFunction instance;
    [Header("Enemy")]
    public GameObject Enemy;
    public GameObject[] Special;
    public GameObject[] Boss;
    [Header("Canvas")]
    public GameObject GameStartCanvas;
    public GameObject GameWinCanvas;
    public GameObject GameLoseCanvas;
    [Header("Text Object")]
    public Text HighScore;
    public Text ScoreText;
    [Header("Button Object")]
    public GameObject PlayButton;
    public GameObject RestartButton;
    public GameObject QuitButton;
    public GameObject NextLevelButton;
    public GameObject ItemSelectPanel;
    [Header("Audio")]
    public AudioClip audioClip;
    AudioSource playaudio;

    [Header("Variable")]
    public float time;
    private int Score;
    private int oldScore;
    public bool doubleCoin;
    public bool IsPlaying;
    public int nextlevel = 1;
    public bool BossSpawn; //check the boss is it spawn
    public float EnemySpawnSpeed = 2f; //set how long to spawn enemy
    public float ReadySpecial; //check how long to spawn special enemy
    public int SpawnSpecial = 5; // set how long to spawn special enemy
    public int SpawnBoss = 100; //set when the boss spawn

    private CheckAuthentication script;
    private DatabaseReference reference;
    private string userID;

    // Start is called before the first frame update
    void Start()
    {
        script = GameObject.Find("CheckAuth").GetComponent<CheckAuthentication>();
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        userID = script.GetUserId();
        Screen.orientation = ScreenOrientation.Portrait;
        instance = this;
        StartCoroutine(GetPlayerScore());
        Score = 0;
        doubleCoin = false;
        IsPlaying = false;
        BossSpawn = false;
        GameStartCanvas.SetActive(true);
        GameWinCanvas.SetActive(false);
        GameLoseCanvas.SetActive(false);
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
                    Debug.Log(Invader.EnemiesAlive);
                    if (Invader.EnemiesAlive == 0) {
                        pos = new Vector3(0f, 7.3f, -1);
                        Instantiate(Boss[Random.Range(0, Boss.Length)], pos, transform.rotation);
                        BossSpawn = true;
                    }
                } else if (ReadySpecial >= SpawnSpecial) { //Special Enemy
                    Instantiate(Special[Random.Range(0, Special.Length)], pos, transform.rotation);
                    ReadySpecial = 0;
                } else { //Normal Enemy
                    Instantiate(Enemy, pos, transform.rotation);
                }
                Debug.Log("Spawn " + Enemy.tag);
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
        Debug.Log("Old Score:" + Score);
        Debug.Log("Score decrease:" + minus);
        Score -= minus;
        ScoreText.text = "Score : " + Score;
        Debug.Log("New Score:" + Score);
    }

    public void GameStart()
    {
        IsPlaying = true;
        GameStartCanvas.SetActive(false);
        GameWinCanvas.SetActive(false);
        GameLoseCanvas.SetActive(false);
        Debug.Log("Game Start");
        for (int i = 0; i <= 2; i++) {
            if (ItemSelector.itemInstance.itemEnable[i]) {
                switch (i) {
                    case 0:
                        StartCoroutine(ItemSelector.itemInstance.SetInvincibleAmount(-1));
                        Debug.Log("Invincible activated");
                        break;
                    case 1:
                        StartCoroutine(ItemSelector.itemInstance.SetSpecialModeAmount(-1));
                        Debug.Log("Special Shoot activated");
                        break;
                    case 2:
                        doubleCoin = true;
                        StartCoroutine(ItemSelector.itemInstance.SetDoubleCoinAmount(-1));
                        Debug.Log("Double Coin activated");
                        break;
                    default:
                        Debug.LogError("Unknow Item, Item ID : " + i);
                        break;
                }
            }
        }
    }

    public void GameOver()
    {
        playaudio.clip = audioClip;
        playaudio.Play();
        IsPlaying = false;
        GameLoseCanvas.SetActive(true);
        Debug.Log("Game Over");
    }

    public IEnumerator GameWin() {
        Debug.Log("Player Win");
        IsPlaying = false;
        if (Score > oldScore)
            UpdateScore();
        yield return new WaitForSeconds(1);
        playaudio.clip = audioClip;
        playaudio.Play();
        GameWinCanvas.SetActive(true);
        NextLevel();
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Restart the game
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("SpaceShooterLevelSelect");
    }

    public void NextLevel() {
        int levelReached = SpaceShooterLevelSelector.ssLevelSelectorInstance._levelReached;
        Debug.Log("Store level " + levelReached);
        if (levelReached < nextlevel) {
            SpaceShooterLevelSelector.ssLevelSelectorInstance.SetPlayerLevel(nextlevel);
        }
    }

    public void GoNextLevel (string levelName) {
        SceneManager.LoadScene(levelName);
    }

    private void UpdateScore() {
        reference.Child("SpaceShooter").Child("Score").Child(userID).Child("Level" + (nextlevel - 1)).SetValueAsync(Score);
        Debug.Log("Score Updated");
    }

    private IEnumerator GetPlayerScore() {
        var getTask = FirebaseDatabase.DefaultInstance
            .GetReference("SpaceShooter")
            .Child("Score")
            .Child(userID)
            .GetValueAsync();
        yield return new WaitUntil(() => getTask.IsCompleted || getTask.IsFaulted);
        if (getTask.IsCompleted) {
            Dictionary<string, object> results = (Dictionary<string, object>)getTask.Result.Value;
            oldScore = int.Parse(results["Level" + (nextlevel - 1)].ToString());
            HighScore.text = "High Score : " + oldScore;
        }
    }
}
