using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Firebase.Database;
using UnityEngine.UI;
using System.IO;

public class BirdLowLevelSelect : MonoBehaviour {

    public Text CoinText;
    public Text DoubleCoinText;
    public Button[] levelbuttons;
    public GameObject NotEnoughCoinPanel;

    public int itemOwned;
    int Coins;
    int levelReached;
    [SerializeField] GameObject CheckAuth;

    private string userID;
    private CheckAuthentication script;
    private DatabaseReference reference;

   
    private void Start() {
        script = CheckAuth.GetComponent<CheckAuthentication>();
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        userID = script.GetUserId();


        StartCoroutine(GetPlayerCoins());
        StartCoroutine(GetBirdLevel());
        StartCoroutine(GetItemAmount());

        //reference.Child("students").Child(userID).Child("BirdLevel").SetValueAsync(1);

        //levelReached = PlayerPrefs.GetInt("BirdLowLevel", 1);
        //Coins = PlayerPrefs.GetInt("BirdCoin", 15);

        /*for (int i = 0; i < levelbuttons.Length; i++) {
            if (i + 1 > levelReached)
                levelbuttons[i].interactable = false;
        }*/
    }
    private void Update()
    {
        CoinText.text = "Coin : " + Coins;
        DoubleCoinText.text = "x2 : " + itemOwned;
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
            for (int i = 0; i < levelbuttons.Length; i++)
            {
                if (i + 1 > levelReached)
                    levelbuttons[i].interactable = false;
            }

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

    public void Select(string levelName) {
        if(Coins >= 10)
        {
            Coins -= 10;

            reference.Child("students").Child(userID).Child("coins").SetValueAsync(Coins);

            //PlayerPrefs.SetInt("BirdCoin", NewCoin);
            SceneManager.LoadScene(levelName);
        }
        else
        {
            NotEnoughCoinPanel.SetActive(true);
        }
    }
    public void GoQuiz()
    {
        SceneManager.LoadScene("MathQuiz");
    }
    public void BackMenu()

    {
        NotEnoughCoinPanel.SetActive(false);
    }
    public void BackHome()

    {
        SceneManager.LoadScene("Bird_HomePage");
    }

}