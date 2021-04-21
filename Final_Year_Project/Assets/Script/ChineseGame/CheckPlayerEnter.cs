using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using MoreMountains.Tools;
using MoreMountains.CorgiEngine;
using UnityEngine.SceneManagement;

public class CheckPlayerEnter : MonoBehaviour
{
    [SerializeField] GameObject questionView;
    [SerializeField] GameObject playerdie;
    [SerializeField] GameObject QuestionManager;
    [SerializeField] GameObject Player;
    private int savehp;
    // Start is called before the first frame update

    private void Awake()
    {

    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            CheckPlayerDie aaa = playerdie.GetComponent<CheckPlayerDie>();
            aaa.playing = false;
            ChineseGenerateQuestion a = QuestionManager.GetComponent<ChineseGenerateQuestion>();
            Health hp = Player.GetComponent<Health>();
            savehp = hp.CurrentHealth;
            Debug.Log("Player Enter Health:" + hp.CurrentHealth);
            a.setHp(savehp);
            a.createQuestion();
            a.ChangeActive();
            a.setObject(transform.gameObject);
            questionView.SetActive(true);
            Player.SetActive(false);


        }
    }
}
