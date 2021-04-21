using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Tools;
using MoreMountains.CorgiEngine;
using Firebase.Database;
using UnityEngine.SceneManagement;
public class ChineseGetMoney : MonoBehaviour
{
    [SerializeField] GameObject CheckAuth;
    [SerializeField] Text MoneyText;
    private int coin;
    private CheckAuthentication script;
    // Start is called before the first frame update
    void Start()
    {
        script = CheckAuth.GetComponent<CheckAuthentication>();
        StartCoroutine(GetCoins());
    }

    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator GetCoins()
    {

        var getTask = FirebaseDatabase.DefaultInstance
        .GetReference("students")
        .Child(script.GetUserId())
        .GetValueAsync();
        yield return new WaitUntil(() => getTask.IsCompleted || getTask.IsFaulted);
        if (getTask.IsCompleted)
        {
            Dictionary<string, object> results = (Dictionary<string, object>)getTask.Result.Value;
            coin = int.Parse(results["coins"].ToString());
            MoneyText.text = coin.ToString();

        }
    }
}
