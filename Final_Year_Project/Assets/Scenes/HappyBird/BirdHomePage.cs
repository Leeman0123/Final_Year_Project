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

    public void Back()

    {
        LevelPanel.SetActive(false);
        StartBtn.SetActive(true);
        QuitBtn.SetActive(true);
    }
    public void Low()

    {
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