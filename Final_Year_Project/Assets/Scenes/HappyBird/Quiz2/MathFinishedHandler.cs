using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Database;
using System.IO;

public class MathFinishedHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject good;
    [SerializeField] GameObject awesome;
    [SerializeField] GameObject amazing;
    [SerializeField] GameObject FinishPanel;
    [SerializeField] Text scoreText;
    [SerializeField] Button btn;
    private int correct = 0;
    private int total = 0;
    int Coin;
    int Coins;
    int NewCoin;
    [SerializeField] GameObject CheckAuth;

    private string userID;
    private CheckAuthentication script;
    private DatabaseReference reference;
    
    void Awake()
    {
        FinishPanel.SetActive(false);
        /*MathGenerateQuestion gq = MathGenerateQuestion.instance;
        //correct = gq.GetCorrect();
        //total = gq.GetQuestionTotal();
        if (correct >=0 && correct < 5)
        {
            good.SetActive(true);
            awesome.SetActive(false);
            amazing.SetActive(false);
        }
        else if (correct >5 && correct < 10)
        {
            good.SetActive(false);
            awesome.SetActive(true);
            amazing.SetActive(false);
        }
        else if (correct >10)
        {
            good.SetActive(false);
            awesome.SetActive(false);
            amazing.SetActive(true);

        }
        scoreText.text = string.Format("Score: {0}/10", correct);
        //NewCoin = Coins + correct;*/
    }

    void Start()
    {
        
        //userID = GameObject.Find("CheckAuth").GetComponent<CheckAuthentication>().GetUserId();
        script = CheckAuth.GetComponent<CheckAuthentication>();
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        userID = script.GetUserId();
        StartCoroutine(GetPlayerCoins());

        btn.onClick.AddListener(() =>
        {


            /*string userId = GameObject.Find("CheckAuth").GetComponent<CheckAuthentication>().GetUserId();
            if (SceneManager.GetActiveScene().name.Equals("Animals1"))
            {
                StartCoroutine(GameObject.Find("DatabaseQuizWriter").GetComponent<DatabaseQuizWriter>()
                                        .WriteVocabAnimals(
                                            userId,
                                            correct,
                                            GameObject.Find("BarBackground").GetComponentInChildren<BarScript>().GetRemainTime(),
                                            total));
            }
            else if (SceneManager.GetActiveScene().name.Equals("Vehicle1"))
            {
                StartCoroutine(GameObject.Find("DatabaseQuizWriter").GetComponent<DatabaseQuizWriter>()
                                        .WriteVocabVehicles(
                                            userId,
                                            correct,
                                            GameObject.Find("BarBackground").GetComponentInChildren<BarScript>().GetRemainTime(),
                                            total));
            }*/

            //Coin = PlayerPrefs.GetInt("BirdCoin", 0);
            MathGenerateQuestion gq = MathGenerateQuestion.instance;
            correct = gq.GetCorrect();
            Debug.Log(Coins);
            Debug.Log(correct);
            NewCoin = Coins + correct;
            Debug.Log(NewCoin);
            
            if (correct >= 0 && correct < 5)
            {
                good.SetActive(true);
                awesome.SetActive(false);
                amazing.SetActive(false);
            }
            else if (correct > 5 && correct < 10)
            {
                good.SetActive(false);
                awesome.SetActive(true);
                amazing.SetActive(false);
            }
            else if (correct > 10)
            {
                good.SetActive(false);
                awesome.SetActive(false);
                amazing.SetActive(true);

            }
            scoreText.text = string.Format("Score: {0}/10", correct);

            //Coins = Coins + correct;
            reference.Child("students").Child(userID).Child("coins").SetValueAsync(NewCoin);
            //PlayerPrefs.SetInt("BirdCoin", NewCoin);
            SceneManager.LoadScene("Bird_Low_LevelSelect");
        });
    }

    // Update is called once per frame
    void Update()
    {

        MathGenerateQuestion gq = MathGenerateQuestion.instance;
        correct = gq.GetCorrect();
        scoreText.text = string.Format("Score: {0}/10", correct);

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
            //string coins = results["coins"].ToString();
            //Debug.Log(Coins);
            //SetPlayerCoins(Coins, (int)(Score * 0.05));
        }
    }
}
