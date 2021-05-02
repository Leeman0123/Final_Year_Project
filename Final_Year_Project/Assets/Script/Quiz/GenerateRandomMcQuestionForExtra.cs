using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using System.Threading;

public class GenerateRandomMcQuestionForExtra : MonoBehaviour
{
    private string __type;
    private string mcQuestionPath;
    public Stack<ExtraQuizQuestions> questions;
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
    public GameObject amazing;
    public GameObject good;
    public GameObject awesome;
    public GameObject congradMessageContain;
    public Text newRecordText;
    public Text previousResultText;
    public Button nextBtn;
    [Header("SavedPanel")]
    public GameObject savedPanel;
    public Text timeUsedText;
    public Text savedCorrectCountText;
    public Text coinsGainText;
    public Text attemptText;


    public static GenerateRandomMcQuestionForExtra instance;
    private FirebaseAuth auth;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        ExtraCoinsLevel ebb = GameObject.Find("ExtraCoinsLevel").GetComponent<ExtraCoinsLevel>();
        mcQuestionPath = ebb.quizName + ".json";
        __type = ebb.quizSubject;
        string json = File.ReadAllText(Application.persistentDataPath + "/" + mcQuestionPath);
        List<ExtraQuizQuestions> questionsList = new List<ExtraQuizQuestions>(JsonHelper.FromJson<ExtraQuizQuestions>(json));
        totalQuestion = questionsList.Count;
        questionsList = questionsList.OrderBy(i => Guid.NewGuid()).ToList();
        questions = new Stack<ExtraQuizQuestions>(questionsList);
        messagePanelCloseBtn.onClick.AddListener(() =>{
            messagePanelCloseBtn.gameObject.transform.parent.gameObject.SetActive(false);
            GeneralScript.RedirectPageWithT("Main", "Redirecting to the main page...", "Canvas");
            
        });
        messagePanelOkBtn.onClick.AddListener(() => StartQuiz());
        nextBtn.onClick.AddListener(() => ShowSavedDatabasePanel());
        /*GenerateRandomQuestion();
        UpdateScore();*/
        if(instance == null)
        {
            instance = this;
        }
        SetMessagePanelMessage();
    }

    void SetMessagePanelMessage()
    {
        messagePanel.GetComponentInChildren<Text>().text = "This Test has " + totalQuestion + " questions\n\n" +
            "Each question has " + GameObject.Find("GameManager").GetComponent<TimerCounterForExtraQuiz>().time + " seconds to answer.";
    }

    void StartQuiz()
    {
        messagePanel.SetActive(false);
        GenerateRandomQuestion();
        GameObject.Find("GameManager").GetComponent<TimerCounterForExtraQuiz>().StartTimer();
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
        if (questions.Count != 0)
        {
            ExtraQuizQuestions esq = questions.Peek();
            Stack<string> wrongAnsList = new Stack<string>(esq.wrongAns);
            questionTitle.text = esq.questionTitle;
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
        else
        {
            ShowFinishPanel();
        }
    }

    async void ShowSavedDatabasePanel()
    {
        string userId = auth.CurrentUser.UserId;
        string quizName = GameObject.Find("ExtraCoinsLevel").GetComponent<ExtraCoinsLevel>().quizName;
        string quizSubject = GameObject.Find("ExtraCoinsLevel").GetComponent<ExtraCoinsLevel>().quizSubject;
        TimerCounterForExtraQuiz timerCounter = GameObject.Find("GameManager").GetComponent<TimerCounterForExtraQuiz>();
        McQuestionQuiz currentResult = await DbHelper.GetExtraQuizResult(userId, quizSubject, quizName);
        Students student = await DbHelper.GetStudentById(userId);
        int studentCoins = student.coins;
        timeUsedText.text = $"Time used:{timerCounter.GetTimeCountString()}";
        savedCorrectCountText.text = $"{correctCount}/{totalQuestion}";
        int myTime =  timerCounter.GetSeconds();
        savedPanel.SetActive(true);
        int canGetCoins = GameObject.Find("ExtraCoinsLevel").GetComponent<ExtraCoinsLevel>().coinsGain;
        int refreshCoins = 0;
        int canAttemptTimes = 1;
        if (currentResult == null)
        {
            coinsGainText.text = $"Coins: +{canGetCoins}";
            attemptText.text = $"The quiz result will be saved, and you have {(canAttemptTimes-1)} more times to gain coins";
            bool success = await DbHelper.AddNewExtraQuizResult(userId, correctCount, totalQuestion, myTime, (canAttemptTimes-1), quizSubject, quizName);
            int newstudentCoins = studentCoins + canGetCoins;
            bool updateCoinSuccess = await DbHelper.UpdateStudentCoins(userId, newstudentCoins);
            if (!success || !updateCoinSuccess)
            {
                GeneralScript.ShowErrorMessagePanel("Canvas", "Cannot connect to the firebase server");
            }
            else if (success && updateCoinSuccess) {
                StartCoroutine(DelayForRedirectPanel(5));
            }
        }
        else
        {
                coinsGainText.text = "Coins: +0";
                attemptText.text = $"All quiz's attemp chance used. This result will not save";
                StartCoroutine(DelayForRedirectPanel(5));
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
        ExtraQuizQuestions esq = questions.Peek();
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

    IEnumerator DelayForRedirectPanel(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(savedPanel);
        GeneralScript.RedirectPageWithT("Main", "Redirecting to the main page..", "Canvas");
    }

    void correctBtnOperation()
    {
        Debug.Log("Correct Btn");
        correctCount++;
        correct.SetActive(true);
        Debug.Log("Correct count: " + correctCount);
        UpdateScore();
        TimerCounterForExtraQuiz timerCounter = GameObject.Find("GameManager").GetComponent<TimerCounterForExtraQuiz>();
        try
        {
            ExtraQuizQuestions esq = questions.Pop();
            GenerateRandomQuestion();
            timerCounter.ResetTimer();
            StartCoroutine(DelayEnableAnswerBtn(1.25f));
        }
        catch (Exception)
        {
            Debug.Log(correctCount);
            ShowFinishPanel();
        }
        
    }

    async void ShowFinishPanel()
    {
        TimerCounterForExtraQuiz timerCounter = GameObject.Find("GameManager").GetComponent<TimerCounterForExtraQuiz>();
        finishedPanel.SetActive(true);
        timerCounter.StopTimer();
        scoreText.text = $"Score: {correctCount}/{totalQuestion}";
        string userId = auth.CurrentUser.UserId;
        string quizName = GameObject.Find("ExtraCoinsLevel").GetComponent<ExtraCoinsLevel>().quizName;
        string quizSubject = GameObject.Find("ExtraCoinsLevel").GetComponent<ExtraCoinsLevel>().quizSubject;
        McQuestionQuiz currentResult = await DbHelper.GetExtraQuizResult(userId, quizSubject, quizName);
        if (correctCount == totalQuestion)
        {
            awesome.SetActive(true);
            good.SetActive(false);
            amazing.SetActive(false);
        }
        if (currentResult == null)
        {
            newRecordText.text = "New record Added";
            newRecordText.rectTransform.localPosition = new Vector3(0, -15, 0);
            congradMessageContain.GetComponent<RectTransform>().localPosition = new Vector3(0, 109, 0);
        }
        else
        {
            string previousResult = currentResult.correctCount.ToString();
            string lastQuizQuestions = currentResult.questionsTotal.ToString();
            previousResultText.text = $"Previous Result: {previousResult}/{lastQuizQuestions}";
            previousResultText.gameObject.SetActive(true);
            if (correctCount > currentResult.correctCount)
            {
                newRecordText.text = "You did better than your last quiz!!";
            }
            else {
                newRecordText.text = "You did worse than your last quiz!!";
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
        GameObject.Find("GameManager").GetComponent<TimerCounterForExtraQuiz>().AddTime(8);
        GameObject.Find("GameManager").GetComponent<TimerCounterForExtraQuiz>().ReduceTimeAvailabe(8);
        incorrect.SetActive(true);
        Handheld.Vibrate();
        StartCoroutine(DelayShuffleCorrectBtn(1.25f));
    }

}
