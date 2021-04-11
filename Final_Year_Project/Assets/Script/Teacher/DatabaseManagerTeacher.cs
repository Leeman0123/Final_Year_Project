using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;

public class DatabaseManagerTeacher : MonoBehaviour
{
    Firebase.Auth.FirebaseAuth auth;
    [SerializeField] GameObject requiredLoginPanel;
    void Start()
    {
        InitializeFirebase();
    }

    void InitializeFirebase() {
        FirebaseApp app = FirebaseApp.DefaultInstance;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        FirebaseDatabase.DefaultInstance.GetReference("teachers")
            .Child(auth.CurrentUser.UserId)
            .ValueChanged += HandleValueChanged;
    }

    private void HandleValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (e.DatabaseError != null)
        {
            Debug.LogError(e.DatabaseError.Message);
            return;
        }
        DataSnapshot snapshot = e.Snapshot;
        if (e.Snapshot.Child("email").Value != null)
        {
            string email = snapshot.Child("email").Value.ToString();
            string name = snapshot.Child("name").Value.ToString();
            GameObject.Find("Welcome Text").GetComponentInChildren<Text>()
                .text = "Welcome, " + name;
        }
        else
        {
            Debug.LogError("Cannot get user's data from database");
            return;
        }
        
    }

}
