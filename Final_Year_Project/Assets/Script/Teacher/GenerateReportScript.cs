using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using System;
using System.Threading.Tasks;
using TMPro;

public class GenerateReportScript : MonoBehaviour
{
    // Start is called before the first frame update
    private List<McQuestionQuiz> mcChinese = new List<McQuestionQuiz>();
    private List<McQuestionQuiz> mcEnglish = new List<McQuestionQuiz>();
    private List<McQuestionQuiz> mcMathematics = new List<McQuestionQuiz>();
    private HashSet<Students> stds = new HashSet<Students>();
    public GameObject tableContent;
    public GameObject tableContentViewForReport;
    public Button searchBtn;
    public Text fullname;
    public Text email;
    public Text coins;
    public Text accuracy;
    public Text SubjectText;
    public GameObject detailsPanel;
    public GameObject searchView;
    public InputField searchInput;
    public Button searchBtn2;
    private int subject = 0;
    private string _myUid;
    void Start()
    {
        searchBtn.onClick.AddListener(() =>
        {
            searchView.SetActive(true);
        });
        searchBtn2.onClick.AddListener(() => {
            if (searchInput.text == "") {
                GeneralScript.ShowErrorMessagePanel("Canvas", "You must input the email.");
                searchView.SetActive(false);
                searchInput.text = "";
                return;
            }
            Students targetStudent = QueryStudent(searchInput.text);
            if (targetStudent == null)
            {
                GeneralScript.ShowErrorMessagePanel("Canvas", "Cannot find the student.");
                searchView.SetActive(false);
                searchInput.text = "";
                return;
            }
            else {
                searchView.SetActive(false);
                searchInput.text = "'";
                detailsPanel.gameObject.SetActive(true);
                Button chineseBtn = detailsPanel.transform.Find("ChineseBtn").GetComponent<Button>();
                chineseBtn.onClick.AddListener(() => GetChineseStudentDetails(targetStudent.uid));
                Button englishBtn = detailsPanel.transform.Find("EnglishBtn").GetComponent<Button>();
                englishBtn.onClick.AddListener(() => GetEnglishStudentDetails(targetStudent.uid));
                Button mathsBtn = detailsPanel.transform.Find("MathsBtn").GetComponent<Button>();
                mathsBtn.onClick.AddListener(() => GetMathsStudentDetails(targetStudent.uid));
                GetChineseStudentDetails(targetStudent.uid);
            }
        });
        AddStudentDatabaseListener();
    }

    private Students QueryStudent(string text)
    {
        foreach (Students std in stds)
        {
            if (std.email == text)
            {
                return std;
            }
        }
        return null;
    }

