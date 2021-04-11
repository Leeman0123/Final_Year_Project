using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

}
