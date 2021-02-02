using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishedHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject good;
    [SerializeField] GameObject awesome;
    [SerializeField] GameObject amazing;
    [SerializeField] Text scoreText;
    [SerializeField] Button btn;
    private int correct = 0;
    private int total = 0;
    void Awake()
    {
        GenerateQuestion gq = GenerateQuestion.instance;
        correct = gq.GetCorrect();
        total = gq.GetQuestionTotal();
        if (correct >=0 && correct < 5)
        {
            good.SetActive(true);
            awesome.SetActive(false);
            amazing.SetActive(false);
        }
        else if (correct >5 && correct < 10)
        {
            good.SetActive(false);
            awesome.SetActive(true);
            amazing.SetActive(false);
        }
        else if (correct >10)
        {
            good.SetActive(false);
            awesome.SetActive(false);
            amazing.SetActive(true);

        }
        scoreText.text = string.Format("Score: {0}/12", correct);
    }

    void Start()
    {
        btn.onClick.AddListener(() =>
        {
            string userId = GameObject.Find("CheckAuth").GetComponent<CheckAuthentication>().GetUserId();
            GameObject.Find("DatabaseQuizWriter").GetComponent<DatabaseQuizWriter>()
            .WriteVocabAnimals(
                userId, 
                correct,
                GameObject.Find("BarBackground").GetComponentInChildren<BarScript>().GetRemainTime(),
                total);
            GameObject.Find("DatabaseQuizWriter").GetComponent<DatabaseQuizWriter>()
            .WriteUserCoins(userId, 5);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
