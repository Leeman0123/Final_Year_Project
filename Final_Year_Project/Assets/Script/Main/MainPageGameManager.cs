using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using System;
using System.IO;

public class MainPageGameManager : MonoBehaviour
{
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;
    bool databaseInitialize = false;
    bool databaseInitializeFinished = false;
    [Header("Top side UI")]
    public Text coinText;
    public Text nameText;
    [Header("Pop up panel")]
    public GameObject loginRequiredPanel;
    public GameObject confirmLogoutPanel;
    [Header("Button")]
    public Button logoutBtn;
    public Button confirmLogoutBtn;
    public Button popUpLogoutRequiredBtn;
    [Header("ScrollView English")]
    public Button backBtnEng;
    public Button engP1;
    public Button engP2;
    public Button engP3;
    public GameObject engSelectLevelScrollView;
    public GameObject P1ScrollView;
    public GameObject P2ScrollView;
    public GameObject P3ScrollView;
    [Header("P1 English Btn")]
    public Button p1VocabAnimals;
    [Header("Store")]
    [SerializeField] Button storeBtn;
    [SerializeField] GameObject storePanel;
    // Start is called before the first frame update
    void Start()
    {
        InitializeFirebase();
        backBtnEng.onClick.AddListener(() => ShowEngSelect());
        engP1.onClick.AddListener(() => ShowP1Eng());
        storeBtn.onClick.AddListener(() => ShowStorePanel());
        p1VocabAnimals.onClick.AddListener(async() =>
        {
            bool downCoinsDetailsSuccess = await CloudStorageHelper.DownloadAnimalsP1QuizConisDetails();
            bool downQuizDetailsSuccess = await CloudStorageHelper.DownloadAnimalsP1Quiz();
            if (downCoinsDetailsSuccess && downQuizDetailsSuccess) {
                RedirectToEngMCAnimalsL1();
            }
        });
    }

    void RedirectToEngMCAnimalsL1()
    {
        string coinsJson = File.ReadAllText(Application.persistentDataPath + "/" + "VocabularyAnimalsCoins.json");
        Coins coin = JsonUtility.FromJson<Coins>(coinsJson);
        GameObject createNewGameObject = new GameObject("CoinsLevel");
        CoinsLevel c = createNewGameObject.AddComponent<CoinsLevel>();
        c.InitializeValue(coin.coins, coin.attempt, coin.refreshRankCoins, coin.description);
        DontDestroyOnLoad(createNewGameObject);
        GeneralScript.RedirectPageWithT("Animals1", "Redirecting to the Vocab - Animals(P1)", "Canvas");
    }

    void ShowEngSelect()
    {
        backBtnEng.gameObject.SetActive(false);
        engSelectLevelScrollView.gameObject.SetActive(true);
        P1ScrollView.SetActive(false);
        //P2ScrollView.SetActive(false);
        //P3ScrollView.SetActive(false);
    }

    void ShowP1Eng()
    {
        backBtnEng.gameObject.SetActive(true);
        engSelectLevelScrollView.gameObject.SetActive(false);
        P1ScrollView.SetActive(true);
    }

    void ShowStorePanel()
    {
        //panelBackground.SetActive(true);
        storePanel.SetActive(true);
    }


    void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
        logoutBtn.onClick.AddListener(() =>
        {
            confirmLogoutPanel.SetActive(true);
        });
        confirmLogoutBtn.onClick.AddListener(() =>
        {
            auth.SignOut();
            confirmLogoutBtn.transform.parent.gameObject.SetActive(false);
            GeneralScript.RedirectPageWithT("StartMenu", "Logouting...", "Canvas");
        });
        popUpLogoutRequiredBtn.onClick.AddListener(() =>
        {
            popUpLogoutRequiredBtn.transform.parent.gameObject.SetActive(false);
            GeneralScript.RedirectPageWithT("StartMenu", "Redirecting to the login page...", "Canvas");
        });
    }

    private void AuthStateChanged(object sender, EventArgs e)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
                loginRequiredPanel.gameObject.SetActive(true);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                if (!databaseInitialize)
                {
                    databaseInitialize = true;
                    if (!databaseInitializeFinished)
                    {
                        FirebaseDatabase.DefaultInstance
                            .GetReference("students")
                            .Child(user.UserId)
                            .ValueChanged += HandledStudentDataChanged;
                        Debug.Log("Setting up Firebase realtime database");
                        databaseInitializeFinished = true;
                    }
                    
                }
            }
        }
        if (auth.CurrentUser == null)
        {
            loginRequiredPanel.SetActive(true);
            return;
        }
    }

    private void HandledStudentDataChanged(object sender, ValueChangedEventArgs e)
    {
        if (e.DatabaseError != null)
        {
            GeneralScript.ShowErrorMessagePanel("Canvas", "Cannot Initialize Database data\n Please Login again!");
            auth.SignOut();
            GeneralScript.RedirectPageWithT("StartMenu", "Redirecting to the Login page...", "Canvas");
            return;
        }
        Debug.Log("Studnet's Data updated");
        DataSnapshot data = e.Snapshot;
        int coins = Convert.ToInt32(data.Child("coins").Value);
        string name = data.Child("name").Value.ToString();
        Debug.Log(string.Format("retrieve data name: {0}, coins: {1}", name, coins));
        coinText.text = coins.ToString();
        nameText.text = name;
    }
}
