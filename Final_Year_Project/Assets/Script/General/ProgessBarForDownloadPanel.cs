using Firebase.Storage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgessBarForDownloadPanel : MonoBehaviour
{
    [Header("Loading Panel")]
    [SerializeField] Slider slider;
    private bool operationInProgress = false;
    private string savedMessage = "";
    int i = 0;

    void Awake()
    {
        operationInProgress = true;
    }

    void Start()
    {
        StartCoroutine("DoCheck");
    }

    public void SetSaveMessage(string m)
    {
        savedMessage = m;
    }

    public void SetMessageText(string myMesage)
    {
        if (operationInProgress)
            gameObject.transform.Find("Message").GetComponent<Text>().text = myMesage;
    }

    IEnumerator DoCheck()
    {
        for (; ; )
        {
            if (i % 2 == 0) {
                SetMessageText("Downloading " + savedMessage + " .");
            }
            else
            {
                SetMessageText("Downloading " + savedMessage + " ..");
            }
            i += 1;
            yield return new WaitForSeconds(1.05f);
        }
    }
}
