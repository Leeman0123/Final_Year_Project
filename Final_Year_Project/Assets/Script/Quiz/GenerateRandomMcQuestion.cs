using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;

public class GenerateRandomMcQuestion : MonoBehaviour
{
    public string mcQuestionPath;
    public Stack<EnglishSpellingQuestion> questions;
    private CorrectBtn correctBtn;
    private int totalQuestion;
    private int correctCount;
    [Header("Answer Btn")]
    public Button[] answerBtn;
    [Header("Correct_Incorrect UI")]
    public GameObject correct;
    public GameObject incorrect;
    [Header("Ui")]
    public Text questionTitle;
    public Text socreText;
    public GameObject finishedPanel;
    public GameObject messagePanel;
    public Button messagePanelOkBtn;
    public Button messagePanelCloseBtn;
    [Header("Finished UI Panel")]
    public Text scoreText;
    public Text coinsText;
    public GameObject amazing;
    public GameObject good;
    public GameObject awesome;

    public static GenerateRandomMcQuestion instance;
    private FirebaseAuth auth;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        string json = File.ReadAllText(Application.persistentDataPath + "/" + mcQuestionPath);
        List<EnglishSpellingQuestion> questionsList = new List<EnglishSpellingQuestion>(JsonHelper.FromJson<EnglishSpellingQuestion>(json));
        totalQuestion = questionsList.Count;
        questionsList = questionsList.OrderBy(i => Guid.NewGuid()).ToList();
        questions = new Stack<EnglishSpellingQuestion>(questionsList);
        SetMessagePanelMessage();
        messagePanelCloseBtn.onClick.AddListener(() =>{
            messagePanelCloseBtn.gameObject.transform.parent.gameObject.SetActive(false);
            GeneralScript.RedirectPageWithT("Main", "Redirecting to the main page...", "Canvas");
            
        });
        messagePanelOkBtn.onClick.AddListener(() => StartQuiz());
        /*GenerateRandomQuestion();
        UpdateScore();*/
        if(instance == null)
        {
            instance = this;
        }
    }

    void SetMessagePanelMessage()
    {
        messagePanel.GetComponentInChildren<Text>().text = "This Test has " + totalQuestion + " questions\n\n" +
            "Each question has " + GameObject.Find("GameManager").GetComponent<TimerCounter>().time + " seconds to answer.";
    }

    void StartQuiz()
    {
        messagePanel.SetActive(false);
        GenerateRandomQuestion();
        GameObject.Find("GameManager").GetComponent<TimerCounter>().StartTimer();
        UpdateScore(); 
    }

    public void GenerateRandomQuestion()
    {
        foreach (Button btn in answerBtn)
        {
            btn.onClick.RemoveAllListeners();
        }
        int index = UnityEngine.Random.Range(0, answerBtn.Length);
        correctBtn = new CorrectBtn(index, answerBtn[index]);
        EnglishSpellingQuestion esq = questions.Peek();
        Stack<string> wrongAnsList = new Stack<string>(esq.wrongAns);
        questionTitle.text = esq.question;
        for (int i = 0; i < answerBtn.Length; i++)
        {
            if (answerBtn[i].Equals(correctBtn.button))
            {
                answerBtn[i].GetComponentInChildren<Text>().text = esq.correctAns;
                answerBtn[i].onClick.AddListener(() => correctBtnOperation());
            }
            else
            {
                answerBtn[i].GetComponentInChildren<Text>().text = wrongAnsList.Pop();
                answerBtn[i].onClick.AddListener(() => wrongBtnOperation());
            }
        }
    }

    void ShuffleCorrectBtn()
    {
        foreach(Button btn in answerBtn)
        {
            btn.onClick.RemoveAllListeners();
        }
        int index = UnityEngine.Random.Range(0, answerBtn.Length);
        correctBtn = new CorrectBtn(index, answerBtn[index]);
        EnglishSpellingQuestion esq = questions.Peek();
        Stack<string> wrongAnsList = new Stack<string>(esq.wrongAns);
        for (int i = 0; i < answerBtn.Length; i++)
        {
            if (answerBtn[i].Equals(correctBtn.button))
            {
                answerBtn[i].GetComponentInChildren<Text>().text = esq.correctAns;
                answerBtn[i].onClick.AddListener(() => correctBtnOperation());
            }
            else
            {
                answerBtn[i].GetComponentInChildren<Text>().text = wrongAnsList.Pop();
                answerBtn[i].onClick.AddListener(() => wrongBtnOperation());
            }
        }
    }

    async void correctBtnOperation()
    {
        Debug.Log("Correct Btn");
        correctCount++;
        correct.SetActive(true);
        Debug.Log("Correct count: " + correctCount);
        UpdateScore();
        TimerCounter timerCounter = GameObject.Find("GameManager").GetComponent<TimerCounter>();
        try
        {
            EnglishSpellingQuestion esq = questions.Pop();
            GenerateRandomQuestion();
            timerCounter.ResetTimer();
            StartCoroutine(DelayEnableAnswerBtn(1.25f));
        }
        catch (Exception)
        {
            finishedPanel.SetActive(true);
            timerCounter.StopTimer();
            string userId = auth.CurrentUser.UserId;
            EnglishQuizVocabAnimalsOne x =  await DbHelper.GetEngVocabAnimOneResultById(userId);
            if (correctCount == totalQuestion) {
                awesome.SetActive(true);
                good.SetActive(false);
                amazing.SetActive(false);
            }
            if (x == null) {
                Debug.Log("No record");
            }
        }
        
    }

    void UpdateScore()
    {
        socreText.text = string.Format("{0}/{1}", correctCount, totalQuestion);
    }

    void DisabledAnswerBtn()
    {
        for(int i=0;i<answerBtn.Length;i++)
        {
            answerBtn[i].interactable = false;
        }
    }

    void EnableAnswerBtn()
    {
        incorrect.SetActive(false);
        correct.SetActive(false);
        for (int i = 0; i < answerBtn.Length; i++)
        {
            answerBtn[i].interactable = true;
        }
    }

    IEnumerator DelayEnableAnswerBtn(float seconds)
    {
        DisabledAnswerBtn();
        yield return new WaitForSeconds(seconds);
        EnableAnswerBtn();
    }

    IEnumerator DelayShuffleCorrectBtn(float seconds)
    {
        DisabledAnswerBtn();
        ShuffleCorrectBtn();
        yield return new WaitForSeconds(seconds);
        EnableAnswerBtn();
    }

    void wrongBtnOperation()
    {
        Debug.Log("Wrong Btn");
        Debug.Log("Mobile phone vibration");
        GameObject.Find("GameManager").GetComponent<TimerCounter>().AddTime(5);
        GameObject.Find("GameManager").GetComponent<TimerCounter>().ReduceTimeAvailabe(5);
        incorrect.SetActive(true);
        Handheld.Vibrate();
        StartCoroutine(DelayShuffleCorrectBtn(1.25f));
    }

}
