using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

public class LoginController : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseUser user;
    [Header("Login Field")]
    [SerializeField] InputField email;
    [SerializeField] InputField password;

    [Header("Teacher Login")] 
    [SerializeField] Button teacherLoginBtn;
    [Header("Student Login")]
    [SerializeField] Button studentLoginBtn;
    [Header("Loading Panel")]
    [SerializeField] GameObject loadingScreen;
    [SerializeField] Slider slider;
    void Awake()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                InitializeFirebase();
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else //
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }


    // Start is called before the first frame update
    void Start()
    {
        studentLoginBtn.onClick.AddListener(() => Login());
        teacherLoginBtn.onClick.AddListener(() => Login());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void InitializeFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += Auth_StateChanged;
    }

    void Auth_StateChanged(object sender, System.EventArgs e)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Logined");
                LoadLevel();
            }
        }
    }

    void Login()
    {
        StartCoroutine(WaitLogin(email.text, password.text));
        if (user != null)
        {
            LoadLevel();
        }

    }


    void LoadLevel()
    {
        LoadUserScreenProgressAction();
    }

    void LoadUserScreenProgressAction()
    {
        StartCoroutine(CheckStudentExist(user.UserId, (result) => {
            StartCoroutine(LoadUserScreenProgress(result));
        }));
    }

    IEnumerator LoadUserScreenProgress(bool isStudent)
    {
        AsyncOperation operation;
        if (isStudent)
        {
            operation = SceneManager.LoadSceneAsync("Main");
        }
        else
        {
            operation = SceneManager.LoadSceneAsync("TeacherMain");
        }
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            yield return null;
        }
    }

    IEnumerator WaitLogin(string email, string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(predicate: () => loginTask.IsCompleted);
        if (loginTask.IsCanceled)
        {
            Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
        }
        if (loginTask.IsFaulted)
        {
            int authErrorCode = (loginTask.Exception.GetBaseException() as FirebaseException).ErrorCode;
            StartMenuUIManager.instance.ShowLoginErrorMessage(GetAuthErrorMessage(authErrorCode)
                , "Login Error Occured");
            Debug.LogError(GetAuthErrorMessage(authErrorCode));
        }
        else
        {
            user = loginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                user.DisplayName, user.UserId);
        }
    }

    //Reference for later
    //error code: https://firebase.google.com/docs/reference/unity/namespace/firebase/auth
    string GetAuthErrorMessage(int authErrorCode)
    {
        string errorMessage = "Unknown Error";
        switch ((AuthError)authErrorCode)
        {
            case AuthError.InvalidEmail:
                errorMessage = "Email is Invalid";
                break;
            case AuthError.WrongPassword:
                errorMessage = "Password is wrong";
                break;
            case AuthError.MissingEmail:
                errorMessage = "Email is missing";
                break;
            case AuthError.MissingPassword:
                errorMessage = "Password is missing";
                break;
            case AuthError.WeakPassword:
                errorMessage = "Password is too weak";
                break;
            case AuthError.UserNotFound:
                errorMessage = "User account is not exist.";
                break;
            case AuthError.UserDisabled:
                errorMessage = "User account is suspended.";
                break;
        }
        return errorMessage;
    }

    IEnumerator CheckStudentExist(string userID, System.Action<bool> result)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("students")
            .Child(userID)
            .GetValueAsync();
        yield return new WaitUntil(predicate: () => task.IsCompleted);
        if (task.IsFaulted)
        {
            Debug.LogError("Cannot connect to the database's table");
        }
        else if (task.IsCompleted)
        {
            DataSnapshot snapshot = task.Result;
            // Do something with snapshot...
            if (snapshot.Value != null)
            {
                result(true);
            }
            else
            {
                result(false);
            }
        }
    }
    IEnumerator CheckTeacherExist(string userID, System.Action<bool> result)
    {
        var task = FirebaseDatabase.DefaultInstance
            .GetReference("teachers")
            .Child(userID)
            .GetValueAsync();
        yield return new WaitUntil(predicate: () => task.IsCompleted);
        if (task.IsFaulted)
        {
            Debug.LogError("Cannot connect to the database's table");
        }
        else if (task.IsCompleted)
        {
            DataSnapshot snapshot = task.Result;
            // Do something with snapshot...
            if (snapshot.Value != null)
            {
                result(true);
            }
            else
            {
                result(false);
            }
        }
    }
}
