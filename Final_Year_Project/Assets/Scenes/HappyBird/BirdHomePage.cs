using Firebase.Storage;
using System.Threading.Tasks;
using UnityEngine;

using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class BirdHomePage : MonoBehaviour {

    public GameObject StartBtn, QuitBtn, LevelPanel, LowBtn, MidBtn, HighBtn, BackBtn, QuizBtn;

    private void Start() {


    }

    public void Play()
    {
        LevelPanel.SetActive(true);
        StartBtn.SetActive(false);
        QuitBtn.SetActive(false);
        
    }

    public static async Task<bool> DonwloadJson()
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        string jsonFileName = "Math.json";
        StorageReference httpsReference = storage.GetReferenceFromUrl("gs://finalyearproject-cc646.appspot.com/"
            + "Math/" + jsonFileName);
        Debug.Log(Application.persistentDataPath + "/" + jsonFileName);
        var task = httpsReference.GetFileAsync(Application.persistentDataPath + "/" + jsonFileName);
        await task;
        return true;
    }


    public void Back()

    {
        LevelPanel.SetActive(false);
        StartBtn.SetActive(true);
        QuitBtn.SetActive(true);
    }
    public async void Low()

    {
        await DonwloadJson();
        SceneManager.LoadScene("Bird_Low_LevelSelect");
    }
    public void BackPage()

    {
        SceneManager.LoadScene("Main");
    }
    public void High()

    {

    }
    public void Quiz()

    {
        SceneManager.LoadScene("MathQuiz");
    }


}