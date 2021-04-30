using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerCounter : MonoBehaviour
{
    private bool stopTimer = true;
    private float timeCount = 0;
    [Header("Bar")]
    public Image barColorArea;
    [Header("Minutes Timer UI")]
    public Text minutesTimer;
    [Header("Seconds Timer UI")]
    public Text secondsTimer;
    [Header("Timer control")]
    public float time;
    private float constTime;
    private float maxBarValue;
    [Header("Pause UI")]
    public GameObject pausePanel;
    public Button continueBtn;
    public Button pauseBtn;
    public Button leaveBtn;


    void Start()
    {
        pauseBtn.onClick.AddListener(() =>
        {
            StopTimer();
            pausePanel.SetActive(true);
        });
        continueBtn.onClick.AddListener(() =>
        {
            stopTimer = false;
            pausePanel.SetActive(false);
        });
        leaveBtn.onClick.AddListener(() =>
        {
            GeneralScript.RedirectPageWithT("Main", "Redirecting to main page...", "Canvas");
        });
        maxBarValue = time;
        constTime = time;
    }

    void Update()
    {
        if (!stopTimer)
        {
            time -= Time.deltaTime;
            timeCount += Time.deltaTime;
            int timeToSeconds = System.Convert.ToInt32(time);
            int timeCountToMinutes = Mathf.FloorToInt(timeCount / 60);
            int timeCountToSeconds = Mathf.FloorToInt(timeCount - timeCountToMinutes * 60f);
            minutesTimer.text = string.Format("{0}:{1}", timeCountToMinutes.ToString("00"), timeCountToSeconds.ToString("00"));
            secondsTimer.text = timeToSeconds.ToString();
            barColorArea.fillAmount = time / maxBarValue;
            if (barColorArea.fillAmount >= 0.7 && barColorArea.fillAmount <= 1.0)
            {
                barColorArea.color = new Color32(59, 219, 109, 255);
            }
            else if (barColorArea.fillAmount >= 0.3 && barColorArea.fillAmount <= 0.7)
            {
                barColorArea.color = new Color32(225, 209, 89, 255);
            }
            else if (barColorArea.fillAmount <= 0.3)
            {
                barColorArea.color = new Color32(236, 102, 107, 255);
                if (barColorArea.fillAmount <= 0)
                {
                    ResetTimer();
                    if (SceneManager.GetActiveScene().name == "Animals1")
                    {
                        GenerateRandomMcQuestionAnimalsOne.instance.questions.Pop();
                        GenerateRandomMcQuestionAnimalsOne.instance.GenerateRandomQuestion();
                    }
                    else if (SceneManager.GetActiveScene().name == "Vehicle1")
                    {
                        GenerateRandomMcQuestionVehicleOne.instance.questions.Pop();
                        GenerateRandomMcQuestionVehicleOne.instance.GenerateRandomQuestion();
                    }
                    else if (SceneManager.GetActiveScene().name == "Vehicle2")
                    {
                        GenerateRandomMcQuestionVehicleTwo.instance.questions.Pop();
                        GenerateRandomMcQuestionVehicleTwo.instance.GenerateRandomQuestion();
                    }
                    else if (SceneManager.GetActiveScene().name == "Animals2") {
                        GenerateRandomMcQuestionAnimalsTwo.instance.questions.Pop();
                        GenerateRandomMcQuestionAnimalsTwo.instance.GenerateRandomQuestion();
                    }

                    else if (SceneManager.GetActiveScene().name == "CompleteSentences")
                    {
                        GenerateRandomMcQuestionComSentences.instance.questions.Pop();
                        GenerateRandomMcQuestionComSentences.instance.GenerateRandomQuestion();
                    }

                    else if (SceneManager.GetActiveScene().name == "Preposition")
                    {
                        GenerateRandomMcQuestionPreposition.instance.questions.Pop();
                        GenerateRandomMcQuestionPreposition.instance.GenerateRandomQuestion();
                    }
                    else if (SceneManager.GetActiveScene().name == "Addition")
                    {
                        GenerateRandomMcQuestionMathsAddition.instance.questions.Pop();
                        GenerateRandomMcQuestionMathsAddition.instance.GenerateRandomQuestion();
                    }
                    else if (SceneManager.GetActiveScene().name == "Subtract")
                    {
                        GenerateRandomMcQuestionMathsSubtract.instance.questions.Pop();
                        GenerateRandomMcQuestionMathsSubtract.instance.GenerateRandomQuestion();
                    }
                    else if (SceneManager.GetActiveScene().name == "MuDiv")
                    {
                        GenerateRandomMcQuestionMathsMuDiv.instance.questions.Pop();
                        GenerateRandomMcQuestionMathsMuDiv.instance.GenerateRandomQuestion();
                    }
                    else if (SceneManager.GetActiveScene().name == "SubAdd")
                    {
                        GenerateRandomMcQuestionMathsSubAdd.instance.questions.Pop();
                        GenerateRandomMcQuestionMathsSubAdd.instance.GenerateRandomQuestion();
                    }
                    else if (SceneManager.GetActiveScene().name == "Decimal")
                    {
                        GenerateRandomMcQuestionMathsDecimal.instance.questions.Pop();
                        GenerateRandomMcQuestionMathsDecimal.instance.GenerateRandomQuestion();
                    }
                    else if (SceneManager.GetActiveScene().name == "Arithmetic")
                    {
                        GenerateRandomMcQuestionMathsArith.instance.questions.Pop();
                        GenerateRandomMcQuestionMathsArith.instance.GenerateRandomQuestion();
                    }
                }
            }
        }
    }

    public void ResetTimer()
    {
        time = constTime;
    }

    public void AddTime(float seconds) {
        timeCount += seconds;
    }

    public string GetTimeCountString()
    {
        int timeCountToMinutes = Mathf.FloorToInt(timeCount / 60);
        int timeCountToSeconds = Mathf.FloorToInt(timeCount - timeCountToMinutes * 60f);
        string tc = string.Format("{0}:{1}", timeCountToMinutes.ToString("00"), timeCountToSeconds.ToString("00"));
        return tc;
    }

    public int GetMinutes()
    {
        int timeCountToMinutes = Mathf.FloorToInt(timeCount / 60);
        return timeCountToMinutes;
    }

    public int GetSeconds() {
        int timeCountToMinutes = Mathf.FloorToInt(timeCount / 60);
        int timeCountToSeconds = Mathf.FloorToInt(timeCount - timeCountToMinutes * 60f);
        return timeCountToSeconds;
    }

    public void ReduceTimeAvailabe(float seconds) {
        time -= seconds;
    }

    public void StartTimer()
    {
        stopTimer = false;
    }

    public void StopTimer()
    {
        stopTimer = true;

    }
}
