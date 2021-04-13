using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerTeacher : MonoBehaviour
{
    [SerializeField] Button accountManagementBtn;
    [SerializeField] Button studentReportBtn;
    [SerializeField] Button cancelBtn;
    [Header("Account Management Group")]
    [SerializeField] GameObject btnAccountManagementBtnGroup;
    [SerializeField] Button createAccountBtn;
    // Start is called before the first frame update
    void Start()
    {
        accountManagementBtn.onClick.AddListener(() => {
            btnAccountManagementBtnGroup.SetActive(true);
            cancelBtn.gameObject.SetActive(true);
            studentReportBtn.gameObject.SetActive(false);
            accountManagementBtn.gameObject.SetActive(false);
        });

        cancelBtn.onClick.AddListener(() => {
            btnAccountManagementBtnGroup.SetActive(false);
            studentReportBtn.gameObject.SetActive(true);
            accountManagementBtn.gameObject.SetActive(true);
            cancelBtn.gameObject.SetActive(false);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
