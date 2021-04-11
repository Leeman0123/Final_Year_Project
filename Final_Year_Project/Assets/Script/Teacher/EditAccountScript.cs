using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using System;
using System.Threading.Tasks;
using TMPro;

public class EditAccountScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject tableContent;
    public Button searchBtn;
    public Text fullname;
    public Text email;
    public GameObject detailsPanel;
    public GameObject createAccountPanel;
    private int userType = 0;
    void Start()
    {
        searchBtn.onClick.AddListener(() =>
        {
            /*
            var loadedObject = Resources.Load("Teacher/EditAccount/DataRow");
            GameObject dataRow = GameObject.Instantiate(loadedObject) as GameObject;
            dataRow.transform.SetParent(tableContent.transform, false);
            */
            createAccountPanel.SetActive(true);
        });
        AddStudentDatabaseListener();
    }

    public void HandledSelectedIndex(int val)
    {
        if (val == 0)
        {
            userType = 0;
            AddStudentDatabaseListener();
        }
        else if (val == 1)
        {
            userType = 1;
            AddTeacherDatabaseListener();
        }
    }

    void AddStudentDatabaseListener()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("teachers")
            .ValueChanged -= HandledDatabaseValueChanged;
        FirebaseDatabase.DefaultInstance
            .GetReference("students")
            .ValueChanged += HandledDatabaseValueChanged;
    }

    void AddTeacherDatabaseListener()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("students")
            .ValueChanged -= HandledDatabaseValueChanged;
        FirebaseDatabase.DefaultInstance
            .GetReference("teachers")
            .ValueChanged += HandledDatabaseValueChanged;
    }


    private void HandledDatabaseValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (e.DatabaseError != null)
        {
            Debug.LogError(e.DatabaseError);
            return;
        }
        else
        {
            ClearRowsInTable();
            DataSnapshot studentsDataList = e.Snapshot;
            int count = 1;
            foreach (DataSnapshot data in studentsDataList.Children)
            {
                var loadedObject = Resources.Load("Teacher/EditAccount/DataRow");
                GameObject dataRow = GameObject.Instantiate(loadedObject) as GameObject;
                Text no = dataRow.transform.Find("No").gameObject.GetComponent<Text>();
                Text fullName = dataRow.transform.Find("Fullname").gameObject.GetComponent<Text>();
                Text email = dataRow.transform.Find("Email").gameObject.GetComponent<Text>();
                no.text = count.ToString();
                fullName.text = data.Child("name").Value.ToString();
                email.text = data.Child("email").Value.ToString();
                string uid = data.Child("uid").Value.ToString();
                Button detailsBtn = dataRow.transform.Find("DetailsBtn").gameObject.GetComponent<Button>();
                if (userType == 0)
                {
                    detailsBtn.onClick.AddListener(() =>
                    {
                        detailsPanel.gameObject.SetActive(true);
                        GetStudentDetails(uid);

                    });
                }
                else if (userType == 1)
                {
                    detailsBtn.onClick.AddListener(() =>
                    {
                        detailsPanel.gameObject.SetActive(true);
                        GetTeacherDetails(uid);

                    });
                }
                dataRow.transform.SetParent(tableContent.transform, false);
                Debug.Log(string.Format("Append user to table name: {0}, email: {1}", fullName.text, email.text));
                count++;
            }
        }
    }

    private void ClearRowsInTable()
    {
        int tableRowCount = tableContent.transform.childCount;
        for (int i = 0; i < tableRowCount; i++)
        {
            GameObject.Destroy(tableContent
                .transform
                .GetChild(i)
                .gameObject);
            Debug.Log(string.Format("Deleted table row {0}", i));
        }
    }

    public async void GetStudentDetails(string uid)
    {
        Students student = await DbHelper.GetStudentById(uid);
        Debug.Log(string.Format("Retrieve data: Full Name: {0}, Email: {1}", student.name, student.email));
        fullname.text = string.Format("Full Name: {0}", student.name);
        email.text = string.Format("Email: {0}", student.email);
    }

    public async void GetTeacherDetails(string uid)
    {
        Teachers teacher = await DbHelper.GetTeacherById(uid);
        fullname.text = string.Format("Full Name: {0}",teacher.name);
        email.text = string.Format("Full Name: {0}", teacher.email);
        Debug.Log(string.Format("Retrieve data: Full Name: {0}, Email: {1}", teacher.name, teacher.email));
    }

    public async void TestMethod()
    {
        HashSet<string> emailList = await DbHelper.GetAllUserEmail();
        foreach(string email in emailList)
        {
            Debug.Log("email: " + email);
        }
    }
}
