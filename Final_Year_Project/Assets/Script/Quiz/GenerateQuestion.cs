using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class GenerateQuestion : MonoBehaviour
{
    // Start is called before the first frame update
    public Button btn;
    private Button correctAnsBtn;
    private List<Question> questionsList = new List<Question>();
    private int skippedIndex;
    private int count = 0;
    private int correct = 0;
    private int questionsTotal;
    public static GenerateQuestion instance;
    [SerializeField] Button[] ansBtn;
    [SerializeField] Text questionTitle;
    [SerializeField] Text scoreUI;
    [SerializeField] GameObject finishedView;
    [SerializeField] string jsonPath;
    private string userId;



    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        string json = File.ReadAllText(Application.dataPath + jsonPath);
        Question[] questions = JsonHelper.FromJson<Question>(json);
        foreach (Question q in questions) {
            questionsList.Add(q);
        }
        questionsTotal = questions.Length;
        GenerateQuest();
        UpdateScore();
        userId = GameObject.Find("CheckAuth").GetComponent<CheckAuthentication>().GetUserId();
        Debug.Log("UserId: " + userId);
    }

    void Start()
    {
        ansBtn[0].onClick.AddListener(() => CheckAnswer(ansBtn[0]));
        ansBtn[1].onClick.AddListener(() => CheckAnswer(ansBtn[1]));
        ansBtn[2].onClick.AddListener(() => CheckAnswer(ansBtn[2]));
        ansBtn[3].onClick.AddListener(() => CheckAnswer(ansBtn[3]));
    }

    void UpdateScore()
    {
        scoreUI.text = string.Format("{0}/{1}", correct, questionsTotal);
    }

    void CheckAnswer(Button button)
    {
        BarScript script = GameObject.Find("BarBackground").GetComponentInChildren<BarScript>();
        if (button == correctAnsBtn)
        {
            Debug.Log("Correct Answer");
            if (count < 12)
            {
                script.ResetTime();
                GetNextQuestion();
                correct++;
                UpdateScore();
            }
            else if (count == 12) {
                count++;
                correct++;
                UpdateScore();
            }
            
        }
        else
        {
            Debug.Log("Wrong Answer");
            Debug.Log("Count:" + count);
            if (count < 12)
            {
                script.ResetTime();
                GetNextQuestion();
            }
            else if (count == 12)
            {
                count++;
            }
        }
    }

    public int GetQuestionTotal()
    {
        return questionsTotal;
    }

    public int GetCount()
    {
        return count;
    }

    public int GetCorrect()
    {
        return correct;
    }

    public void GenerateQuest()
    {
        correctAnsBtn = ansBtn[RandomButtonIndex()];
        Shuffle(questionsList);
        questionTitle.text = questionsList[0].question;
        for (int i = 0; i < ansBtn.Length; i++)
        {
            Button btn = ansBtn[i];
            Text text = btn.GetComponentInChildren<Text>();
            if (GameObject.ReferenceEquals(btn, correctAnsBtn))
            {
                skippedIndex = i;
                continue;
            }
            if (i != ansBtn.Length - 1)
            {
                text.text = questionsList[0].wrongAns[i];
            }
            else
            {
                text.text = questionsList[0].wrongAns[skippedIndex];
            }

        }
        correctAnsBtn.GetComponentInChildren<Text>().text = questionsList[0].correctAns;
        count++;
        btn = correctAnsBtn;
        Debug.Log(correctAnsBtn.name);
    }

    public void GetNextQuestion()
    {
        questionsList.RemoveAt(0);
        GenerateQuest();

    }

    int RandomButtonIndex()
    {
        int index = UnityEngine.Random.Range(0, 4);
        return index;
    }

    void Shuffle<T>(IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (count == 13)
        {
            finishedView.SetActive(true);
            count++;
            GameObject.Find("BarBackground").GetComponentInChildren<BarScript>().StopTimer();
        }
    }
}