    void AddStudentDatabaseListener()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("students")
            .ValueChanged += HandledDatabaseValueChanged;
    }

    void AddChineseQuizListener()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("ChineseExtra")
            .ValueChanged += HandledQuizResultValueChanged;
        FirebaseDatabase.DefaultInstance
            .GetReference("EnglishExtra")
            .ValueChanged -= HandledQuizResultValueChanged;
        FirebaseDatabase.DefaultInstance
            .GetReference("MathematicsExtra")
            .ValueChanged -= HandledQuizResultValueChanged;
    }

    void AddEnglishQuizListener()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("ChineseExtra")
            .ValueChanged -= HandledQuizResultValueChanged;
        FirebaseDatabase.DefaultInstance
            .GetReference("EnglishExtra")
            .ValueChanged += HandledQuizResultValueChanged;
        FirebaseDatabase.DefaultInstance
            .GetReference("MathematicsExtra")
            .ValueChanged -= HandledQuizResultValueChanged;
    }

    void AddMathsQuizListener()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("ChineseExtra")
            .ValueChanged -= HandledQuizResultValueChanged;
        FirebaseDatabase.DefaultInstance
            .GetReference("EnglishExtra")
            .ValueChanged -= HandledQuizResultValueChanged;
        FirebaseDatabase.DefaultInstance
            .GetReference("MathematicsExtra")
            .ValueChanged += HandledQuizResultValueChanged;
    }

    private void HandledQuizResultValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (e.DatabaseError != null)
        {
            Debug.LogError(e.DatabaseError);
            return;
        }
        else
        {
            DataSnapshot dataList = e.Snapshot;
            foreach (DataSnapshot data in dataList.Children)
            {
                string subjectName = data.Key.ToString();
                string json = data.Child(_myUid).GetRawJsonValue();
                if (json == null)
                {
                    continue;
                }
                else
                {
                    McQuestionQuiz x = JsonUtility.FromJson<McQuestionQuiz>(json);
                    Debug.Log(x.uid);
                    mcChinese.Add(x);
                }
            }
        }
    }

    private void HandledDatabaseValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (e.DatabaseError != null)
        {
            Debug.LogError(e.DatabaseError);
            return;
        }
        else
        {
            ClearRowsInTable();
            stds.Clear();
            DataSnapshot studentsDataList = e.Snapshot;
            int count = 1;
            foreach (DataSnapshot data in studentsDataList.Children)
            {
                var loadedObject = Resources.Load("Teacher/EditAccount/DataRow");
                GameObject dataRow = GameObject.Instantiate(loadedObject) as GameObject;
                Text no = dataRow.transform.Find("No").gameObject.GetComponent<Text>();
                Text fullName = dataRow.transform.Find("Fullname").gameObject.GetComponent<Text>();
                Text email = dataRow.transform.Find("Email").gameObject.GetComponent<Text>();
                no.text = count.ToString();
                fullName.text = data.Child("name").Value.ToString();
                email.text = data.Child("email").Value.ToString();
                string uid = data.Child("uid").Value.ToString();
                Students std = new Students();
                std.uid = uid;
                std.email = data.Child("email").Value.ToString();
                stds.Add(std);
                Button detailsBtn = dataRow.transform.Find("DetailsBtn").gameObject.GetComponent<Button>();
                detailsBtn.onClick.AddListener(() =>
                {
                    detailsPanel.gameObject.SetActive(true);
                    Button chineseBtn = detailsPanel.transform.Find("ChineseBtn").GetComponent<Button>();
                    chineseBtn.onClick.AddListener(() => GetChineseStudentDetails(uid));
                    Button englishBtn = detailsPanel.transform.Find("EnglishBtn").GetComponent<Button>();
                    englishBtn.onClick.AddListener(() => GetEnglishStudentDetails(uid));
                    Button mathsBtn = detailsPanel.transform.Find("MathsBtn").GetComponent<Button>();
                    mathsBtn.onClick.AddListener(() => GetMathsStudentDetails(uid));
                    GetChineseStudentDetails(uid);

                });
                dataRow.transform.SetParent(tableContent.transform, false);
                Debug.Log(string.Format("Append user to table name: {0}, email: {1}", fullName.text, email.text));
                count++;
            }
        }
    }

    private void ClearRowsInTable()
    {
        int tableRowCount = tableContent.transform.childCount;
        for (int i = 0; i < tableRowCount; i++)
        {
            GameObject.Destroy(tableContent
                .transform
                .GetChild(i)
                .gameObject);
            Debug.Log(string.Format("Deleted table row {0}", i));
        }
    }

    public void GetChineseStudentDetails(string uid)
    {
        _myUid = uid;
        GetAllChineseResult(uid);
    }

    public void GetEnglishStudentDetails(string uid)
    {
        _myUid = uid;
        GetAllEnglishResult(uid);
    }

    public void GetMathsStudentDetails(string uid)
    {
        _myUid = uid;
        GetAllMathsResult(uid);
    }


    public async void GetAllMathsResult(string uid)
    {
        mcMathematics.Clear();
        mcChinese.Clear();
        mcEnglish.Clear();
        ClearRowsInTableReport();
        AddMathsQuizListener();
        McQuestionQuiz m = new McQuestionQuiz();
        m = await DbHelper.GetMathsP1AdditionResultById(uid);
        if (m != null) { mcMathematics.Add(m); }
        m = await DbHelper.GetMathsP1SubtractResultById(uid);
        if (m != null) { mcMathematics.Add(m); }
        m = await DbHelper.GetMathsP2MuDivResultById(uid);
        if (m != null) { mcMathematics.Add(m); }
        m = await DbHelper.GetMathsP2SubAddResultById(uid);
        if (m != null) { mcMathematics.Add(m); }
        m = await DbHelper.GetMathsP3ArithResultById(uid);
        if (m != null) { mcMathematics.Add(m); }
        m = await DbHelper.GetMathsP3DecimalResultById(uid);
        if (m != null) { mcMathematics.Add(m); }
        int count = 0;
        foreach (McQuestionQuiz mqq in mcMathematics)
        {
            count++;
            var loadedObject = Resources.Load("Teacher/DataRowForReport");
            GameObject panel = GameObject.Instantiate(loadedObject) as GameObject;
            Text message = panel.transform.Find("Text").gameObject.GetComponent<Text>();
            message.text = $"quiz{count}: {mqq.correctCount}/{mqq.questionsTotal}";
            panel.transform.SetParent(tableContentViewForReport.transform, false);
        }
        accuracy.text = $"Quiz Answered: {mcMathematics.Count}";
        Students student = await DbHelper.GetStudentById(uid);
        Debug.Log(string.Format("Retrieve data: Full Name: {0}, Email: {1}", student.name, student.email));
        fullname.text = string.Format("Full Name: {0}", student.name);
        email.text = string.Format("Email: {0}", student.email);
        coins.text = string.Format("Coins: {0}", student.coins);
        SubjectText.text = "Mathematics";
    }

    public async void GetAllEnglishResult(string uid)
    {
        mcMathematics.Clear();
        mcChinese.Clear();
        mcEnglish.Clear();
        ClearRowsInTableReport();
        AddEnglishQuizListener();
        McQuestionQuiz m = new McQuestionQuiz();
        m = await DbHelper.GetEngVocabAnimOneResultById(uid);
        if (m != null) { mcEnglish.Add(m); }
        m = await DbHelper.GetEngVocabAnimalsTwoResultById(uid);
        if (m != null) { mcEnglish.Add(m); }
        m = await DbHelper.GetEngVocabVehicleOneResultById(uid);
        if (m != null) { mcEnglish.Add(m); }
        m = await DbHelper.GetEngVocabVehicleTwoResultById(uid);
        if (m != null) { mcEnglish.Add(m); }
        m = await DbHelper.GetEngP3PrepositionResultById(uid);
        if (m != null) { mcEnglish.Add(m); }
        m = await DbHelper.GetEngP3CompleteSentencesResultById(uid);
        if (m != null) { mcEnglish.Add(m); }
        int count = 0;
        foreach (McQuestionQuiz mqq in mcEnglish)
        {
            count++;
            var loadedObject = Resources.Load("Teacher/DataRowForReport");
            GameObject panel = GameObject.Instantiate(loadedObject) as GameObject;
            Text message = panel.transform.Find("Text").gameObject.GetComponent<Text>();
            message.text = $"quiz{count}: {mqq.correctCount}/{mqq.questionsTotal}";
            panel.transform.SetParent(tableContentViewForReport.transform, false);
        }
        accuracy.text = $"Quiz Answered: {mcEnglish.Count}";
        Students student = await DbHelper.GetStudentById(uid);
        Debug.Log(string.Format("Retrieve data: Full Name: {0}, Email: {1}", student.name, student.email));
        fullname.text = string.Format("Full Name: {0}", student.name);
        email.text = string.Format("Email: {0}", student.email);
        coins.text = string.Format("Coins: {0}", student.coins);
        SubjectText.text = "English";
    }

    public async void GetAllChineseResult(string uid)
    {
        mcMathematics.Clear();
        mcChinese.Clear();
        mcEnglish.Clear();
        ClearRowsInTableReport();
        AddChineseQuizListener();
        int accuracyP = 0;
        McQuestionQuiz m = new McQuestionQuiz();
        m = await DbHelper.GetChineseP1FillInResultById(uid);
        if (m != null) { mcChinese.Add(m); }
        m=await DbHelper.GetChineseP1UnitResultById(uid);
        if (m != null) { mcChinese.Add(m); }
        m =await DbHelper.GetChineseP2FillInAdvResultById(uid);
        if (m != null) { mcChinese.Add(m); }
        m =await DbHelper.GetChineseP2HeadResultById(uid);
        if (m != null) { mcChinese.Add(m); }
        m =await DbHelper.GetChineseP3Idiom2ResultById(uid);
        if (m != null) { mcChinese.Add(m); }
        m =await DbHelper.GetChineseP3IdiomResultById(uid);
        if (m != null) { mcChinese.Add(m); }
        int count = 0;
        foreach (McQuestionQuiz mqq in mcChinese) {
            count++;
            var loadedObject = Resources.Load("Teacher/DataRowForReport");
            GameObject panel = GameObject.Instantiate(loadedObject) as GameObject;
            Text message = panel.transform.Find("Text").gameObject.GetComponent<Text>();
            message.text = $"quiz{count}: {mqq.correctCount}/{mqq.questionsTotal}";
            panel.transform.SetParent(tableContentViewForReport.transform, false);
        }
        accuracy.text = $"Quiz Answered: {mcChinese.Count}";
        Students student = await DbHelper.GetStudentById(uid);
        Debug.Log(string.Format("Retrieve data: Full Name: {0}, Email: {1}", student.name, student.email));
        fullname.text = string.Format("Full Name: {0}", student.name);
        email.text = string.Format("Email: {0}", student.email);
        coins.text = string.Format("Coins: {0}", student.coins);
        SubjectText.text = "Chinese";
    }

    private void ClearRowsInTableReport()
    {
        int tableRowCount = tableContentViewForReport.transform.childCount;
        for (int i = 0; i < tableRowCount; i++)
        {
            GameObject.Destroy(tableContentViewForReport
                .transform
                .GetChild(i)
                .gameObject);
            Debug.Log(string.Format("Deleted table row {0}", i));
        }

    }


    public async void TestMethod()
    {
        HashSet<string> emailList = await DbHelper.GetAllUserEmail();
        foreach(string email in emailList)
        {
            Debug.Log("email: " + email);
        }
    }
}
