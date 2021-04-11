using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase.Database;

public class BirdFunction : MonoBehaviour
{
    public GameObject GameTitle, GameOverTitle, GameWinTitle;
    public GameObject PlayButton, RestartButton, QuitButton, NextLevelButton;

    public static BirdFunction instance;
    public bool IsPlaying;
    public int nextlevel = 1;
    //public int Enemies;
    public int life = 3;
    public int Enemies = 0;

    public Text LifeText;
    public Text EmenyText;

    int levelReached;
    [SerializeField] GameObject CheckAuth;

    private string userID;
    private CheckAuthentication script;
    private DatabaseReference reference;
    //public Text EnemiesAliveText;

    // Start is called before the first frame update
    void Start()
    {
        script = CheckAuth.GetComponent<CheckAuthentication>();
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        userID = script.GetUserId();
        //userID = GameObject.Find("CheckAuth").GetComponent<CheckAuthentication>().GetUserId();

        StartCoroutine(GetBirdLevel());

        LifeText.text = "Life : " + life;
        EmenyText.text = "Emeny : " + Enemies;
        //Enemies = 0;
        Time.timeScale = 0;
        instance = this;
        IsPlaying = false;
        GameTitle.SetActive(true);
        GameOverTitle.SetActive(false);
        RestartButton.SetActive(false);
        GameWinTitle.SetActive(false);
        NextLevelButton.SetActive(false);
    }

    private IEnumerator GetBirdLevel()
    {
        var getTask = FirebaseDatabase.DefaultInstance
        .GetReference("students")
        .Child(script.GetUserId())
        .GetValueAsync();
        yield return new WaitUntil(() => getTask.IsCompleted || getTask.IsFaulted);
        if (getTask.IsCompleted)
        {
            Dictionary<string, object> results = (Dictionary<string, object>)getTask.Result.Value;
            levelReached = int.Parse(results["BirdLevel"].ToString());
            Debug.Log("LevelReached = " + levelReached);

            //string coins = results["coins"].ToString();
            //Debug.Log(coins);
            //SetPlayerCoins(Coins, (int)(Score * 0.05));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStart()
    {
        Time.timeScale = 1;
        IsPlaying = true;
        GameTitle.SetActive(false);
        PlayButton.SetActive(false);
        QuitButton.SetActive(false);
        GameWinTitle.SetActive(false);
        NextLevelButton.SetActive(false);
        Debug.Log("Game Start");
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        IsPlaying = false;
        GameOverTitle.SetActive(true);
        //RestartButton.SetActive(true);
        QuitButton.SetActive(true);
        Debug.Log("Game Over");
    }

    public void GameWin()
    {
        Time.timeScale = 0;
        Debug.Log("Player Win");
        IsPlaying = false;
        GameWinTitle.SetActive(true);
        //NextLevelButton.SetActive(true);
        QuitButton.SetActive(true);
        NextLevel();

    }

    public void AddEnemies()
    {
        Enemies++;
    }
    
    public void MinusEnemies()
    {
        Enemies--;
        EmenyText.text = "Emeny : " + Enemies;
    }

    public void Minuslife()
    {
        life--;
        LifeText.text = "Life : " + life;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Restart the game
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Bird_Low_LevelSelect");
    }

    public void NextLevel()
    {
        Debug.Log("Old level" + levelReached);
        Debug.Log("New level" + nextlevel);
        /*if (PlayerPrefs.GetInt("BirdLowLevel") <= nextlevel)
        {
            PlayerPrefs.SetInt("BirdLowLevel", nextlevel);
        }*/
        if (levelReached <= nextlevel)
        {
            reference.Child("students").Child(userID).Child("BirdLevel").SetValueAsync(nextlevel);
        }
    }

    public void GoNextLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }
}
