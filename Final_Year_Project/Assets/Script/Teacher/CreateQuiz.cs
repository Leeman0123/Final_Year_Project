using Firebase.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CreateQuiz : MonoBehaviour
{

    private string _subjectOptions = "ChineseExtra";
    private string _savedQuizName = "";
    private string _jsonQuestion = "";
    private List<QuestionJson> qjList = new List<QuestionJson>();
    [Header("Quiz Panel")]
    public GameObject quizPanel;
    public Button createQuizBtn;
    public Button newButton;
    public Button closeBtn;

    [Header("Quiz Details")]
    public InputField quizName;
    public InputField quizDuration;
    public InputField coinsGain;
    public Button saveQuizSettingsBtn;
    public Button exitBtn;
    public GameObject quizDetailsPanel;
    private string _quizSetting;

    [Header("Quiz Questions")]
    public GameObject quizQuestionsPanel;
    public InputField question;
    public InputField quizCorrectAnswer;
    public InputField quizWrongAns1;
    public InputField quizWrongAns2;
    public InputField quizWrongAns3;
    public Button addQuestion;
    public Button saveQuiz;

    // Start is called before the first frame update
    void Start()
    {
        createQuizBtn.onClick.AddListener(() =>
        {
            quizPanel.SetActive(true);
        });
        newButton.onClick.AddListener(() =>
        {
            quizDetailsPanel.SetActive(true);
        });
        closeBtn.onClick.AddListener(() =>
        {
            quizPanel.SetActive(false);
        });
        saveQuizSettingsBtn.onClick.AddListener(() => SaveQuizSetting());
        exitBtn.onClick.AddListener(() => ExitQuizSetting());
        addQuestion.onClick.AddListener(() => AddQuestion());
        saveQuiz.onClick.AddListener(() => SaveQuiz());
    }

    void AddQuestion()
    {
        if (question.text == "" ||
            quizCorrectAnswer.text == "" ||
            quizWrongAns1.text == "" ||
            quizWrongAns2.text == "" ||
            quizWrongAns3.text == "")
        {
            GeneralScript.ShowErrorMessagePanel("Canvas", "You must input all the fields.");
            return;
        }
        QuestionJson qj = new QuestionJson();
        qj.questionTitle = question.text;
        qj.correctAns = quizCorrectAnswer.text;
        qj.wrongAns = new string[]{quizWrongAns1.text, quizWrongAns2.text, quizWrongAns3.text};
        qjList.Add(qj);
        var loadedObject = Resources.Load("General/QuestionRow");
        GameObject panel = GameObject.Instantiate(loadedObject) as GameObject;
        Text message = panel.transform.Find("QuestionAns").gameObject.GetComponent<Text>();
        Button deleteBtn = panel.transform.Find("DeleteBtn").gameObject.GetComponent<Button>();
        deleteBtn.onClick.AddListener(() =>
        {
            qjList.Remove(qj);
            GameObject.Destroy(deleteBtn.transform.parent.gameObject);

        });
        message.text = $"Question{qj.questionTitle}\nAns:{qj.correctAns}";
        panel.transform.SetParent(GameObject.Find("OhMyGod").transform, false);
        question.text = "";
        quizCorrectAnswer.text = "";
        quizWrongAns1.text = "";
        quizWrongAns2.text = "";
        quizWrongAns3.text = "";
        //_jsonQuestion = JsonHelper.ToJson(qjList.ToArray(), false);
    }

    async void SaveQuiz()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        string userid = auth.CurrentUser.UserId;
        _jsonQuestion = JsonHelper.ToJson(qjList.ToArray(), false);
        System.IO.File.WriteAllText(Application.persistentDataPath + $"/{_savedQuizName}.json", _quizSetting);
        await DbHelper.AddNewExtraQuiz(_subjectOptions, _savedQuizName, "true", userid);
        await CloudStorageHelper.UploadFileWithName(_subjectOptions, $"{_savedQuizName}.json", 0);
        await CloudStorageHelper.UploadFileWithName(_subjectOptions, $"{_savedQuizName}Coins.json", 1);
        quizQuestionsPanel.SetActive(false);
        StartCoroutine(verififyData());
    }

    IEnumerator verififyData()
    {
        GameObject myObj = GeneralScript.ShowMessagePanelWithTextLoading("Canvas", "Verifying data ");
        yield return new WaitForSeconds(0.9f);
        GameObject.Destroy(myObj);
    }

    async void SaveQuizSetting()
    {
        if (quizName.text == "" ||
            quizDuration.text == "" ||
            coinsGain.text == "")
        {
            GeneralScript.ShowErrorMessagePanel("Canvas", "You must input all the fields.");
            return;
        }
        QuizDetails qd = new QuizDetails(quizName.text, _subjectOptions, Convert.ToInt32(quizDuration.text), Convert.ToInt32(coinsGain.text));
        _quizSetting = JsonUtility.ToJson(qd);
        GameObject loadingPanel = GeneralScript.ShowMessagePanelWithTextLoading("Canvas", $"Setting's Details Quiz");
        _savedQuizName = quizName.text;
        ExtraQuizEntry eq = await DbHelper.GetExtraQuizByName(_subjectOptions, _savedQuizName);
        Debug.Log(eq == null);
        if (eq == null) {
            System.IO.File.WriteAllText(Application.persistentDataPath + $"/{_savedQuizName}Coins.json", _quizSetting);
            StartCoroutine(DelaySecondsClose(1.35f, loadingPanel));
        }
        else
        {
            GeneralScript.ShowErrorMessagePanel("Canvas", "Quiz already Exist");
        }

        
    }

    IEnumerator DelaySecondsClose(float sec, GameObject panel) {
        yield return new WaitForSeconds(sec);
        GameObject.Destroy(panel);
        GameObject.Destroy(quizDetailsPanel);
        quizQuestionsPanel.gameObject.SetActive(true);
    }

    public void DropDownOption(int val)
    {
        if (val==0)
        {
            _subjectOptions = "ChineseExtra";
        }
        else if (val == 1)
        {
            _subjectOptions = "EnglishExtra";
        }
        else
        {
            _subjectOptions = "MathematicsExtra";
        }
    }

    void ExitQuizSetting()
    {
        quizName.text = "";
        quizDuration.text = "";
        coinsGain.text = "";
        _savedQuizName = "";
        _quizSetting = "";
        quizDetailsPanel.SetActive(false);
    }


}
