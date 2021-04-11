using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class CheckAuthentication : MonoBehaviour
{
    private FirebaseUser user;
    private FirebaseAuth auth;
    [SerializeField] GameObject[] interactiveScript;
    private bool setInteractiveScript = true;

    void Awake()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                InitializeFirebase();
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else //
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }


    // Start is called before the first frame update

    public void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }



    public bool IsLogined()
    {
        return auth.CurrentUser != null;
    }

    public void Logout()
    {
        if (auth.CurrentUser != null)
        {
            auth.SignOut();
        }
    }

    public string GetUserId()
    {
        string userId = null;
        if (auth.CurrentUser != null)
        {
            userId =  auth.CurrentUser.UserId;
        }
        return userId;
    }
    void BackToLoginScene()
    {
        SceneManager.LoadScene("StartMenu");
    }

    // Update is called once per frame
    void Update()
    {
        if (auth != null)
        {
            if (setInteractiveScript)
            {
                foreach (GameObject obj in interactiveScript) {
                    obj.SetActive(true);
                }
                setInteractiveScript = false;
            }
        }
    }
}
