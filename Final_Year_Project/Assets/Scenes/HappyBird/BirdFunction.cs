using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions;

public class BirdFunction : MonoBehaviour
{
    public GameObject GameTitle, GameOverTitle;
    public GameObject PlayButton, RestartButton, QuitButton, NextLevelButton;
    public Text GameWinTitle;
    public Button PlayButtonDoubleCoin;


    public static BirdFunction instance;
    public bool IsPlaying;
    public int nextlevel = 1;
    public int Enemies = 0;
    public int life = 3;
    public Sprite disableImage;
    bool DoubleCoin = false;
    public int itemOwned;
    int Coins;

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
        StartCoroutine(GetPlayerCoins());
        StartCoroutine(GetItemAmount());

        LifeText.text = "Life : " + life;
        Time.timeScale = 0;
        instance = this;
        IsPlaying = false;
        GameTitle.SetActive(true);
        GameOverTitle.SetActive(false);
        RestartButton.SetActive(false);
        GameWinTitle.gameObject.SetActive(false);
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

    IEnumerator GetItemAmount()
    {
        var getTask = FirebaseDatabase.DefaultInstance
        .GetReference("students")
        .Child(userID)
        .Child("ItemAmount")
        .GetValueAsync();
        yield return new WaitUntil(() => getTask.IsCompleted || getTask.IsFaulted);
        if (getTask.IsCompleted)
        {
            Dictionary<string, object> results = (Dictionary<string, object>)getTask.Result.Value;
            Debug.Log(results);
            itemOwned = int.Parse(results["DoubleCoin"].ToString());

        }
    }

    private IEnumerator GetPlayerCoins()
    {
        var getTask = FirebaseDatabase.DefaultInstance
        .GetReference("students")
        .Child(script.GetUserId())
        .GetValueAsync();
        yield return new WaitUntil(() => getTask.IsCompleted || getTask.IsFaulted);
        if (getTask.IsCompleted)
        {
            Dictionary<string, object> results = (Dictionary<string, object>)getTask.Result.Value;
            Coins = int.Parse(results["coins"].ToString());
            Debug.Log("Coins = " + Coins);
            //levelReached = int.Parse(results["BirdLevel"].ToString());
            //string coins = results["coins"].ToString();
            //Debug.Log(coins);
            //SetPlayerCoins(Coins, (int)(Score * 0.05));
        }
    }



    // Update is called once per frame
    void Update()
    {
        if(itemOwned <= 0)
        {
            PlayButtonDoubleCoin.interactable = false;
            PlayButtonDoubleCoin.GetComponent<Image>().sprite = disableImage;
        }
        else
        {
            PlayButtonDoubleCoin.interactable = true;
        }
    }

    public void GameDoubleCoin()
    {
        if(itemOwned > 0)
        {
            FirebaseDatabase dbInstance = FirebaseDatabase.DefaultInstance;
            dbInstance.GetReference("students").Child(userID).Child("ItemAmount").GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    int temp = int.Parse(snapshot.Child("DoubleCoin").Value.ToString());
                    temp--;
                    reference.Child("students").Child(userID).Child("ItemAmount").Child("DoubleCoin").SetValueAsync(temp);
                }
            });
            DoubleCoin = true;
            GameStart();
        }
        else
        {
            PlayButtonDoubleCoin.enabled = false;
        }
    }

    public void GameStart()
    {
        Time.timeScale = 1;
        IsPlaying = true;
        GameTitle.SetActive(false);
        PlayButton.SetActive(false);
        PlayButtonDoubleCoin.gameObject.SetActive(false);
        QuitButton.SetActive(false);
        GameWinTitle.gameObject.SetActive(false);
        NextLevelButton.SetActive(false);
        Debug.Log("Game Start");
        Debug.Log(DoubleCoin);
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        IsPlaying = false;
        GameOverTitle.SetActive(true);
        //RestartButton.SetActive(true);
        QuitButton.SetActive(true);
        Debug.Log("Game Over");
        //Emeny.instance.ResetEnemiesAlive();
    }

    public void GameWin()
    {
        int AddCoin;
        Time.timeScale = 0;
        Debug.Log("Player Win");
        IsPlaying = false;
        GameWinTitle.gameObject.SetActive(true);
        //NextLevelButton.SetActive(true);
        QuitButton.SetActive(true);
        Debug.Log(Enemies * (life + 1));
        //Coins += Enemies;
        if (DoubleCoin)
        {
            AddCoin = ((Enemies * (life + 1))*2);
        }
        else
        {
            AddCoin = (Enemies * (life + 1));
        }
        //Emeny.instance.ResetEnemiesAlive();
        Debug.Log(Coins);
        GameWinTitle.text = string.Format("Congratulations, You have passed this level !!!.\n Coin +  {0}", AddCoin);
        Coins += AddCoin;
        reference.Child("students").Child(userID).Child("coins").SetValueAsync(Coins);
        NextLevel();
    }

    public void AddEnemies()
    {
        Enemies++;
        Debug.Log("Kill : " + Enemies);
    }

    public void MinusEnemies()
    {
        Enemies--;
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
