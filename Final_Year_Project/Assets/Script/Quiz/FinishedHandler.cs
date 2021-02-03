using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
            if (SceneManager.GetActiveScene().name.Equals("Animals1"))
            {
                StartCoroutine(GameObject.Find("DatabaseQuizWriter").GetComponent<DatabaseQuizWriter>()
                                        .WriteVocabAnimals(
                                            userId,
                                            correct,
                                            GameObject.Find("BarBackground").GetComponentInChildren<BarScript>().GetRemainTime(),
                                            total));
            }
            else if (SceneManager.GetActiveScene().name.Equals("Vehicle1"))
            {
                StartCoroutine(GameObject.Find("DatabaseQuizWriter").GetComponent<DatabaseQuizWriter>()
                                        .WriteVocabVehicles(
                                            userId,
                                            correct,
                                            GameObject.Find("BarBackground").GetComponentInChildren<BarScript>().GetRemainTime(),
                                            total));
            }

        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
