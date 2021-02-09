using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;

public class DatabaseQuizWriter : MonoBehaviour
{
    [SerializeField] GameObject progressPanel;
    private bool dataSaved = false;

    void Start()
    {

    }
    public IEnumerator WriteVocabAnimals(string userId, int correctCount, string remainTime, int answerTotal)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        var getTask = reference
            .Child("EnglishQuiz")
            .Child("VocabAnimals")
            .Child(userId)
            .GetValueAsync();
        yield return new WaitUntil(() => getTask.IsCompleted || getTask.IsFaulted);
        if (getTask.IsCompleted)
        {
            Dictionary<string, object> results = (Dictionary<string, object>)getTask.Result.Value;
            if (results == null)
            {
                StartCoroutine(UpdateVocabAnimalsQuizTable(userId, correctCount, remainTime, answerTotal));
                StartCoroutine(WriteUserCoins(userId, 5));
            }
            else
            {
                int correctTotal = int.Parse(results["correctCount"].ToString());
                if (correctTotal < correctCount)
                {
                    StartCoroutine(UpdateVocabAnimalsQuizTable(userId, correctCount, remainTime, answerTotal));
                    StartCoroutine(WriteUserCoins(userId, 5));
                }
            }
            dataSaved = true;
        }
    }

    public IEnumerator WriteVocabVehicles(string userId, int correctCount, string remainTime, int answerTotal)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        var getTask = reference
            .Child("EnglishQuiz")
            .Child("VocabVehicle")
            .Child(userId)
            .GetValueAsync();
        yield return new WaitUntil(() => getTask.IsCompleted || getTask.IsFaulted);
        if (getTask.IsCompleted)
        {
            Dictionary<string, object> results = (Dictionary<string, object>)getTask.Result.Value;
            if (results == null)
            {
                StartCoroutine(UpdateVocabVehicleQuizTable(userId, correctCount, remainTime, answerTotal));
                StartCoroutine(WriteUserCoins(userId, 5));
            }
            else
            {
                int correctTotal = int.Parse(results["correctCount"].ToString());
                if (correctTotal < correctCount)
                {
                    StartCoroutine(UpdateVocabVehicleQuizTable(userId, correctCount, remainTime, answerTotal));
                    StartCoroutine(WriteUserCoins(userId, 5));
                }
            }
            dataSaved = true;
        }
    }

    public IEnumerator UpdateVocabAnimalsQuizTable(string userId, int correctCount, string remainTime, int answerTotal)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        VocabAnimals va = new VocabAnimals(correctCount, remainTime, answerTotal);
        string json = JsonUtility.ToJson(va);
        var getTask = reference.Child("EnglishQuiz").Child(va.GetType().Name).Child(userId).SetRawJsonValueAsync(json);
        yield return new WaitUntil(() => getTask.IsCompleted || getTask.IsFaulted);
    }

    public IEnumerator UpdateVocabVehicleQuizTable(string userId, int correctCount, string remainTime, int answerTotal)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        VocabVehicle va = new VocabVehicle(correctCount, remainTime, answerTotal);
        string json = JsonUtility.ToJson(va);
        var getTask = reference.Child("EnglishQuiz").Child(va.GetType().Name).Child(userId).SetRawJsonValueAsync(json);
        yield return new WaitUntil(() => getTask.IsCompleted || getTask.IsFaulted);
    }

    public IEnumerator WriteUserCoins(string userId, int coins)
    {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        var getTask = FirebaseDatabase.DefaultInstance
        .GetReference("students")
        .Child(userId)
        .GetValueAsync();
        yield return new WaitUntil(() => getTask.IsCompleted || getTask.IsFaulted);
        if (getTask.IsCompleted)
        {
            Dictionary<string, object> results = (Dictionary<string, object>)getTask.Result.Value;
            int currentCoins = int.Parse(results["coins"].ToString());
            var DBTask = reference.Child("students").Child(userId).Child("coins").SetValueAsync((currentCoins + coins));
            //Wait Until Modify DB task is completed;
            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to update student name task with {DBTask.Exception}");
            }
            else
            {
                Debug.Log("Updated Student coins as 0" + " with userID: " + userId);
            }
        }

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
