using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;

public class StudentDatabasemanager : MonoBehaviour
{
    [SerializeField] Text studentName;
    [SerializeField] Text coinsTotal;
    private CheckAuthentication script;
    // Start is called before the first frame update
    void Start()
    {
        script = GameObject.Find("CheckAuth").GetComponent<CheckAuthentication>();
        StartCoroutine(GetUserData());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GetUserData()
    {
        Debug.Log(script.GetUserId());
        var getTask = FirebaseDatabase.DefaultInstance
        .GetReference("students")
        .Child(script.GetUserId())
        .GetValueAsync();
        yield return new WaitUntil(() => getTask.IsCompleted || getTask.IsFaulted);
        if (getTask.IsCompleted)
        {
            Dictionary<string, object> results = (Dictionary<string, object>)getTask.Result.Value;
            string name = results["name"].ToString();
            string coins = results["coins"].ToString();
            studentName.text = "Hi Student," + name;
            coinsTotal.text = coins;
        }
    }
}
