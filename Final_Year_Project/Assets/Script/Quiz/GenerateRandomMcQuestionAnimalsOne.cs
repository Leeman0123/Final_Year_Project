using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using System.Threading;

public class GenerateRandomMcQuestionAnimalsOne : MonoBehaviour
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
    public Text messagePanelDescription;
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


    public static GenerateRandomMcQuestionAnimalsOne instance;
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
        messagePanelDescription.text = GameObject.Find("CoinsLevel").GetComponent<CoinsLevel>().description;
        messagePanelOkBtn.onClick.AddListener(() => StartQuiz());
        nextBtn.onClick.AddListener(() => ShowSavedDatabasePanel());
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
        if (questions.Count != 0)
        {
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
        else
        {
            ShowFinishPanel();
        }
    }

    async void ShowSavedDatabasePanel()
    {
        string userId = auth.CurrentUser.UserId;
        TimerCounter timerCounter = GameObject.Find("GameManager").GetComponent<TimerCounter>();
        EnglishQuizVocabAnimalsOne currentResult = await DbHelper.GetEngVocabAnimOneResultById(userId);
        Students student = await DbHelper.GetStudentById(userId);
        int studentCoins = student.coins;
        timeUsedText.text = $"Time used:{timerCounter.GetTimeCountString()}";
        savedCorrectCountText.text = $"{correctCount}/{totalQuestion}";
        int myTime =  timerCounter.GetSeconds();
        savedPanel.SetActive(true);
        int canGetCoins = GameObject.Find("CoinsLevel").GetComponent<CoinsLevel>().coins;
        int refreshCoins = GameObject.Find("CoinsLevel").GetComponent<CoinsLevel>().refreshRankCoins;
        int canAttemptTimes = GameObject.Find("CoinsLevel").GetComponent<CoinsLevel>().attempt;
        if (currentResult == null)
        {
            coinsGainText.text = $"Coins: +{canGetCoins}";
            attemptText.text = "The quiz result will be saved, and you have 2 more times to gain coins";
            bool success = await DbHelper.AddNewEngVocabOneResult(userId, correctCount, totalQuestion, myTime, canAttemptTimes);
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
            if (currentResult.attemptLeft > 0)
            {
                int currentResultAttemptLeft = currentResult.attemptLeft;
                if (correctCount > currentResult.correctCount)
                {
                    int newCoins = studentCoins + refreshCoins;
                    int newAttempt = currentResultAttemptLeft -1 ;
                    coinsGainText.text = $"Coins: +{refreshCoins}";
                    attemptText.text = $"The quiz result will be saved, and you have {currentResultAttemptLeft} times chance";
                    bool updateCoinSuccess = await DbHelper.UpdateStudentCoins(userId, newCoins);
                    bool updateCorrectCountSuccess = await DbHelper.UpdateAnimalsOneQuizCorrectCount(userId, correctCount);
                    bool updateTimesSuccess = await DbHelper.UpdateAnimalsOneQuizTimes(userId, myTime);
                    bool updateAttempTimesSuccess = await DbHelper.UpdateAnimalsOneQuizAttempt(userId, newAttempt);
                    if (updateCoinSuccess 
                        && updateCorrectCountSuccess
                        && updateTimesSuccess
                        && updateAttempTimesSuccess)
                    {
                        StartCoroutine(DelayForRedirectPanel(5));
                    }
                } else
                {
                    int newAttempt = currentResultAttemptLeft - 1;
                    coinsGainText.text = "Coins: +0";
                    attemptText.text = $"The quiz result will be saved, and you have {currentResultAttemptLeft} times chance";
                    bool updateAttempTimesSuccess = await DbHelper.UpdateAnimalsOneQuizAttempt(userId, newAttempt);
                    if (updateAttempTimesSuccess)
                    {
                        StartCoroutine(DelayForRedirectPanel(5));
                    }
                }
            }
            else
            {
                coinsGainText.text = "Coins: +0";
                attemptText.text = $"All quiz's attemp chance used. This result will not save";
                StartCoroutine(DelayForRedirectPanel(5));
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
            ShowFinishPanel();
        }
        
    }

    async void ShowFinishPanel()
    {
        TimerCounter timerCounter = GameObject.Find("GameManager").GetComponent<TimerCounter>();
        finishedPanel.SetActive(true);
        timerCounter.StopTimer();
        string userId = auth.CurrentUser.UserId;
        EnglishQuizVocabAnimalsOne currentResult = await DbHelper.GetEngVocabAnimOneResultById(userId);
        if (correctCount == totalQuestion)
        {
            awesome.SetActive(true);
            good.SetActive(false);
            amazing.SetActive(false);
        }
        scoreText.text = $"Score: {correctCount}/{totalQuestion}";
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
        GameObject.Find("GameManager").GetComponent<TimerCounter>().AddTime(8);
        GameObject.Find("GameManager").GetComponent<TimerCounter>().ReduceTimeAvailabe(8);
        incorrect.SetActive(true);
        Handheld.Vibrate();
        StartCoroutine(DelayShuffleCorrectBtn(1.25f));
    }

}
