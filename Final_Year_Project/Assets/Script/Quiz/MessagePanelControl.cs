using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessagePanelControl : MonoBehaviour
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
        message.text = string.Format("This Test has total {0} questions.\n\n" +
            "Each question has {1} seconds to answer.", questionsCount, questionsSeconds);
        startBtn.onClick.AddListener(() =>
        {
            GameObject.Find("BarBackground").GetComponentInChildren<BarScript>().StartTimer();
            gameObject.SetActive(false);
        });
        closeBtn.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            loadingPanel.SetActive(true);
        });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
