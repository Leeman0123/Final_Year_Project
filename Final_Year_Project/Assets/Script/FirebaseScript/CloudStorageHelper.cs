using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Storage;
using System.Threading;
using System;
using System.Threading.Tasks;

public class CloudStorageHelper : MonoBehaviour
{
    private static readonly string finalReferenceUrl = "gs://finalyearproject-cc646.appspot.com/";
    private static readonly string _animalsMcQuiz = "AnimalsMc/";
    private static readonly string _vehicleMcQuiz = "VehicleMc/";
    public static async Task<bool> DownloadAnimalsP1QuizConisDetails() {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        string jsonFileName = "VocabularyAnimalsCoins.json";
        StorageReference httpsReference = storage.GetReferenceFromUrl(finalReferenceUrl 
            + _animalsMcQuiz + jsonFileName);
        GeneralScript.ShowDownloadPanel("Canvas", "Connecting to the server...");
        Debug.Log(Application.persistentDataPath + "/" + jsonFileName);
        var task = httpsReference.GetFileAsync(Application.persistentDataPath + "/" + jsonFileName);
        GeneralScript.DisplayDownloadStateForDownloadPanel(jsonFileName);
        await task;
        GeneralScript.DestroyDownloadPanel();
        if (task.Exception != null) {
            GeneralScript.ShowErrorMessagePanel("Canvas", "Download data: " + jsonFileName + " failed.");
            return false;
        }
        await Task.Delay(1000);
        return true;
    }

    public static async Task<bool> DownloadVehicleP1QuizConisDetails()
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        string jsonFileName = "VocabularyVehicleCoins1.json";
        StorageReference httpsReference = storage.GetReferenceFromUrl(finalReferenceUrl
            + _vehicleMcQuiz + jsonFileName);
        GeneralScript.ShowDownloadPanel("Canvas", "Connecting to the server...");
        Debug.Log(Application.persistentDataPath + "/" + jsonFileName);
        var task = httpsReference.GetFileAsync(Application.persistentDataPath + "/" + jsonFileName);
        GeneralScript.DisplayDownloadStateForDownloadPanel(jsonFileName);
        await task;
        GeneralScript.DestroyDownloadPanel();
        if (task.Exception != null)
        {
            GeneralScript.ShowErrorMessagePanel("Canvas", "Download data: " + jsonFileName + " failed.");
            return false;
        }
        await Task.Delay(1000);
        return true;
    }

    public static async Task<bool> DownloadAnimalsP1Quiz()
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        string jsonFileName = "VocabularyAnimals.json";
        StorageReference httpsReference = storage.GetReferenceFromUrl(finalReferenceUrl
            + _animalsMcQuiz + jsonFileName);
        GeneralScript.ShowDownloadPanel("Canvas", "Connecting to the server...");
        Debug.Log(Application.persistentDataPath + "/" + jsonFileName);
        GeneralScript.DisplayDownloadStateForDownloadPanel("Extra files");
        var task = httpsReference.GetFileAsync(Application.persistentDataPath + "/" + jsonFileName);
        await task;
        GeneralScript.DestroyDownloadPanel();
        if (task.Exception != null)
        {
            GeneralScript.ShowErrorMessagePanel("Canvas", "Download data: " + jsonFileName + " failed.");
            return false;
        }
        await Task.Delay(1000);
        return true;
    }

    public static async Task<bool> DownloadVehicleP1Quiz()
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        string jsonFileName = "VocabularyVehicle.json";
        StorageReference httpsReference = storage.GetReferenceFromUrl(finalReferenceUrl
            + _vehicleMcQuiz + jsonFileName);
        GeneralScript.ShowDownloadPanel("Canvas", "Connecting to the server...");
        Debug.Log(Application.persistentDataPath + "/" + jsonFileName);
        GeneralScript.DisplayDownloadStateForDownloadPanel("Extra files");
        var task = httpsReference.GetFileAsync(Application.persistentDataPath + "/" + jsonFileName);
        await task;
        GeneralScript.DestroyDownloadPanel();
        if (task.Exception != null)
        {
            GeneralScript.ShowErrorMessagePanel("Canvas", "Download data: " + jsonFileName + " failed.");
            return false;
        }
        await Task.Delay(1000);
        return true;
    }

    private static void DisplayDownloadState(DownloadState downloadState)
    {
            Debug.Log(String.Format("Downloading {0}: {1} out of {2}", downloadState.Reference.Name,
                                   downloadState.BytesTransferred, downloadState.TotalByteCount));
    }
}
