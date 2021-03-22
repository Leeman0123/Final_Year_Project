using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;

public class SpaceShooterDatabaseManager : MonoBehaviour
{
    private CheckAuthentication script;

    // Start is called before the first frame update
    void Start()
    {
        script = GameObject.Find("CheckAuth").GetComponent<CheckAuthentication>();
        StartCoroutine(GetUserData());
    }

    IEnumerator GetUserData() {
        Debug.Log(script.GetUserId());
        var getTask = FirebaseDatabase.DefaultInstance
        .GetReference("SpaceShooter")
        .Child(script.GetUserId())
        .GetValueAsync();
        yield return new WaitUntil(() => getTask.IsCompleted || getTask.IsFaulted);
        if (getTask.IsCompleted) {
            Dictionary<string, object> results = (Dictionary<string, object>)getTask.Result.Value;
            string name = results["name"].ToString();
            string coins = results["coins"].ToString();
        }
    }
}
