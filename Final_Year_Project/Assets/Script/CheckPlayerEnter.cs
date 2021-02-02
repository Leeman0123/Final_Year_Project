using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using MoreMountains.Tools;
using MoreMountains.CorgiEngine;

public class CheckPlayerEnter : MonoBehaviour
{
    [SerializeField] GameObject questionView;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            ChineseGenerateQuestion a = GameObject.Find("QuestionManager").GetComponent<ChineseGenerateQuestion>();
            a.createQuestion();
            a.ChangeActive();
            a.setObject(transform.gameObject);
            questionView.SetActive(true);
            GameObject.Find("Rectangle").SetActive(false);
        }
    }
}
