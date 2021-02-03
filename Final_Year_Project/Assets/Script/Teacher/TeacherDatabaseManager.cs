using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;

public class TeacherDatabaseManager : MonoBehaviour
{
    [SerializeField] Text teacherWelcomeText;
    [SerializeField] GameObject panelBackground;
    [SerializeField] GameObject loginRequiredPanel;
    [SerializeField] GameObject progressPanel;
    [SerializeField] Button backToLoginBtn;
    [SerializeField] Button registerBtn;
    [SerializeField] GameObject registrationPanel;
    private CheckAuthentication script;
    private string userId;
    // Start is called before the first frame update
    void Start()
    {
        backToLoginBtn.onClick.AddListener(() =>
        {
            progressPanel.SetActive(true);
        });
        registerBtn.onClick.AddListener(() => ShowRegistration());
        script = GameObject.Find("CheckAuth").GetComponent<CheckAuthentication>();
        userId = GameObject.Find("CheckAuth").GetComponent<CheckAuthentication>().GetUserId();
        if (!script.IsLogined())
        {
            ShowPanel();
        }
        else
        {
            StartCoroutine(GetTeacherName());
            
        }

    }

    // Update is called once per frame
    void Update()
    {


    }

    void ShowRegistration()
    {
        registrationPanel.SetActive(true);
    }

    void ShowPanel()
    {
        panelBackground.SetActive(true);
        loginRequiredPanel.SetActive(true);
    }

    public void Logout()
    {
        script.Logout();
        ShowPanel();
    }

    private IEnumerator GetTeacherName()
    {
        var getTask = FirebaseDatabase.DefaultInstance
        .GetReference("teachers")
        .Child(userId)
        .GetValueAsync();
        yield return new WaitUntil(() => getTask.IsCompleted || getTask.IsFaulted);
        if (getTask.IsCompleted)
        {
            Dictionary<string, object> results = (Dictionary<string, object>)getTask.Result.Value;
            string teacherName = (string)results["name"];
            Debug.Log(teacherName);
            teacherWelcomeText.text = "Welcome, " + teacherName;
        }
    }
}
