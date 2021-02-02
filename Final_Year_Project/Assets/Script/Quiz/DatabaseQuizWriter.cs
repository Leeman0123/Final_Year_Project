using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;

public class DatabaseQuizWriter : MonoBehaviour
{
    [SerializeField] GameObject progressPanel;
    private bool dataSaved = false;
    public void WriteVocabAnimals(string userId, int correctCount, string remainTime, int answerTotal)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("EnglishQuiz").Child("VocabAnimals").Child(userId).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.Log("Error");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Child("correctCount").Value == null) {
                    DatabaseReference mReference = FirebaseDatabase.DefaultInstance.RootReference;
                    VocabAnimals va = new VocabAnimals(correctCount, remainTime, answerTotal);
                    string json = JsonUtility.ToJson(va);
                    mReference.Child("EnglishQuiz").Child(va.GetType().Name).Child(userId).SetRawJsonValueAsync(json);
                }
                else if ((int.Parse(snapshot.Child("correctCount").Value.ToString())) < correctCount)
                {
                    
                        DatabaseReference mReference = FirebaseDatabase.DefaultInstance.RootReference;
                        VocabAnimals va = new VocabAnimals(correctCount, remainTime, answerTotal);
                        string json = JsonUtility.ToJson(va);
                        mReference.Child("EnglishQuiz").Child(va.GetType().Name).Child(userId).SetRawJsonValueAsync(json);
                }
                dataSaved = true;
            }
        });
    }

    public void WriteUserCoins(string userId, int coins)
    {
        DatabaseReference mReference = FirebaseDatabase.DefaultInstance.RootReference;
        mReference.Child("students").Child(userId).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.Log("Error");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Child("coins").Value == null)
                {
                    DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
                    Debug.Log(snapshot.Child("coins").Value);
                    string email = snapshot.Child("email").Value.ToString();
                    string name = snapshot.Child("name").Value.ToString();
                    Students students = new Students(
                        email,
                        name,
                        coins);
                    string json = JsonUtility.ToJson(students);
                    reference.Child("students").Child(userId).SetRawJsonValueAsync(json);
                }
                else if (snapshot.Child("coins").Value != null)
                {
                    DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
                    int currentCoins = int.Parse(snapshot.Child("coins").Value.ToString());
                    string email = snapshot.Child("email").Value.ToString();
                    string name = snapshot.Child("name").Value.ToString();
                    Debug.Log(currentCoins + "," + coins);
                    Students students = new Students(
                        email,
                        name,
                        (currentCoins + coins));
                    string json = JsonUtility.ToJson(students);
                    reference.Child("students").Child(userId).SetRawJsonValueAsync(json);
                }
            }
        });
       
    }

    void Update()
    {
        while (dataSaved)
        {
            BackToMainPage();
            dataSaved = false;
        }
    }

    public void BackToMainPage()
    {
        Debug.Log("Backing to the main page...");
        progressPanel.gameObject.SetActive(true);
    }
}
