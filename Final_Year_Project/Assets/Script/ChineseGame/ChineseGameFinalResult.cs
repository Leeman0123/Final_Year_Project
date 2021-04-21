using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Tools;
using MoreMountains.CorgiEngine;
using Firebase.Database;
using UnityEngine.SceneManagement;

public class ChineseGameFinalResult : MonoBehaviour
{
    private CheckAuthentication script;
    private DatabaseReference reference;
    private string userID;
    [SerializeField] Text Finalresult;
    [SerializeField] GameObject Final;
    [SerializeField] Text PointText;
    [SerializeField] GameObject CheckAuth;
    [SerializeField] Text CoinResult;

    public int point;
    // Start is called before the first frame update
    void Start()
    {
        CorgiEnginePointsEvent.Trigger(PointsMethods.Set, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            Final.SetActive(true);
            point = int.Parse(PointText.text);
            if (SceneManager.GetActiveScene().name == "ChineseGameL1Double" || SceneManager.GetActiveScene().name == "ChineseGameL2Double" || SceneManager.GetActiveScene().name == "ChineseGameL3Double")
            {
                point = point * 2;
            }
            if (point < 0)
            {
                point = 0;
            }
            Finalresult.text = "You get " + point.ToString() + " point in this game";
            CoinResult.text = "+" + (int)(point * 0.5);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Character>().gameObject.SetActive(false);
            //GameObject.Find("Rectangle").GetComponent<Character>().gameObject.SetActive(false);
            script = CheckAuth.GetComponent<CheckAuthentication>();
            reference = FirebaseDatabase.DefaultInstance.RootReference;
            userID = script.GetUserId();
            StartCoroutine(SetPlayerCoins());

        }
    }
    private IEnumerator SetPlayerCoins()
    {

        var getTask = FirebaseDatabase.DefaultInstance
        .GetReference("students")
        .Child(script.GetUserId())
        .GetValueAsync();
        yield return new WaitUntil(() => getTask.IsCompleted || getTask.IsFaulted);
        if (getTask.IsCompleted)
        {
            Dictionary<string, object> results = (Dictionary<string, object>)getTask.Result.Value;
            int coins = int.Parse(results["coins"].ToString());
            SetPlayerCoins(coins, (int)(point * 0.5));
        }
    }
    private void SetPlayerCoins(int coins, int increaseValue)
    {
        coins += increaseValue;
        Debug.Log("After value : " + coins);
        reference.Child("students").Child(userID).Child("coins").SetValueAsync(coins);
    }
}
