using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine.UI;


public class AddQuestiom : MonoBehaviour
{
    [SerializeField] InputField question;
    [SerializeField] InputField a;
    [SerializeField] InputField b;
    [SerializeField] InputField c;
    [SerializeField] Dropdown subject;
    private static DatabaseReference mDatabaseRef;
    // Start is called before the first frame update
    void Awake()
    {
        //Check下有冇import 到Firebase同call 到佢, 再將佢嘅狀態放去Task呢個variable到
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            //將Task嘅狀態放去dependencyStatus呢個variable度
            var dependencyStatus = task.Result;
            //如果狀態係Ready嘅話
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
                // 自動check table改咗(Server check)
            }

        });
    }

    public static void WriteNewUser(string userId, string name, string email)
    {
        //將Class 轉做JSON 格式
        //{ 
        //    "username" : name,
        //    "email" : email
        //}
        string json = JsonUtility.ToJson("ads");
        //拎User做primary key再塞個JSON落database 去users table度 
        mDatabaseRef.Child("users").Child(userId).SetRawJsonValueAsync(json);
    }

}
