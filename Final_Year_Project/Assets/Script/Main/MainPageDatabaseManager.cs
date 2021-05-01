using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using System;

public class MainPageDatabaseManager : MonoBehaviour
{
    [Header("LeaderBoard UI")]
    public GameObject LeaderBoardPanel;
    public GameObject ChineseLeaderBoard;
    public GameObject EnglishLeaderBoard;
    public GameObject MathematicsLeaderBoard;
    public GameObject LeaderBoardSubjectSelect;
    public Button leaderBoardBtn;
    public Button backBtnLeaderBoard;
    public Button chineseBtnL;
    public Button englishBtnL;
    public Button mathsBtnL;
    [Header("Chinese LeaderBoard Subject UI")]
    public Button fillIn;
    public Button unit;
    public Button fillInAdv;
    public Button headL;
    public Button idiom1L;
    public Button idiom2L;
    [Header("English LeaderBoard Subject UI")]
    public Button animals1;
    public Button animals2;
    public Button vehicle1;
    public Button vehicle2;
    public Button fillInTheBlanks;
    public Button preposition;
    [Header("Maths LeaderBoard Subject UI")]
    public Button addition;
    public Button subtract;
    public Button subAdd;
    public Button muDiv;
    public Button decimalBtn;
    public Button arith;
    [Header("Top10 leaderboard")]
    public GameObject top10ScrollView;
    public GameObject top10;

    void Start()
    {
        backBtnLeaderBoard.onClick.AddListener(() => HideAllLeaderBoard());
        leaderBoardBtn.onClick.AddListener(() => { LeaderBoardPanel.SetActive(true); });
        chineseBtnL.onClick.AddListener(() => ShowChinese());
        englishBtnL.onClick.AddListener(() => ShowEnglish());
        mathsBtnL.onClick.AddListener(() => ShowMathematics());
        fillIn.onClick.AddListener(() =>
        {
            ShowChineseTop10("ChineseFillIn", 1, 1);
        });
        unit.onClick.AddListener(() =>
        {
            ShowChineseTop10("ChineseUnit", 1, 1);
        });
        fillInAdv.onClick.AddListener(() =>
        {
            ShowChineseTop10("ChineseFillInAdv", 1, 2);
        });
        headL.onClick.AddListener(() =>
        {
            ShowChineseTop10("ChineseHead", 1, 2);
        });
        idiom1L.onClick.AddListener(() =>
        {
            ShowChineseTop10("ChineseIdiom", 1, 3);
        });
        idiom2L.onClick.AddListener(() =>
        {
            ShowChineseTop10("ChineseIdiom2", 1, 3);
        });




        animals1.onClick.AddListener(() =>
        {
            ShowChineseTop10("EnglishQuizVocabAnimalsOne", 2, 1);
        });
        vehicle1.onClick.AddListener(() =>
        {
            ShowChineseTop10("EnglishQuizVocabVehicleOne", 2, 1);
        });
        animals2.onClick.AddListener(() =>
        {
            ShowChineseTop10("EnglishQuizVocabAnimalsTwo", 2, 2);
        });
        vehicle2.onClick.AddListener(() =>
        {
            ShowChineseTop10("EnglishQuizVocabVehicleTwo", 2, 2);
        });
        fillInTheBlanks.onClick.AddListener(() =>
        {
            ShowChineseTop10("EnglishQuizCompleteSentences", 2, 3);
        });
        preposition.onClick.AddListener(() =>
        {
            ShowChineseTop10("EnglishQuizPreposition", 2, 3);
        });


        addition.onClick.AddListener(() =>
        {
            ShowChineseTop10("MathsQuizAddition", 3, 1);
        });
        subtract.onClick.AddListener(() =>
        {
            ShowChineseTop10("MathsQuizSubtract", 3, 1);
        });
        subAdd.onClick.AddListener(() =>
        {
            ShowChineseTop10("MathsQuizSubAdd", 3, 2);
        });
        muDiv.onClick.AddListener(() =>
        {
            ShowChineseTop10("MathsQuizMuDiv", 3, 2);
        });
        arith.onClick.AddListener(() =>
        {
            ShowChineseTop10("MathsQuizArith", 3, 3);
        });
        decimalBtn.onClick.AddListener(() =>
        {
            ShowChineseTop10("MathsQuizDecimal", 3, 3);
        });
    }

    void ShowChinese()
    {
        ChineseLeaderBoard.SetActive(true);
        EnglishLeaderBoard.SetActive(false);
        MathematicsLeaderBoard.SetActive(false);
        LeaderBoardSubjectSelect.SetActive(false);
        backBtnLeaderBoard.gameObject.SetActive(true);
    }

    void ShowEnglish()
    {
        ChineseLeaderBoard.SetActive(false);
        EnglishLeaderBoard.SetActive(true);
        MathematicsLeaderBoard.SetActive(false);
        LeaderBoardSubjectSelect.SetActive(false);
        backBtnLeaderBoard.gameObject.SetActive(true);
    }

    void ShowMathematics()
    {
        ChineseLeaderBoard.SetActive(false);
        EnglishLeaderBoard.SetActive(false);
        MathematicsLeaderBoard.SetActive(true);
        LeaderBoardSubjectSelect.SetActive(false);
        backBtnLeaderBoard.gameObject.SetActive(true);
    }

