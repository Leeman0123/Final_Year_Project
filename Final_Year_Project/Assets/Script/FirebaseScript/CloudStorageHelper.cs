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

    public static async Task<bool> DownloadVehicleP2Quiz()
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        string jsonFileName = "VocabularyVehicle2.json";
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

    public static async Task<bool> DownloadVehicleP2QuizConisDetails()
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        string jsonFileName = "VocabularyVehicleCoins2.json";
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

    public static async Task<bool> DownloadAnimalsP2QuizConisDetails()
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        string jsonFileName = "VocabularyAnimalsCoins2.json";
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

    public static async Task<bool> DownloadAnimalsP2Quiz()
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        string jsonFileName = "VocabularyAnimals2.json";
        StorageReference httpsReference = storage.GetReferenceFromUrl(finalReferenceUrl
            + _animalsMcQuiz + jsonFileName);
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

    public static async Task<bool> DownloadP3EngCompleteSenCoinsDetails()
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        string jsonFileName = "FillInTheBlanksCoins.json";
        StorageReference httpsReference = storage.GetReferenceFromUrl(finalReferenceUrl
            + "P3EnglishMc/" + jsonFileName);
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

    public static async Task<bool> DownloadP3EngCompleteSenQuizDetails()
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        string jsonFileName = "FillInTheBlanks.json";
        StorageReference httpsReference = storage.GetReferenceFromUrl(finalReferenceUrl
            + "P3EnglishMc/" + jsonFileName);
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

    public static async Task<bool> DownloadP3EngPrepositionCoinsDetails()
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        string jsonFileName = "PrepositionCoins.json";
        StorageReference httpsReference = storage.GetReferenceFromUrl(finalReferenceUrl
            + "P3EnglishMc/" + jsonFileName);
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

    public static async Task<bool> DownloadP3EngPrepositionQuizDetails()
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        string jsonFileName = "Preposition.json";
        StorageReference httpsReference = storage.GetReferenceFromUrl(finalReferenceUrl
            + "P3EnglishMc/" + jsonFileName);
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

    public static async Task<bool> DownloadP1MathsAdditionCoinsDetails()
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        string jsonFileName = "AdditionCoins.json";
        StorageReference httpsReference = storage.GetReferenceFromUrl(finalReferenceUrl
            + "P1MathMc/" + jsonFileName);
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

    public static async Task<bool> DownloadP1MathsAdditionQuizDetails()
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        string jsonFileName = "Addition.json";
        StorageReference httpsReference = storage.GetReferenceFromUrl(finalReferenceUrl
            + "P1MathMc/" + jsonFileName);
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

    public static async Task<bool> DownloadP1MathsSubtractCoinsDetails()
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        string jsonFileName = "SubtractCoins.json";
        StorageReference httpsReference = storage.GetReferenceFromUrl(finalReferenceUrl
            + "P1MathMc/" + jsonFileName);
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

    public static async Task<bool> DownloadP1MathsSubtractQuizDetails()
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        string jsonFileName = "Subtract.json";
        StorageReference httpsReference = storage.GetReferenceFromUrl(finalReferenceUrl
            + "P1MathMc/" + jsonFileName);
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

    public static async Task<bool> DownloadP2MathsMuDivCoinsDetails()
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        string jsonFileName = "MuDivCoins.json";
        StorageReference httpsReference = storage.GetReferenceFromUrl(finalReferenceUrl
            + "P2MathMc/" + jsonFileName);
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

    public static async Task<bool> DownloadP2MathsMuDivQuizDetails()
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        string jsonFileName = "MuDiv.json";
        StorageReference httpsReference = storage.GetReferenceFromUrl(finalReferenceUrl
            + "P2MathMc/" + jsonFileName);
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

    public static async Task<bool> DownloadP2MathsSubAddCoinsDetails()
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        string jsonFileName = "SubAdditCoins.json";
        StorageReference httpsReference = storage.GetReferenceFromUrl(finalReferenceUrl
            + "P2MathMc/" + jsonFileName);
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

    public static async Task<bool> DownloadP2MathsSubAddQuizDetails()
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        string jsonFileName = "SubAddit.json";
        StorageReference httpsReference = storage.GetReferenceFromUrl(finalReferenceUrl
            + "P2MathMc/" + jsonFileName);
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

    public static async Task<bool> DownloadP3MathsArithmeticCoinsDetails()
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        string jsonFileName = "ArithmeticCoins.json";
        StorageReference httpsReference = storage.GetReferenceFromUrl(finalReferenceUrl
            + "P3MathMc/" + jsonFileName);
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

    public static async Task<bool> DownloadP3MathsArithmeticQuizDetails()
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        string jsonFileName = "Arithmetic.json";
        StorageReference httpsReference = storage.GetReferenceFromUrl(finalReferenceUrl
            + "P3MathMc/" + jsonFileName);
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

    public static async Task<bool> DownloadP3MathsDecimalCoinsDetails()
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        string jsonFileName = "DecimalCoins.json";
        StorageReference httpsReference = storage.GetReferenceFromUrl(finalReferenceUrl
            + "P3MathMc/" + jsonFileName);
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

    public static async Task<bool> DownloadP3MathsDecimalQuizDetails()
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        string jsonFileName = "Decimal.json";
        StorageReference httpsReference = storage.GetReferenceFromUrl(finalReferenceUrl
            + "P3MathMc/" + jsonFileName);
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

    private static void DisplayDownloadState(DownloadState downloadState)
    {
            Debug.Log(String.Format("Downloading {0}: {1} out of {2}", downloadState.Reference.Name,
                                   downloadState.BytesTransferred, downloadState.TotalByteCount));
    }
}
