using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuUIManager : MonoBehaviour
{
    [SerializeField] Button startMenuLoginBtn;
    [SerializeField] Button startMenuQuitBtn;
    [SerializeField] Button teacherBtn;
    [SerializeField] Button studentBtn;
    [SerializeField] Button teacherLoginBtn;
    [SerializeField] Button studentLoginBtn;
    [SerializeField] Button backLoginSelectionBtn;
    [SerializeField] Text teacherTitle;
    [SerializeField] Text studentTitle;
    [SerializeField] GameObject backStartMenuBtn;
    [SerializeField] GameObject loginSelection;
    [SerializeField] GameObject loginView;
    [SerializeField] GameObject startMenu;
    [SerializeField] InputField emailField;
    [SerializeField] InputField passwordField;
    [SerializeField] GameObject dialog;
    [SerializeField] Text messageText;
    [SerializeField] Text dialogTitle;
    [SerializeField] Button okBtn;
    public static StartMenuUIManager instance;
    // Start is called before the first frame update

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        startMenuLoginBtn.onClick.AddListener(() => ShowLoginSelection());
        startMenuQuitBtn.onClick.AddListener(() => QuitGame());
        backStartMenuBtn.GetComponentInChildren<Button>().onClick.AddListener(() => ShowStartMenu());
        teacherBtn.onClick.AddListener(() => ShowTeacherLoginView());
        studentBtn.onClick.AddListener(() => ShowStudentLoginView());
        backLoginSelectionBtn.onClick.AddListener(() => BackLoginSelectionView());
        okBtn.onClick.AddListener(() => CloseDialog());


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BackLoginSelectionView()
    {
        loginSelection.gameObject.SetActive(true);
        loginView.gameObject.SetActive(false);

    }

    void ShowTeacherLoginView()
    {
        loginView.SetActive(true);
        loginSelection.SetActive(false);
        teacherTitle.gameObject.SetActive(true);
        studentTitle.gameObject.SetActive(false);
        teacherLoginBtn.gameObject.SetActive(true);
        studentLoginBtn.gameObject.SetActive(false);
    }

    void ShowStudentLoginView()
    {
        loginView.SetActive(true);
        loginSelection.SetActive(false);
        teacherTitle.gameObject.SetActive(false);
        studentTitle.gameObject.SetActive(true);
        teacherLoginBtn.gameObject.SetActive(false);
        studentLoginBtn.gameObject.SetActive(true);
    }

    void ShowStartMenu()
    {
        loginSelection.gameObject.SetActive(false);
        startMenu.gameObject.SetActive(true);
    }

    void ShowLoginSelection()
    {
        loginSelection.gameObject.SetActive(true);
        startMenu.gameObject.SetActive(false);
    }

    //Quit Application
    void QuitGame()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    void clearLoginField()
    {
        emailField.text = "";
        passwordField.text = "";
    }

    void CloseDialog()
    {
        dialog.SetActive(false);
    }

    public void BackLoginSelection()
    {
        clearLoginField();
        loginSelection.SetActive(true);
        loginView.SetActive(false);
    }

    public void ShowLoginErrorMessage(string message, string messageTitle)
    {
        dialog.SetActive(true);
        messageText.text = message;
        dialogTitle.text = messageTitle;
    }

    public void ShowFirebaseErrorMessage(string message)
    {
        dialog.SetActive(true);
        messageText.text = "Cannot initialize the Firebase SDK.";
    }

}