    void HideAllLeaderBoard()
    {
        ChineseLeaderBoard.SetActive(false);
        EnglishLeaderBoard.SetActive(false);
        MathematicsLeaderBoard.SetActive(false);
        top10.SetActive(false);
    }

    async void ShowChineseTop10(string testName, int subject, int primary)
    {
        string quizSubject = "";
        string primaryLevel = "";
        if (subject == 1)
        {
            quizSubject = "ChineseQuiz";
        }
        else if (subject == 2)
        {
            quizSubject = "EnglishQuiz";
        }
        else
        {
            quizSubject = "MathsQuiz";
        }
        if (primary == 1) {
            primaryLevel = "PrimaryOne";
        }
        else if (primary == 2)
        {
            primaryLevel = "PrimaryTwo";
        }
        else
        {
            primaryLevel = "PrimaryThree";
        }
        HideAllLeaderBoard();
        top10.SetActive(true);
        backBtnLeaderBoard.gameObject.SetActive(true);
        foreach (Transform child in top10ScrollView.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        top10ScrollView.SetActive(true);
        List<McQuestionQuiz> rankList = await DbHelper.GetQuizRank(testName, quizSubject, primaryLevel);
        for (int i=0; i<= rankList.Count - 1; i++)
        {
            Students student = await DbHelper.GetStudentById(rankList[i].uid);
            if (i == 0)
            {
                var loadedObject = Resources.Load("General/Rank1Panel");
                GameObject rankRow = GameObject.Instantiate(loadedObject) as GameObject;
                Text accuracy = rankRow.transform.Find("Accuracy").gameObject.GetComponent<Text>();
                Text timeUsed = rankRow.transform.Find("TimeUsed").gameObject.GetComponent<Text>();
                Text name = rankRow.transform.Find("Name").gameObject.GetComponent<Text>();
                name.text = student.name;
                TimeSpan time = TimeSpan.FromSeconds(rankList[i].timeCount);
                DateTime dateTime = DateTime.Today.Add(time);
                int mmss = Convert.ToInt32(rankList[i].correctCount / (float)rankList[i].questionsTotal * 100);

                accuracy.text = $"{mmss.ToString()}%";
                timeUsed.text = dateTime.ToString("mm:ss");
                rankRow.transform.SetParent(top10ScrollView.transform, false);

            }
            else if (i == 1)
            {
                var loadedObject = Resources.Load("General/Rank2Panel");
                GameObject rankRow = GameObject.Instantiate(loadedObject) as GameObject;
                Text accuracy = rankRow.transform.Find("Accuracy").gameObject.GetComponent<Text>();
                Text timeUsed = rankRow.transform.Find("TimeUsed").gameObject.GetComponent<Text>();
                Text name = rankRow.transform.Find("Name").gameObject.GetComponent<Text>();
                name.text = student.name;
                TimeSpan time = TimeSpan.FromSeconds(rankList[i].timeCount);
                DateTime dateTime = DateTime.Today.Add(time);
                int mmss = Convert.ToInt32(rankList[i].correctCount / (float)rankList[i].questionsTotal * 100);

                accuracy.text = $"{mmss.ToString()}%";
                timeUsed.text = dateTime.ToString("mm:ss");
                rankRow.transform.SetParent(top10ScrollView.transform, false);

            }
            else if (i == 2)
            {
                var loadedObject = Resources.Load("General/Rank3Panel");
                GameObject rankRow = GameObject.Instantiate(loadedObject) as GameObject;
                Text accuracy = rankRow.transform.Find("Accuracy").gameObject.GetComponent<Text>();
                Text timeUsed = rankRow.transform.Find("TimeUsed").gameObject.GetComponent<Text>();
                Text name = rankRow.transform.Find("Name").gameObject.GetComponent<Text>();
                name.text = student.name;
                TimeSpan time = TimeSpan.FromSeconds(rankList[i].timeCount);
                DateTime dateTime = DateTime.Today.Add(time);
                int mmss = Convert.ToInt32(rankList[i].correctCount / (float)rankList[i].questionsTotal * 100);

                accuracy.text = $"{mmss.ToString()}%";
                timeUsed.text = dateTime.ToString("mm:ss");
                rankRow.transform.SetParent(top10ScrollView.transform, false);

            } 
            else
            {
                var loadedObject = Resources.Load("General/NormalLeaderBoardPanel");
                GameObject rankRow = GameObject.Instantiate(loadedObject) as GameObject;
                Text rankText = rankRow.transform.Find("RankText").gameObject.GetComponent<Text>();
                rankText.text = $"{(i + 1).ToString()}.";
                Text accuracy = rankRow.transform.Find("Accuracy").gameObject.GetComponent<Text>();
                Text timeUsed = rankRow.transform.Find("TimeUsed").gameObject.GetComponent<Text>();
                Text name = rankRow.transform.Find("Name").gameObject.GetComponent<Text>();
                name.text = student.name;
                TimeSpan time = TimeSpan.FromSeconds(rankList[i].timeCount);
                DateTime dateTime = DateTime.Today.Add(time);
                int mmss = Convert.ToInt32(rankList[i].correctCount / (float)rankList[i].questionsTotal * 100);

                accuracy.text = $"{mmss.ToString()}%";
                timeUsed.text = dateTime.ToString("mm:ss");
                rankRow.transform.SetParent(top10ScrollView.transform, false);
            }
        }
    }


}
