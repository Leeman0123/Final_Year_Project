using Firebase.Database;
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
    private List<ExtraQuizEntry> eqListForChinese = new List<ExtraQuizEntry>();
    private List<ExtraQuizEntry> eqListForEnglish = new List<ExtraQuizEntry>();
    private List<ExtraQuizEntry> eqListForMaths = new List<ExtraQuizEntry>();

    [Header("Available Quiz")]
    public GameObject availableQuizScrollView;

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
        AddListener();
    }

    void AddListener()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("ChineseExtra")
            .ValueChanged += HandledDatabaseValueChanged;
        FirebaseDatabase.DefaultInstance
            .GetReference("EnglishExtra")
            .ValueChanged += HandledDatabaseValueChanged;
        FirebaseDatabase.DefaultInstance
            .GetReference("MathematicsExtra")
            .ValueChanged += HandledDatabaseValueChanged;
    }

    private void HandledDatabaseValueChanged(object sender, ValueChangedEventArgs e)
    {
        ClearRowsInTable();
        if (e.DatabaseError != null)
        {
            Debug.LogError(e.DatabaseError);
            return;
        }
        else
        {
            DataSnapshot dataList = e.Snapshot;
            string subjectName = dataList.Key;
            if (subjectName == "ChineseExtra")
            {
                eqListForChinese.Clear();
            }
            else if (subjectName == "EnglishExtra")
            {
                eqListForEnglish.Clear();
            }
            else if (subjectName == "MathematicsExtra")
            {
                eqListForMaths.Clear();
            }
            foreach (DataSnapshot data in dataList.Children)
            {
                if (subjectName == "ChineseExtra")
                {
                    ExtraQuizEntry eqe = JsonUtility.FromJson<ExtraQuizEntry>(data.GetRawJsonValue());
                    eqListForChinese.Add(eqe);
                }
                else if (subjectName == "EnglishExtra")
                {
                    ExtraQuizEntry eqe = JsonUtility.FromJson<ExtraQuizEntry>(data.GetRawJsonValue());
                    eqListForEnglish.Add(eqe);
                }
                else if (subjectName == "MathematicsExtra")
                {
                    ExtraQuizEntry eqe = JsonUtility.FromJson<ExtraQuizEntry>(data.GetRawJsonValue());
                    eqListForMaths.Add(eqe);
                }
            }
            UpdateRowsInTable();
        }
    }

    void UpdateRowsInTable()
    {
        foreach (ExtraQuizEntry chineseData in eqListForChinese)
        {
            var loadedObject = Resources.Load("Teacher/MainPage/AvaiableQuizList");
            GameObject panel = GameObject.Instantiate(loadedObject) as GameObject;
            Text subject = panel.transform.Find("SubjectType").gameObject.GetComponent<Text>();
            subject.text = "Chinese";
            Text quizName = panel.transform.Find("QuizName").gameObject.GetComponent<Text>();
            quizName.text = chineseData.quizName;
            panel.transform.SetParent(availableQuizScrollView.transform, false);
            Button detailsBtn = panel.transform.Find("Details").gameObject.GetComponent<Button>();
            detailsBtn.onClick.AddListener(async () =>
            {
                var loadedObject2 = Resources.Load("Teacher/MainPage/DetailsQuiz");
                GameObject object2 = GameObject.Instantiate(loadedObject2) as GameObject;
                object2.transform.SetParent(GameObject.Find("Canvas").transform, false);
                Text myQuizName = object2.transform.Find("QuizName1").gameObject.GetComponent<Text>();
                myQuizName.text = chineseData.quizName;
                Text teacherName = object2.transform.Find("TeacherName").gameObject.GetComponent<Text>();
                teacherName.text = await DbHelper.GetExtraQuizUploaderById(chineseData.uid);
                Button myBtnn = object2.transform.Find("OkButton").gameObject.GetComponent<Button>();
                Dropdown dp = object2.transform.Find("Enable").gameObject.GetComponent<Dropdown>();
                string st = await DbHelper.GetQuizStatus("ChineseExtra", chineseData.quizName);
                if (st == "true")
                {
                    dp.value = 0;
                }
                else if (st=="false")
                {
                    dp.value = 1;
                }
                myBtnn.onClick.AddListener(async() =>
                {
                    EnableQuiz eqE = object2.transform.Find("Enable").gameObject.GetComponent<EnableQuiz>();
                    string enableStatus = eqE._enableStatus;
                    Debug.Log(enableStatus);
                    await DbHelper.UpdateQuizStatus("ChineseExtra", chineseData.quizName, enableStatus);
                    GameObject.Destroy(object2);
                });
            });
            
        }
        foreach (ExtraQuizEntry englishData in eqListForEnglish)
        {
            var loadedObject = Resources.Load("Teacher/MainPage/AvaiableQuizList");
            GameObject panel = GameObject.Instantiate(loadedObject) as GameObject;
            Text subject = panel.transform.Find("SubjectType").gameObject.GetComponent<Text>();
            subject.text = "English";
            Text quizName = panel.transform.Find("QuizName").gameObject.GetComponent<Text>();
            quizName.text = englishData.quizName;
            panel.transform.SetParent(availableQuizScrollView.transform, false);
            Button detailsBtn = panel.transform.Find("Details").gameObject.GetComponent<Button>();
            detailsBtn.onClick.AddListener(async () =>
            {
                var loadedObject2 = Resources.Load("Teacher/MainPage/DetailsQuiz");
                GameObject object2 = GameObject.Instantiate(loadedObject2) as GameObject;
                object2.transform.SetParent(GameObject.Find("Canvas").transform, false);
                Text myQuizName = object2.transform.Find("QuizName1").gameObject.GetComponent<Text>();
                myQuizName.text = englishData.quizName;
                Text teacherName = object2.transform.Find("TeacherName").gameObject.GetComponent<Text>();
                teacherName.text = await DbHelper.GetExtraQuizUploaderById(englishData.uid);
                EnableQuiz eqE = object2.transform.Find("Enable").gameObject.GetComponent<EnableQuiz>();
                string enableStatus = eqE._enableStatus;
                Button myBtnn = object2.transform.Find("OkButton").gameObject.GetComponent<Button>();
                Dropdown dp = object2.transform.Find("Enable").gameObject.GetComponent<Dropdown>();
                string st = await DbHelper.GetQuizStatus("ChineseExtra", englishData.quizName);
                if (st == "true")
                {
                    dp.value = 0;
                }
                else if (st == "false")
                {
                    dp.value = 1;
                }
                myBtnn.onClick.AddListener(async () =>
                {
                    await DbHelper.UpdateQuizStatus("EnglishExtra", englishData.quizName, enableStatus);
                    GameObject.Destroy(object2);
                });
            });
        }
        foreach (ExtraQuizEntry mathsData in eqListForMaths)
        {
            var loadedObject = Resources.Load("Teacher/MainPage/AvaiableQuizList");
            GameObject panel = GameObject.Instantiate(loadedObject) as GameObject;
            Text subject = panel.transform.Find("SubjectType").gameObject.GetComponent<Text>();
            subject.text = "Math";
            Text quizName = panel.transform.Find("QuizName").gameObject.GetComponent<Text>();
            quizName.text = mathsData.quizName;
            panel.transform.SetParent(availableQuizScrollView.transform, false);
            Button detailsBtn = panel.transform.Find("Details").gameObject.GetComponent<Button>();
            detailsBtn.onClick.AddListener(async () =>
            {
                var loadedObject2 = Resources.Load("Teacher/MainPage/DetailsQuiz");
                GameObject object2 = GameObject.Instantiate(loadedObject2) as GameObject;
                object2.transform.SetParent(GameObject.Find("Canvas").transform, false);
                Text myQuizName = object2.transform.Find("QuizName1").gameObject.GetComponent<Text>();
                myQuizName.text = mathsData.quizName;
                Text teacherName = object2.transform.Find("TeacherName").gameObject.GetComponent<Text>();
                teacherName.text = await DbHelper.GetExtraQuizUploaderById(mathsData.uid);
                EnableQuiz eqE = object2.transform.Find("Enable").gameObject.GetComponent<EnableQuiz>();
                string enableStatus = eqE._enableStatus;
                Button myBtnn = object2.transform.Find("OkButton").gameObject.GetComponent<Button>();
                Dropdown dp = object2.transform.Find("Enable").gameObject.GetComponent<Dropdown>();
                string st = await DbHelper.GetQuizStatus("ChineseExtra", mathsData.quizName);
                if (st == "true")
                {
                    dp.value = 0;
                }
                else if (st == "false")
                {
                    dp.value = 1;
                }
                myBtnn.onClick.AddListener(async () =>
                {
                    await DbHelper.UpdateQuizStatus("MathematicsExtra", mathsData.quizName, enableStatus);
                    GameObject.Destroy(object2);
                });
            });
        }
    }

    private void ClearRowsInTable()
    {
        int tableRowCount = availableQuizScrollView.transform.childCount;
        for (int i = 0; i < tableRowCount; i++)
        {
            GameObject.Destroy(availableQuizScrollView
                .transform
                .GetChild(i)
                .gameObject);
            Debug.Log(string.Format("Deleted table row {0}", i));
        }
        
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
        foreach (Transform x in GameObject.Find("OhMyGod").transform) {
            Destroy(x.gameObject);
        }
        quizQuestionsPanel.SetActive(false);
        StartCoroutine(verififyData());
    }

    IEnumerator verififyData()
    {
        GameObject myObj = GeneralScript.ShowMessagePanelWithTextLoading("Canvas", "Verifying data ");
        yield return new WaitForSeconds(0.9f);
        GameObject.Destroy(myObj);
        question.text = "";
        quizCorrectAnswer.text = "";
        quizWrongAns1.text = "";
        quizWrongAns2.text = "";
        quizWrongAns3.text = "";
        _jsonQuestion = "";
        quizName.text = "";
        quizDuration.text = "";
        coinsGain.text = "";
        _subjectOptions = "ChineseExtra";
        qjList.Clear();
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
        quizDetailsPanel.SetActive(false);
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
