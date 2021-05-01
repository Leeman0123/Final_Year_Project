using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Storage;
using System.Threading;
using System.IO;

public class GeneralScript : MonoBehaviour
{
    // Start is called before the first frame update
    public string scene, message, parent;
    public void RedirectPageWithT() {
        var loadedObject = Resources.Load("General/ProgressBarPanelWithText");
        GameObject inObj = GameObject.Instantiate(loadedObject) as GameObject;
        inObj.transform.SetParent(GameObject.Find(parent).transform,false);
        inObj.GetComponent<ProgressBar>().scene = scene;
        inObj.GetComponent<ProgressBar>().message = message;
        inObj.SetActive(true);
    }

    public static void RedirectPageWithT(string scene, string message, string parent)
    {
        var loadedObject = Resources.Load("General/ProgressBarPanelWithText");
        GameObject inObj = GameObject.Instantiate(loadedObject) as GameObject;
        inObj.transform.SetParent(GameObject.Find(parent).transform, false);
        inObj.GetComponent<ProgressBar>().scene = scene;
        inObj.GetComponent<ProgressBar>().message = message;
        inObj.SetActive(true);
    }

    public static void ShowErrorMessagePanel(string myParent, string myMessage)
    {
        var loadedObject = Resources.Load("General/MessageErrorPanel");
        GameObject panel = GameObject.Instantiate(loadedObject) as GameObject;
        Text message = panel.transform.Find("MessageText").gameObject.GetComponent<Text>();
        message.text = myMessage;
        Button okBtn = panel.transform.Find("OkButton").gameObject.GetComponent<Button>();
        okBtn.onClick.AddListener(() =>
        {
            GameObject.Destroy(panel);
        });
        panel.transform.SetParent(GameObject.Find(myParent).transform, false);
    }

    public static void ShowMessagePanelWithTick(string myParent, string myMessage)
    {
        var loadedObject = Resources.Load("General/MessagePanelWithTick");
        GameObject panel = GameObject.Instantiate(loadedObject) as GameObject;
        Text message = panel.transform.Find("MessageText").gameObject.GetComponent<Text>();
        message.text = myMessage;
        Button okBtn = panel.transform.Find("OkButton").gameObject.GetComponent<Button>();
        okBtn.onClick.AddListener(() =>
        {
            GameObject.Destroy(panel);
        });
        panel.transform.SetParent(GameObject.Find(myParent).transform, false);
    }

    public static void ShowMessagePanel(string myParent, string myMessage)
    {

        var loadedObject = Resources.Load("General/MessagePanelWithText");
        GameObject panel = GameObject.Instantiate(loadedObject) as GameObject;
        Text message = panel.transform.Find("MessageText").gameObject.GetComponent<Text>();
        message.text = myMessage;
        Button okBtn = panel.transform.Find("OkButton").gameObject.GetComponent<Button>();
        okBtn.onClick.AddListener(() =>
        {
            GameObject.Destroy(panel);
        });
        panel.transform.SetParent(GameObject.Find(myParent).transform, false);
    }

    public static void ShowDownloadPanel(string myParent, string myMessage) {
        var loadedObject = Resources.Load("General/ProgressBarDownPanelWithText");
        GameObject panel = GameObject.Instantiate(loadedObject) as GameObject;
        panel.gameObject.name = "ProgressBarDownPanelWithText";
        Text message = panel.transform.Find("Message").gameObject.GetComponent<Text>();
        message.text = myMessage;
        panel.transform.SetParent(GameObject.Find(myParent).transform, false);
        panel.SetActive(true);
    }

    public static GameObject ShowMessagePanelWithTextLoading(string myParent, string myMessage)
    {
        var loadedObject = Resources.Load("General/MessagePanelWithTextLoading");
        GameObject panel = GameObject.Instantiate(loadedObject) as GameObject;
        Text message = panel.transform.Find("MessageText").gameObject.GetComponent<Text>();
        message.text = myMessage;
        panel.transform.SetParent(GameObject.Find(myParent).transform, false);
        return panel;
    }

    public static void EditMessagePanelWithTextLoading(GameObject panel, string text)
    {
        Text message = panel.transform.Find("MessageText").gameObject.GetComponent<Text>();
        message.text = text;
    }

    public static void DisplayDownloadStateForDownloadPanel(string fileName)
    {
        string myMessage = string.Format("Downloading {0}", fileName);
        ProgessBarForDownloadPanel panelScript = GameObject.Find("ProgressBarDownPanelWithText").GetComponent<ProgessBarForDownloadPanel>();
        panelScript.SetMessageText(myMessage);
        panelScript.SetSaveMessage(fileName);
    }

    public static void DisplayUploadQuestionStateForDownloadPanel()
    {
        string myMessage = string.Format("Uploading {0}", "quiz question");
        ProgessBarForDownloadPanel panelScript = GameObject.Find("ProgressBarDownPanelWithText").GetComponent<ProgessBarForDownloadPanel>();
        panelScript.SetMessageText(myMessage);
        panelScript.SetSaveMessage("quiz question");
    }

    public static void DisplayUploadQuestionDetailsStateForDownloadPanel()
    {
        string myMessage = string.Format("Uploading {0}", "quiz details");
        ProgessBarForDownloadPanel panelScript = GameObject.Find("ProgressBarDownPanelWithText").GetComponent<ProgessBarForDownloadPanel>();
        panelScript.SetMessageText(myMessage);
        panelScript.SetSaveMessage("quiz question");
    }



    public static IEnumerator FreeUpStorage()
    {
        string[] filePaths = Directory.GetFiles(Application.persistentDataPath);
        int filesCount = filePaths.Length;
        int count = 1;
        GameObject myPanel = GeneralScript.ShowMessagePanelWithTextLoading("Canvas", "Searching Files...");
        foreach (string filePath in filePaths) {
            GeneralScript.EditMessagePanelWithTextLoading(myPanel, $"Deleting {Path.GetFileName(filePath)} ({count}/{filesCount})");
            File.Delete(filePath);
            yield return new WaitForSeconds(0.25f);
            count++;
        }
        if (filePaths.Length == 0)
        {
            GameObject.Destroy(myPanel);
            ShowMessagePanel("Canvas", "Operation Finished");
        } else
        {
            GameObject.Destroy(myPanel);
        }
        
    }


    public static void DestroyDownloadPanel()
    {
        Destroy(GameObject.Find("ProgressBarDownPanelWithText"));
    }

}
