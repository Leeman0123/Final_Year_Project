using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Proyecto26;
using TMPro;

public class RegisterScript : MonoBehaviour
{
    [SerializeField] TMP_InputField studentName;
    [SerializeField] TMP_InputField studentEmail;
    [SerializeField] TMP_InputField studentPassword;
    [SerializeField] Button registerBtn;
    private int selectedIndex = 0;
    protected Firebase.Auth.FirebaseAuth auth;
    string authKey = "AIzaSyCf_KWb7BLMrDhDZmi_LH_sXZks03QvPI0";
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
        dependencyStatus = task.Result;
        if (dependencyStatus == DependencyStatus.Available) {
          InitializeFirebase();
        } else {
          Debug.LogError(
            "Could not resolve all Firebase dependencies: " + dependencyStatus);
        }
      });
        registerBtn.onClick.AddListener(() =>
        {
            AddAccount();
        });
    }

    void InitializeFirebase() {
        FirebaseApp app = FirebaseApp.DefaultInstance;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public void AddAccount() {
        if (studentName.text == ""
            || studentEmail.text == "" 
            || studentPassword.text == ""
            || !IsValidEmail(studentEmail.text)
            || studentPassword.text.Length < 8)
        {
            GeneralScript.ShowErrorMessagePanel("Canvas", "Make sure the account info are correct.");
            return;
        }
        SignUpUser(studentEmail.text, studentPassword.text);
    }

    async void SignUpUser(string email, string password)
    {
        string userdata = "{\"email\": \"" + email + "\", \"password\": " +
            "\"" + password + "\", \"returnSecureToken\": true}";
        HashSet<string> emailList = await DbHelper.GetAllUserEmail();
        foreach (string existEmail in emailList)
        {
            if (email.Equals(existEmail, StringComparison.InvariantCultureIgnoreCase))
            {
                GeneralScript.ShowErrorMessagePanel("Canvas", "The Email is already Exist!");
                studentEmail.text = "";
                studentName.text = "";
                studentPassword.text = "";
                return;
            }
        }
        RestClient.Post<SignUpResponse>("https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=" + authKey,
            userdata).Then(async(response) =>
            {
                string localId = response.localId;
                if (selectedIndex == 0)
                {
                    await DbHelper.AddNewStudent(localId, email, studentName.text);
                }
                else if (selectedIndex == 1)
                {
                    await DbHelper.AddNewTeacher(localId, email, studentName.text);
                }
                studentEmail.text = "";
                studentName.text = "";
                studentPassword.text = "";
                GeneralScript.ShowMessagePanelWithTick("Canvas", "Create account success!");
            }).Catch(error =>
            {
                Debug.LogError(error);
            });
    }

    public void HandleInputData(int val)
    {
        if (val == 0)
        {
            selectedIndex = 0;
        }
        else if (val == 1)
        {
            selectedIndex = 1;
        }
        Debug.Log("Selected Index:" + selectedIndex);
    }


}
