using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

public class FirebaseManager : MonoBehaviour
{
    [SerializeField] InputField loginText;
    [SerializeField] InputField passwordText;
    public static FirebaseManager instance;
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser User;
    public DatabaseReference DBreference;

    [Header("Register")]
    [SerializeField] InputField studentNameText;
    [SerializeField] InputField registerEmailText;
    [SerializeField] InputField registerPasswordText;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        //Check Firebase assest is ready;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitalizeFirebase();
            }
            else
            {
                Debug.LogError("Cannot Resolve Firebase Dependency: " + task.Result);
            }
        });
    }

    void InitalizeFirebase()
    {
        Debug.Log("Setting up Firebase Auth && Database");
        //initialize auth function
        auth = FirebaseAuth.DefaultInstance;
        //initialize DB
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void BackStartMenuAction()
    {
        StartCoroutine(BackStartMenu());
    }

    public void LoginAction()
    {
        StartCoroutine(Login(loginText.text, passwordText.text));
    }

    public void RegisterAction()
    {
        StartCoroutine(Register(registerEmailText.text, registerPasswordText.text, studentNameText.text));
    }

    IEnumerator BackStartMenu()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("StartMenu");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    IEnumerator LoadStudentMainScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LevelSelect");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    IEnumerator LoadTeacherMainScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("RegisterPage");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    IEnumerator Login(string email, string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(predicate: () => loginTask.IsCompleted);
        if (loginTask.Exception == null)
        {
            User = loginTask.Result;
            Debug.Log("User signed in successfully: " + User.DisplayName + ", " + User.Email);
            StartCoroutine(RedirectUserScene());
        }
        else
        {
            Debug.LogWarning(message: $"Failed to login task with {loginTask.Exception}");
            FirebaseException firebaseEx = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
            string message = "Login Failed";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            Debug.LogError(message);
        }
    }


    IEnumerator Register(string email, string password, string studentName)
    {
        if (studentName == "")
        {
            Debug.LogError("Missing Student Name");
        }
        else
        {
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);
            if (RegisterTask.Exception == null)
            {
                FirebaseUser newUser = RegisterTask.Result;
                if (newUser != null)
                {
                    UserProfile profile = new UserProfile { DisplayName = studentName };
                    var ProfileTask = newUser.UpdateUserProfileAsync(profile);
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception == null)
                    {
                        StartCoroutine(UpdateDatabaseStudentName(newUser, studentNameText.text));
                        StartCoroutine(UpdateDatabaseUserEmail(newUser, email, 0));
                        Debug.Log("Register account with Email:" + email + " and Student Name:" + studentName + " Successfully!!");
                    }
                    else
                    {
                        Debug.LogWarning(message: $"Failed to register task with: {ProfileTask.Exception}");
                    }
                }
            }
            else
            {
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");

                FirebaseException registerEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError code = (AuthError)registerEx.ErrorCode;

                string message = "Register Failed";
                switch (code)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email already in use";
                        break;
                    case AuthError.WeakPassword:
                        message = "Password is too weak";
                        break;
                }
                Debug.LogError(message);
            }
        }
    }
    IEnumerator UpdateDatabaseTeacherName(FirebaseUser user, string name)
    {
        //Teachers Row => UserId => name => 
        var DBTask = DBreference.Child("teachers").Child(user.UserId).Child("name").SetValueAsync(name);
        //Wait Until Modify DB task is completed;
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to update teacher name task with {DBTask.Exception}");
        }
        else
        {
            Debug.Log("Updated Teacher name as " + name + " with userID: " + user.UserId);
        }
    }

    IEnumerator UpdateDatabaseUserEmail(FirebaseUser user, string email, int role)
    {
        string userRole;
        if (role.Equals(0))
            userRole = "students";
        else
            userRole = "teachers";
        //Users Row => UserId => name => 
        var DBTask = DBreference.Child(userRole).Child(user.UserId).Child("email").SetValueAsync(email);
        //Wait Until Modify DB task is completed;
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to update user email task with {DBTask.Exception}");
        }
        else
        {
            Debug.Log("Updated User email as " + email + " with userID: " + user.UserId);
        }
    }

    IEnumerator UpdateDatabaseStudentName(FirebaseUser user, string name)
    {
        //Users Row => UserId => name => 
        var DBTask = DBreference.Child("students").Child(user.UserId).Child("name").SetValueAsync(name);
        //Wait Until Modify DB task is completed;
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to update student name task with {DBTask.Exception}");
        }
        else
        {
            Debug.Log("Updated Student name as " + name + " with userID: " + user.UserId);
        }
    }

    IEnumerator RedirectUserScene()
    {
        bool isStudent = false;
        var DBTask = DBreference.Child("students").Child(User.UserId).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to read database data {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            if (snapshot != null  && snapshot.ChildrenCount > 0)
            {
                isStudent = true;
            }
        }
        if (isStudent)
            StartCoroutine(LoadStudentMainScene());
        else
            StartCoroutine(LoadTeacherMainScene());
    }

}
