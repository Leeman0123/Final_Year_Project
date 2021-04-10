using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MathMessagePanelControl : MonoBehaviour
{
    [SerializeField] int questionsCount;
    [SerializeField] int questionsSeconds;
    [SerializeField] Text message;
    [SerializeField] Button startBtn;
    [SerializeField] Button closeBtn;
    [SerializeField] GameObject loadingPanel;

    // Start is called before the first frame update
    void Start()
    {
        message.text = string.Format("This quiz has total {0} questions.\n\n" +
            "Each question has {1} seconds to answer. \n\n" +
            "Get a coin for every correct answer", questionsCount, questionsSeconds);
        startBtn.onClick.AddListener(() =>
        {
            GameObject.Find("BarBackground").GetComponentInChildren<MathBarScript>().StartTimer();
            gameObject.SetActive(false);
        });
        closeBtn.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            //loadingPanel.SetActive(true);
            SceneManager.LoadScene("Bird_Low_LevelSelect");
        });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
