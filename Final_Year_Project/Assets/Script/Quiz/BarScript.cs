using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
    private Image Bar;
    [SerializeField] float gameTime;
    [SerializeField] Text timerText;
    [SerializeField] Button stopBtn;
    [SerializeField] Text totalTimerText;
    [SerializeField] float timeLimit;
    [Header("Pause")]
    [SerializeField] GameObject pauseBackground;
    [SerializeField] GameObject pauseView;
    [SerializeField] Button resumeBtn;
    [SerializeField] Button leaveBtn;

    private bool stopTimer;
    private float MaxBarValue;
    private float time;
    private string remainTimeString;
    // Start is called before the first frame update
    void Awake()
    {
    }

    void Start()
    {
        Bar = GetComponent<Image>();
        stopTimer = true;
        MaxBarValue = gameTime;
        time = gameTime;
        stopBtn.onClick.AddListener(() => PauseGame());
        resumeBtn.onClick.AddListener(() => ResumeGame());
        leaveBtn.onClick.AddListener(() => LeaveQuiz());
    }

    // Update is called once per frame
    void Update()
    {
        //float time = gameTime - Time.time;
        if (!stopTimer)
        {
            time -= Time.deltaTime;
            float remainTime = timeLimit - Time.time;
            int remainTimeMinutes = Mathf.FloorToInt(remainTime / 60);
            int remainTimeSeconds = Mathf.FloorToInt(remainTime - remainTimeMinutes * 60f);
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time - minutes * 60f);
            Bar.fillAmount = time / MaxBarValue;
            if (Bar.fillAmount >= 0.7 && Bar.fillAmount <= 1.0)
            {
                Bar.color = new Color32(59, 219, 109, 255);
            }
            else if (Bar.fillAmount >= 0.3 && Bar.fillAmount <= 0.7)
            {
                Bar.color = new Color32(225, 209, 89, 255);
            }
            else if (Bar.fillAmount <= 0.3)
            {
                Bar.color = new Color32(236, 102, 107, 255);
                int count = GenerateQuestion.instance.GetCount();
                if (Bar.fillAmount <= 0 && count != 12 && count < 12)
                {
                    ResetTime();
                    Debug.Log("Count: " + GenerateQuestion.instance.GetCount());
                    GenerateQuestion.instance.GetNextQuestion();
                    //stopTimer = true;
                }
            }
            if (remainTime >= 0)
            {
                remainTimeString = string.Format("0{0}:{1}", remainTimeMinutes, remainTimeSeconds.ToString("00"));
                totalTimerText.text = string.Format("0{0}:{1}", remainTimeMinutes, remainTimeSeconds.ToString("00"));
            }
            timerText.text = seconds.ToString();

        }
    }

    public void StartTimer()
    {
        stopTimer = false;
    }

    void LeaveQuiz()
    {
        SceneManager.LoadSceneAsync("Main");
    }

    void PauseGame()
    {
        stopTimer = true;
        pauseBackground.gameObject.SetActive(true);
        pauseView.gameObject.SetActive(true);
    }

    void ResumeGame()
    {
        stopTimer = false;
        pauseBackground.gameObject.SetActive(false);
        pauseView.gameObject.SetActive(false);
    }

    public string GetRemainTime()
    {
        return remainTimeString;
    }

    public void StopTimer()
    {
        stopTimer = true;
    }

    public void ResetTime()
    {
        time = gameTime;
    }

    public void ReduceTime(float t)
    {
        time -= t;
    }
}
