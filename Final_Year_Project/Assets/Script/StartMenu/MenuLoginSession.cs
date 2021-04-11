using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;

public class MenuLoginSession : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseUser user;
    [Header("Account Info")]
    [SerializeField] Text accountText;
    [Header("StartMenu UI")]
    private GameObject ui;
    [SerializeField] Button loginBtn;
    [Header("UI settings")]
    [SerializeField] Button button;
    [SerializeField] float duration;
    [SerializeField] GameObject mainMenuGroup;
    

    void Start() {
        InitializeFireBase();
        button.onClick.AddListener(() => {
            auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            auth.SignOut();
            loginBtn.gameObject.SetActive(false);
        });
    }

    void InitializeFireBase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += (object sender, System.EventArgs e) => {
            if (auth.CurrentUser != user)
            {
                bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
                user = auth.CurrentUser;
                if (signedIn)
                {
                    Debug.Log(user.UserId);
                    accountText.text = string.Format("Account ID: {0}, Logging in", user.UserId);
                    StartCoroutine(ShowLoginButton());
                }
            }
        };
    }

    IEnumerator ShowLoginButton() {
        ui = GameObject.Find("MenuButtonGroup");
        ui.SetActive(false);
        loginBtn.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        this.gameObject.SetActive(false);
    }

    public void ShowMainMenuGroup()
    {
        mainMenuGroup.SetActive(true);
    }
}
