using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Extensions;
using Firebase.Auth;

public class LogoutScript : MonoBehaviour
{

    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;
    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    // Start is called before the first frame update

    void Awake()
    {
        InitializeFirebase();
    }

    void InitializeFirebase() {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    public void LogOut() {
        auth.SignOut();
        var loadedObject = Resources.Load("General/ProgressBarPanelWithText");
        GameObject inObj = GameObject.Instantiate(loadedObject) as GameObject;
        inObj.transform.SetParent(GameObject.Find("Canvas").transform,false);
        inObj.GetComponent<ProgressBar>().scene = "StartMenu";
        inObj.GetComponent<ProgressBar>().message = "Signing Out....\n Redirecting to Start Menu.";
        inObj.SetActive(true);
    }

    void OnDestroy() {
        auth = null;
    }

}
