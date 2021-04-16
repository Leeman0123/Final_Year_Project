using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;


public class MainPageUIManager : MonoBehaviour
{
    [Header("AuthUserChecker")]
    [SerializeField] GameObject CheckAuth;
    [Header("LoginErrorPanel")]
    [SerializeField] Button loginBtn;
    [SerializeField] GameObject panelBackground;
    [SerializeField] GameObject loginErrorPanel;
    [Header("LogoutPanel")]
    [SerializeField] GameObject logoutPanel;
    [SerializeField] Button logoutBtn;
    [SerializeField] Button logoutConfirmBtn;
    [SerializeField] Button cancelLogoutBtn;
    [Header("LogoutProgress")]
    [SerializeField] GameObject loginCanvasPanel;
    [SerializeField] GameObject loginRequiredPanel;
    [Header("Store")]
    [SerializeField] Button storeBtn;
    [SerializeField] GameObject storePanel;

    private string userId;
    private CheckAuthentication script;

    void Awake()
    {
        script = CheckAuth.GetComponent<CheckAuthentication>();
        userId = CheckAuth.GetComponent<CheckAuthentication>().GetUserId();
        if (!script.IsLogined()) 
        { 
            ShowLoginPanel();
        }
    }

    void Start()
    {
        logoutBtn.onClick.AddListener(() => ShowLogoutPanel());
        cancelLogoutBtn.onClick.AddListener(() => CloseLogoutPanel());
        logoutConfirmBtn.onClick.AddListener(() => Logout());
        loginBtn.onClick.AddListener(() => {
            loginRequiredPanel.SetActive(true);
        });
        storeBtn.onClick.AddListener(() => ShowStorePanel());
    }

    void CloseLogoutPanel()
    {
        panelBackground.SetActive(false);
        logoutPanel.SetActive(false);
    }

    void Logout()
    {
        if (script.IsLogined())
        {
            script.Logout();
            loginCanvasPanel.SetActive(true);
        }


    }

    void ShowLogoutPanel()
    {
        panelBackground.SetActive(true);
        logoutPanel.SetActive(true);
    }

    void ShowLoginPanel()
    {
        panelBackground.SetActive(true);
        loginErrorPanel.SetActive(true);
    }

    void ShowStorePanel() {
        panelBackground.SetActive(true);
        storePanel.SetActive(true);
    }
}
