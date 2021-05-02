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
    public Button englishExtra;
    public GameObject engSelectLevelScrollView;
    public GameObject P1ScrollView;
    public GameObject P2ScrollView;
    public GameObject P3ScrollView;
    public GameObject englishExtraScrollView;
    public GameObject englishExtraScrollViewContent;
    [Header("ScrollView Maths")]
    public Button backBtnMaths;
    public Button mathsP1;
    public Button mathsP2;
    public Button mathsP3;
    public Button mathsExtra;
    public GameObject mathsSelectLevelScrollView;
    public GameObject P1ScrollViewMaths;
    public GameObject P2ScrollViewMaths;
    public GameObject P3ScrollViewMaths;
    public GameObject mathsExtraScrollView;
    public GameObject mathsExtraScrollViewContent;
    [Header("ScrollView Chinese")]
    public Button backBtnChinese;
    public Button chineseP1;
    public Button chineseP2;
    public Button chineseP3;
    public Button chineseExtra;
    public GameObject chineseSelectLevelScrollView;
    public GameObject P1ScrollViewChinese;
    public GameObject P2ScrollViewChinese;
    public GameObject P3ScrollViewChinese;
    public GameObject chineseExtraScrollView;
    public GameObject chineseExtraScrollViewContent;
    [Header("P1 English Btn")]
    public Button p1VocabAnimals;
    public Button p1VocabVehicles;
    [Header("P2 English Btn")]
    public Button p2VocabVehicles;
    public Button p2VocabAnimals;
    [Header("P3 English Btn")]
    public Button p3CompleteCentences;
    public Button p3Preposition;
    [Header("P1 Maths Btn")]
    public Button p1Add;
    public Button p1Sub;
    [Header("P2 Maths Btn")]
    public Button p2AddSub;
    public Button p2MuDiv;
    [Header("P3 Maths Btn")]
    public Button p3Arith;
    public Button p3Decimal;
    [Header("P1 Chinese Btn")]
    public Button p1FillIn;
    public Button p1Unit;
    [Header("P2 Chinese Btn")]
    public Button p2FillInAdvanced;
    public Button p2Head;
    [Header("P3 Chinese Btn")]
    public Button p3Idiom1;
    public Button p3Idiom2;
    [Header("Setting Button")]
    public Button settingBtn;
    public GameObject settingPanel;
    public Button resetPassword;
    public Button freeUpSpaceBtn;
    public InputField emailInputField;
    public Button submitEmail;
    public GameObject emailResetPanel;

    [Header("Store")]
    [SerializeField] Button storeBtn;
    [SerializeField] GameObject storePanel;
    // Start is called before the first frame update
    void Start()
    {
        InitializeFirebase();
        backBtnEng.onClick.AddListener(() => ShowEngSelect());
        backBtnMaths.onClick.AddListener(() => ShowMathsSelect());
        backBtnChinese.onClick.AddListener(() => ShowChineseSelect());
        engP1.onClick.AddListener(() => ShowP1Eng());
        engP2.onClick.AddListener(() => ShowP2Eng());
        engP3.onClick.AddListener(() => ShowP3Eng());
        mathsP1.onClick.AddListener(() => ShowP1Maths());
        mathsP2.onClick.AddListener(() => ShowP2Maths());
        mathsP3.onClick.AddListener(() => ShowP3Maths());
        chineseP1.onClick.AddListener(() => ShowP1Chinese());
        chineseP2.onClick.AddListener(() => ShowP2Chinese());
        chineseP3.onClick.AddListener(() => ShowP3Chinese());
        storeBtn.onClick.AddListener(() => ShowStorePanel());
        mathsExtra.onClick.AddListener(() => ShowMathsExtra());
        chineseExtra.onClick.AddListener(() => ShowChineseExtra());
        englishExtra.onClick.AddListener(() => ShowEnglishExtra());
        settingBtn.onClick.AddListener(() =>
        {
            settingPanel.SetActive(true);
        });
        freeUpSpaceBtn.onClick.AddListener(() =>
        {
            settingPanel.SetActive(false);
            StartCoroutine(GeneralScript.FreeUpStorage());
        });
        p1VocabAnimals.onClick.AddListener(async() =>
        {
            bool downCoinsDetailsSuccess = await CloudStorageHelper.DownloadAnimalsP1QuizConisDetails();
            bool downQuizDetailsSuccess = await CloudStorageHelper.DownloadAnimalsP1Quiz();
            if (downCoinsDetailsSuccess && downQuizDetailsSuccess) {
                RedirectToEngMCAnimalsL1();
            }
        });
        p1VocabVehicles.onClick.AddListener(async () =>
        {
            bool downCoinsDetailsSuccess = await CloudStorageHelper.DownloadVehicleP1QuizConisDetails();
            bool downQuizDetailsSuccess = await CloudStorageHelper.DownloadVehicleP1Quiz();
            if (downCoinsDetailsSuccess && downQuizDetailsSuccess)
            {
                RedirectToEngMCVehicleL1();
            }
        });
        p2VocabVehicles.onClick.AddListener(async () =>
        {
            bool downCoinsDetailsSuccess = await CloudStorageHelper.DownloadVehicleP2QuizConisDetails();
            bool downQuizDetailsSuccess = await CloudStorageHelper.DownloadVehicleP2Quiz();
            if (downCoinsDetailsSuccess && downQuizDetailsSuccess)
            {
                RedirectToEngMCVehicleL2();
            }
        });
        p2VocabAnimals.onClick.AddListener(async () =>
        {
            bool downQuizDetailsSuccess = await CloudStorageHelper.DownloadAnimalsP2Quiz();
            bool downCoinsDetailsSuccess = await CloudStorageHelper.DownloadAnimalsP2QuizConisDetails();
            if (downCoinsDetailsSuccess && downQuizDetailsSuccess)
            {
                RedirectToEngMCAnimalsL2();
            }
        });
        p3CompleteCentences.onClick.AddListener(async () =>
        {
            bool downQuizDetailsSuccess = await CloudStorageHelper.DownloadP3EngCompleteSenQuizDetails();
            bool downCoinsDetailsSuccess = await CloudStorageHelper.DownloadP3EngCompleteSenCoinsDetails();
            if (downCoinsDetailsSuccess && downQuizDetailsSuccess)
            {
                RedirectToEngQuizCompleteSentencesL3();
            }
        });
        p3Preposition.onClick.AddListener(async () =>
        {
            bool downQuizDetailsSuccess = await CloudStorageHelper.DownloadP3EngPrepositionQuizDetails();
            bool downCoinsDetailsSuccess = await CloudStorageHelper.DownloadP3EngPrepositionCoinsDetails();
            if (downCoinsDetailsSuccess && downQuizDetailsSuccess)
            {
                RedirectToEngQuizPrepositionL3();
            }
        });
        p1Add.onClick.AddListener(async () =>
        {
            bool downQuizDetailsSuccess = await CloudStorageHelper.DownloadP1MathsAdditionCoinsDetails();
            bool downCoinsDetailsSuccess = await CloudStorageHelper.DownloadP1MathsAdditionQuizDetails();
            if (downCoinsDetailsSuccess && downQuizDetailsSuccess)
            {
                RedirectToMathQuizAddL1();
            }
        });
        p1Sub.onClick.AddListener(async () =>
        {
            bool downCoinsDetailsSuccess = await CloudStorageHelper.DownloadP1MathsSubtractCoinsDetails();
            bool downQuizDetailsSuccess = await CloudStorageHelper.DownloadP1MathsSubtractQuizDetails();
            if (downCoinsDetailsSuccess && downQuizDetailsSuccess)
            {
                RedirectToMathQuizSubL1();
            }
        });
        p2AddSub.onClick.AddListener(async () =>
        {
            bool downQuizDetailsSuccess = await CloudStorageHelper.DownloadP2MathsSubAddQuizDetails();
            bool downCoinsDetailsSuccess = await CloudStorageHelper.DownloadP2MathsSubAddCoinsDetails();
            if (downCoinsDetailsSuccess && downQuizDetailsSuccess)
            {
                RedirectToMathQuizAddSubL2();
            }
        });
        p2MuDiv.onClick.AddListener(async () =>
        {
            bool downQuizDetailsSuccess = await CloudStorageHelper.DownloadP2MathsMuDivQuizDetails();
            bool downCoinsDetailsSuccess = await CloudStorageHelper.DownloadP2MathsMuDivCoinsDetails();
            if (downCoinsDetailsSuccess && downQuizDetailsSuccess)
            {
                RedirectToMathQuizMuDivL2();
            }
        });
        p3Arith.onClick.AddListener(async () =>
        {
            bool downQuizDetailsSuccess = await CloudStorageHelper.DownloadP3MathsArithmeticQuizDetails();
            bool downCoinsDetailsSuccess = await CloudStorageHelper.DownloadP3MathsArithmeticCoinsDetails();
            if (downCoinsDetailsSuccess && downQuizDetailsSuccess)
            {
                RedirectToMathArithL3();
            }
        });
        p3Decimal.onClick.AddListener(async () =>
        {
            bool downQuizDetailsSuccess = await CloudStorageHelper.DownloadP3MathsDecimalQuizDetails();
            bool downCoinsDetailsSuccess = await CloudStorageHelper.DownloadP3MathsDecimalCoinsDetails();
            if (downCoinsDetailsSuccess && downQuizDetailsSuccess)
            {
                RedirectToMathDecimalsL3();
            }
        });
        p1FillIn.onClick.AddListener(async () =>
        {
            bool downQuizDetailsSuccess = await CloudStorageHelper.DownloadP1ChineseFillInCoinsDetails();
            bool downCoinsDetailsSuccess = await CloudStorageHelper.DownloadP1ChineseFillInQuizDetails();
            if (downCoinsDetailsSuccess && downQuizDetailsSuccess)
            {
                RedirectToChineseFillInL1();
            }
        });
        p1Unit.onClick.AddListener(async () =>
        {
            bool downQuizDetailsSuccess = await CloudStorageHelper.DownloadP1ChineseUnitCoinsDetails();
            bool downCoinsDetailsSuccess = await CloudStorageHelper.DownloadP1ChineseUnitQuizDetails();
            if (downCoinsDetailsSuccess && downQuizDetailsSuccess)
            {
                RedirectToChineseUnitL1();
            }
        });
        p2FillInAdvanced.onClick.AddListener(async () =>
        {
            bool downQuizDetailsSuccess = await CloudStorageHelper.DownloadP2ChineseFillInAdvancedCoinsDetails();
            bool downCoinsDetailsSuccess = await CloudStorageHelper.DownloadP2ChineseFillInAdvancedQuizDetails();
            if (downCoinsDetailsSuccess && downQuizDetailsSuccess)
            {
                RedirectToChineseFillInAdvL2();
            }
        });
        p2Head.onClick.AddListener(async () =>
        {
            bool downQuizDetailsSuccess = await CloudStorageHelper.DownloadP2ChineseHeadCoinsDetails();
            bool downCoinsDetailsSuccess = await CloudStorageHelper.DownloadP2ChineseHeadQuizDetails();
            if (downCoinsDetailsSuccess && downQuizDetailsSuccess)
            {
                RedirectToChineseHeadL2();
            }
        });
        p3Idiom1.onClick.AddListener(async () =>
        {
            bool downQuizDetailsSuccess = await CloudStorageHelper.DownloadP3ChineseIdiomCoinsDetails();
            bool downCoinsDetailsSuccess = await CloudStorageHelper.DownloadP3ChineseIdiomQuizDetails();
            if (downCoinsDetailsSuccess && downQuizDetailsSuccess)
            {
                RedirectToChineseIdiomL3();
            }
        });
        p3Idiom2.onClick.AddListener(async () =>
        {
            bool downQuizDetailsSuccess = await CloudStorageHelper.DownloadP3ChineseIdiom2CoinsDetails();
            bool downCoinsDetailsSuccess = await CloudStorageHelper.DownloadP3ChineseIdiom2QuizDetails();
            if (downCoinsDetailsSuccess && downQuizDetailsSuccess)
            {
                RedirectToChineseIdiom2L3();
            }
        });
        resetPassword.onClick.AddListener(() =>
        {
            settingPanel.SetActive(false);
            emailResetPanel.SetActive(true);
        });
        submitEmail.onClick.AddListener(async() =>
        {
            Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            string currentEmail = auth.CurrentUser.Email;
            if (emailInputField.text == currentEmail)
            {
                var task = auth.SendPasswordResetEmailAsync(currentEmail);
                await task;
                if (task.IsFaulted)
                {
                    GeneralScript.ShowMessagePanel("Canvas", "Cannot connect to Firebase");
                    return;
                }
                GeneralScript.ShowMessagePanel("Canvas", "Reset email sent. Please check your email");
            }
            else {
                GeneralScript.ShowMessagePanel("Canvas", "Your email is not match to your account.");
            }
        });
        AddExtraExerciseListener();
    }

    private void AddExtraExerciseListener()
    {
        FirebaseDatabase.DefaultInstance
            .GetReference("ChineseExtra")
            .ValueChanged += HandledDatabaseValueChanged;
        FirebaseDatabase.DefaultInstance
            .GetReference("EnglishExtra")
            .ValueChanged += HandledDatabaseValueChanged;
        FirebaseDatabase.DefaultInstance
            .GetReference("MathematicsExtra")
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
            DataSnapshot dataList = e.Snapshot;
            string subjectName = dataList.Key;
            if (subjectName == "ChineseExtra")
            {
                ClearRowsInTableChineseExtra();
            }
            else if (subjectName == "EnglishExtra")
            {
                ClearRowsInTableEnglishExtra();
            }
            else if (subjectName == "MathematicsExtra")
            {
                ClearRowsInTableMathsExtra();
            }
            foreach (DataSnapshot data in dataList.Children)
            {
                if (subjectName == "ChineseExtra")
                {
                    if (data.Child("enable").Value.ToString() == "true") {
                        var loadedObject = Resources.Load("Main/QuizButtonChinese");
                        GameObject panel = GameObject.Instantiate(loadedObject) as GameObject;
                        Text quizName = panel.transform.Find("Text").gameObject.GetComponent<Text>();
                        quizName.text = data.Child("quizName").Value.ToString() + "\n(Extra)";
                        string xxx = data.Child("quizName").Value.ToString();
                        panel.GetComponent<Button>().onClick.AddListener(async () =>
                        {
                            await CloudStorageHelper.DownloadExtraQuizDetails("ChineseExtra", xxx);
                            await CloudStorageHelper.DownloadExtraQuizCoinsDetails("ChineseExtra", xxx);
                            RedirectToExtraQuiz($"{xxx}Coins.json");
                        });
                        panel.transform.SetParent(chineseExtraScrollViewContent.transform, false);
                    }
                    
                }
                else if (subjectName == "EnglishExtra")
                {
                    var loadedObject = Resources.Load("Main/QuizButtonEnglish");
                    GameObject panel = GameObject.Instantiate(loadedObject) as GameObject;
                    Text quizName = panel.transform.Find("Text").gameObject.GetComponent<Text>();
                    quizName.text = data.Child("quizName").Value.ToString() + "\n(Extra)";
                    string xxx = data.Child("quizName").Value.ToString();
                    panel.GetComponent<Button>().onClick.AddListener(async () =>
                    {
                        await CloudStorageHelper.DownloadExtraQuizDetails("EnglishExtra", xxx);
                        await CloudStorageHelper.DownloadExtraQuizCoinsDetails("EnglishExtra", xxx);
                        RedirectToExtraQuiz($"{xxx}Coins.json");
                    });
                    panel.transform.SetParent(englishExtraScrollViewContent.transform, false);


                }
                else if (subjectName == "MathematicsExtra")
                {
                    var loadedObject = Resources.Load("Main/QuizButtonMaths");
                    GameObject panel = GameObject.Instantiate(loadedObject) as GameObject;
                    Text quizName = panel.transform.Find("Text").gameObject.GetComponent<Text>();
                    quizName.text = data.Child("quizName").Value.ToString() + "\n(Extra)";
                    string xxx = data.Child("quizName").Value.ToString();
                    panel.GetComponent<Button>().onClick.AddListener(async () =>
                    {
                        await CloudStorageHelper.DownloadExtraQuizDetails("MathematicsExtra", xxx);
                        await CloudStorageHelper.DownloadExtraQuizCoinsDetails("MathematicsExtra", xxx);
                        RedirectToExtraQuiz($"{xxx}Coins.json");
                    });
                    panel.transform.SetParent(mathsExtraScrollViewContent.transform, false);
                }
            }
        }
    }

    private void ClearRowsInTableChineseExtra()
    {
        int tableRowCount = chineseExtraScrollViewContent.transform.childCount;
        for (int i = 0; i < tableRowCount; i++)
        {
            GameObject.Destroy(chineseExtraScrollViewContent
                .transform
                .GetChild(i)
                .gameObject);
            Debug.Log(string.Format("Deleted table row {0}", i));
        }
    }

    private void RedirectToExtraQuiz(string json)
    {
        string coinsJson = File.ReadAllText(Application.persistentDataPath + "/" + json);
        ExtraCoins coin = JsonUtility.FromJson<ExtraCoins>(coinsJson);
        GameObject createNewGameObject = new GameObject("ExtraCoinsLevel");
        ExtraCoinsLevel c = createNewGameObject.AddComponent<ExtraCoinsLevel>();
        c.InitializeValue(coin.coinsGain, coin.quizDuration, coin.quizName, coin.quizSubject);
        DontDestroyOnLoad(createNewGameObject);
        GeneralScript.RedirectPageWithT("ExtraQuiz", "Redirecting to the Extra Quiz", "Canvas");
    }

    private void ClearRowsInTableEnglishExtra()
    {
        int tableRowCount = englishExtraScrollViewContent.transform.childCount;
        for (int i = 0; i < tableRowCount; i++)
        {
            GameObject.Destroy(chineseExtraScrollViewContent
                .transform
                .GetChild(i)
                .gameObject);
            Debug.Log(string.Format("Deleted table row {0}", i));
        }
    }

    private void ClearRowsInTableMathsExtra()
    {
        int tableRowCount = mathsExtraScrollViewContent.transform.childCount;
        for (int i = 0; i < tableRowCount; i++)
        {
            GameObject.Destroy(mathsExtraScrollViewContent
                .transform
                .GetChild(i)
                .gameObject);
            Debug.Log(string.Format("Deleted table row {0}", i));
        }
    }

    private void RedirectToChineseIdiom2L3()
    {
        string coinsJson = File.ReadAllText(Application.persistentDataPath + "/" + "ChineseIdiom2Coins.json");
        Coins coin = JsonUtility.FromJson<Coins>(coinsJson);
        GameObject createNewGameObject = new GameObject("CoinsLevel");
        CoinsLevel c = createNewGameObject.AddComponent<CoinsLevel>();
        c.InitializeValue(coin.coins, coin.attempt, coin.refreshRankCoins, coin.description);
        DontDestroyOnLoad(createNewGameObject);
        GeneralScript.RedirectPageWithT("Idiom2", "Redirecting to the Chinese - Idiom2(P3)", "Canvas");
    }

    private void RedirectToChineseIdiomL3()
    {
        string coinsJson = File.ReadAllText(Application.persistentDataPath + "/" + "ChineseIdiomCoins.json");
        Coins coin = JsonUtility.FromJson<Coins>(coinsJson);
        GameObject createNewGameObject = new GameObject("CoinsLevel");
        CoinsLevel c = createNewGameObject.AddComponent<CoinsLevel>();
        c.InitializeValue(coin.coins, coin.attempt, coin.refreshRankCoins, coin.description);
        DontDestroyOnLoad(createNewGameObject);
        GeneralScript.RedirectPageWithT("Idiom", "Redirecting to the Chinese - Idiom(P3)", "Canvas");
    }

    private void RedirectToChineseHeadL2()
    {
        string coinsJson = File.ReadAllText(Application.persistentDataPath + "/" + "ChineseHeadCoins.json");
        Coins coin = JsonUtility.FromJson<Coins>(coinsJson);
        GameObject createNewGameObject = new GameObject("CoinsLevel");
        CoinsLevel c = createNewGameObject.AddComponent<CoinsLevel>();
        c.InitializeValue(coin.coins, coin.attempt, coin.refreshRankCoins, coin.description);
        DontDestroyOnLoad(createNewGameObject);
        GeneralScript.RedirectPageWithT("ChineseHead", "Redirecting to the Chinese - radical(P2)", "Canvas");
    }

    private void RedirectToChineseFillInAdvL2()
    {
        string coinsJson = File.ReadAllText(Application.persistentDataPath + "/" + "ChineseFillInAdvancedCoins.json");
        Coins coin = JsonUtility.FromJson<Coins>(coinsJson);
        GameObject createNewGameObject = new GameObject("CoinsLevel");
        CoinsLevel c = createNewGameObject.AddComponent<CoinsLevel>();
        c.InitializeValue(coin.coins, coin.attempt, coin.refreshRankCoins, coin.description);
        DontDestroyOnLoad(createNewGameObject);
        GeneralScript.RedirectPageWithT("ChineseFillInAdv", "Redirecting to the Chinese - Fill In Advanced(P2)", "Canvas");
    }

    private void RedirectToChineseUnitL1()
    {
        string coinsJson = File.ReadAllText(Application.persistentDataPath + "/" + "ChineseUnitCoins.json");
        Coins coin = JsonUtility.FromJson<Coins>(coinsJson);
        GameObject createNewGameObject = new GameObject("CoinsLevel");
        CoinsLevel c = createNewGameObject.AddComponent<CoinsLevel>();
        c.InitializeValue(coin.coins, coin.attempt, coin.refreshRankCoins, coin.description);
        DontDestroyOnLoad(createNewGameObject);
        GeneralScript.RedirectPageWithT("ChineseUnit", "Redirecting to the Chinese - Unit (P1)", "Canvas");
    }

    private void RedirectToChineseFillInL1()
    {
        string coinsJson = File.ReadAllText(Application.persistentDataPath + "/" + "ChineseFillInCoins.json");
        Coins coin = JsonUtility.FromJson<Coins>(coinsJson);
        GameObject createNewGameObject = new GameObject("CoinsLevel");
        CoinsLevel c = createNewGameObject.AddComponent<CoinsLevel>();
        c.InitializeValue(coin.coins, coin.attempt, coin.refreshRankCoins, coin.description);
        DontDestroyOnLoad(createNewGameObject);
        GeneralScript.RedirectPageWithT("ChineseFillIn", "Redirecting to the Chinese - Fill In (P1)", "Canvas");
    }

    private void RedirectToMathDecimalsL3()
    {
        string coinsJson = File.ReadAllText(Application.persistentDataPath + "/" + "DecimalCoins.json");
        Coins coin = JsonUtility.FromJson<Coins>(coinsJson);
        GameObject createNewGameObject = new GameObject("CoinsLevel");
        CoinsLevel c = createNewGameObject.AddComponent<CoinsLevel>();
        c.InitializeValue(coin.coins, coin.attempt, coin.refreshRankCoins, coin.description);
        DontDestroyOnLoad(createNewGameObject);
        GeneralScript.RedirectPageWithT("Decimal", "Redirecting to the Maths - Decimal(P3)", "Canvas");
    }

    private void RedirectToMathArithL3()
    {
        string coinsJson = File.ReadAllText(Application.persistentDataPath + "/" + "ArithmeticCoins.json");
        Coins coin = JsonUtility.FromJson<Coins>(coinsJson);
        GameObject createNewGameObject = new GameObject("CoinsLevel");
        CoinsLevel c = createNewGameObject.AddComponent<CoinsLevel>();
        c.InitializeValue(coin.coins, coin.attempt, coin.refreshRankCoins, coin.description);
        DontDestroyOnLoad(createNewGameObject);
        GeneralScript.RedirectPageWithT("Arithmetic", "Redirecting to the Maths - Arithmetic(P3)", "Canvas");
    }

    private void RedirectToMathQuizMuDivL2()
    {
        string coinsJson = File.ReadAllText(Application.persistentDataPath + "/" + "MuDivCoins.json");
        Coins coin = JsonUtility.FromJson<Coins>(coinsJson);
        GameObject createNewGameObject = new GameObject("CoinsLevel");
        CoinsLevel c = createNewGameObject.AddComponent<CoinsLevel>();
        c.InitializeValue(coin.coins, coin.attempt, coin.refreshRankCoins, coin.description);
        DontDestroyOnLoad(createNewGameObject);
        GeneralScript.RedirectPageWithT("MuDiv", "Redirecting to the Maths - Multiply&Division(P2)", "Canvas");
    }

    private void RedirectToMathQuizAddSubL2()
    {
        string coinsJson = File.ReadAllText(Application.persistentDataPath + "/" + "SubAdditCoins.json");
        Coins coin = JsonUtility.FromJson<Coins>(coinsJson);
        GameObject createNewGameObject = new GameObject("CoinsLevel");
        CoinsLevel c = createNewGameObject.AddComponent<CoinsLevel>();
        c.InitializeValue(coin.coins, coin.attempt, coin.refreshRankCoins, coin.description);
        DontDestroyOnLoad(createNewGameObject);
        GeneralScript.RedirectPageWithT("SubAdd", "Redirecting to the Maths - Subtract&Addition(P2)", "Canvas");
    }

    private void RedirectToMathQuizSubL1()
    {
        string coinsJson = File.ReadAllText(Application.persistentDataPath + "/" + "SubtractCoins.json");
        Coins coin = JsonUtility.FromJson<Coins>(coinsJson);
        GameObject createNewGameObject = new GameObject("CoinsLevel");
        CoinsLevel c = createNewGameObject.AddComponent<CoinsLevel>();
        c.InitializeValue(coin.coins, coin.attempt, coin.refreshRankCoins, coin.description);
        DontDestroyOnLoad(createNewGameObject);
        GeneralScript.RedirectPageWithT("Subtract", "Redirecting to the Maths - Subtract(P1)", "Canvas");
    }

    private void RedirectToMathQuizAddL1()
    {
        string coinsJson = File.ReadAllText(Application.persistentDataPath + "/" + "AdditionCoins.json");
        Coins coin = JsonUtility.FromJson<Coins>(coinsJson);
        GameObject createNewGameObject = new GameObject("CoinsLevel");
        CoinsLevel c = createNewGameObject.AddComponent<CoinsLevel>();
        c.InitializeValue(coin.coins, coin.attempt, coin.refreshRankCoins, coin.description);
        DontDestroyOnLoad(createNewGameObject);
        GeneralScript.RedirectPageWithT("Addition", "Redirecting to the Maths - Addition(P1)", "Canvas");
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

    void RedirectToEngMCVehicleL1()
    {
        string coinsJson = File.ReadAllText(Application.persistentDataPath + "/" + "VocabularyVehicleCoins1.json");
        Coins coin = JsonUtility.FromJson<Coins>(coinsJson);
        GameObject createNewGameObject = new GameObject("CoinsLevel");
        CoinsLevel c = createNewGameObject.AddComponent<CoinsLevel>();
        c.InitializeValue(coin.coins, coin.attempt, coin.refreshRankCoins, coin.description);
        DontDestroyOnLoad(createNewGameObject);
        GeneralScript.RedirectPageWithT("Vehicle1", "Redirecting to the Vocab - Vehicle(P1)", "Canvas");
    }

    void RedirectToEngMCVehicleL2()
    {
        string coinsJson = File.ReadAllText(Application.persistentDataPath + "/" + "VocabularyVehicleCoins2.json");
        Coins coin = JsonUtility.FromJson<Coins>(coinsJson);
        GameObject createNewGameObject = new GameObject("CoinsLevel");
        CoinsLevel c = createNewGameObject.AddComponent<CoinsLevel>();
        c.InitializeValue(coin.coins, coin.attempt, coin.refreshRankCoins, coin.description);
        DontDestroyOnLoad(createNewGameObject);
        GeneralScript.RedirectPageWithT("Vehicle2", "Redirecting to the Vocab - Vehicle(P2)", "Canvas");
    }

    void RedirectToEngMCAnimalsL2()
    {
        string coinsJson = File.ReadAllText(Application.persistentDataPath + "/" + "VocabularyAnimalsCoins2.json");
        Coins coin = JsonUtility.FromJson<Coins>(coinsJson);
        GameObject createNewGameObject = new GameObject("CoinsLevel");
        CoinsLevel c = createNewGameObject.AddComponent<CoinsLevel>();
        c.InitializeValue(coin.coins, coin.attempt, coin.refreshRankCoins, coin.description);
        DontDestroyOnLoad(createNewGameObject);
        GeneralScript.RedirectPageWithT("Animals2", "Redirecting to the Vocab - Animals(P2)", "Canvas");
    }

    void RedirectToEngQuizCompleteSentencesL3()
    {
        string coinsJson = File.ReadAllText(Application.persistentDataPath + "/" + "FillInTheBlanksCoins.json");
        Coins coin = JsonUtility.FromJson<Coins>(coinsJson);
        GameObject createNewGameObject = new GameObject("CoinsLevel");
        CoinsLevel c = createNewGameObject.AddComponent<CoinsLevel>();
        c.InitializeValue(coin.coins, coin.attempt, coin.refreshRankCoins, coin.description);
        DontDestroyOnLoad(createNewGameObject);
        GeneralScript.RedirectPageWithT("CompleteSentences", "Redirecting to the Quiz - Complete Sentences(P3)", "Canvas");
    }

    void RedirectToEngQuizPrepositionL3()
    {
        string coinsJson = File.ReadAllText(Application.persistentDataPath + "/" + "PrepositionCoins.json");
        Coins coin = JsonUtility.FromJson<Coins>(coinsJson);
        GameObject createNewGameObject = new GameObject("CoinsLevel");
        CoinsLevel c = createNewGameObject.AddComponent<CoinsLevel>();
        c.InitializeValue(coin.coins, coin.attempt, coin.refreshRankCoins, coin.description);
        DontDestroyOnLoad(createNewGameObject);
        GeneralScript.RedirectPageWithT("Preposition", "Redirecting to the Quiz - Complete Sentences(P3)", "Canvas");
    }

    void ShowEngSelect()
    {
        backBtnEng.gameObject.SetActive(false);
        engSelectLevelScrollView.gameObject.SetActive(true);
        P1ScrollView.SetActive(false);
        P2ScrollView.SetActive(false);
        P3ScrollView.SetActive(false);
        englishExtraScrollView.SetActive(false);
    }

    void ShowMathsSelect()
    {
        backBtnMaths.gameObject.SetActive(false);
        mathsSelectLevelScrollView.gameObject.SetActive(true);
        P1ScrollViewMaths.SetActive(false);
        P2ScrollViewMaths.SetActive(false);
        P3ScrollViewMaths.SetActive(false);
        mathsExtraScrollView.SetActive(false);
    }

    void ShowChineseSelect()
    {
        backBtnMaths.gameObject.SetActive(false);
        chineseSelectLevelScrollView.gameObject.SetActive(true);
        P1ScrollViewChinese.SetActive(false);
        P2ScrollViewChinese.SetActive(false);
        P3ScrollViewChinese.SetActive(false);
        chineseExtraScrollView.SetActive(false);
    }

    void ShowChineseExtra()
    {
        backBtnChinese.gameObject.SetActive(true);
        chineseSelectLevelScrollView.gameObject.SetActive(false);
        chineseExtraScrollView.SetActive(true);
    }

    void ShowEnglishExtra()
    {
        backBtnEng.gameObject.SetActive(true);
        engSelectLevelScrollView.gameObject.SetActive(false);
        englishExtraScrollView.SetActive(true);
    }

    void ShowMathsExtra()
    {
        backBtnMaths.gameObject.SetActive(true);
        mathsSelectLevelScrollView.gameObject.SetActive(false);
        mathsExtraScrollView.SetActive(true);
    }

    void ShowP1Eng()
    {
        backBtnEng.gameObject.SetActive(true);
        engSelectLevelScrollView.gameObject.SetActive(false);
        P1ScrollView.SetActive(true);
    }

    void ShowP2Eng()
    {
        backBtnEng.gameObject.SetActive(true);
        engSelectLevelScrollView.gameObject.SetActive(false);
        P2ScrollView.SetActive(true);
    }

    void ShowP3Eng()
    {
        backBtnEng.gameObject.SetActive(true);
        engSelectLevelScrollView.gameObject.SetActive(false);
        P3ScrollView.SetActive(true);
    }

    void ShowP1Maths()
    {
        backBtnMaths.gameObject.SetActive(true);
        mathsSelectLevelScrollView.gameObject.SetActive(false);
        P1ScrollViewMaths.SetActive(true);
    }

    void ShowP2Maths()
    {
        backBtnMaths.gameObject.SetActive(true);
        mathsSelectLevelScrollView.gameObject.SetActive(false);
        P2ScrollViewMaths.SetActive(true);
    }

    void ShowP3Maths()
    {
        backBtnMaths.gameObject.SetActive(true);
        mathsSelectLevelScrollView.gameObject.SetActive(false);
        P3ScrollViewMaths.SetActive(true);
    }

    void ShowP1Chinese()
    {
        backBtnChinese.gameObject.SetActive(true);
        chineseSelectLevelScrollView.gameObject.SetActive(false);
        P1ScrollViewChinese.SetActive(true);
    }

    void ShowP2Chinese()
    {
        backBtnChinese.gameObject.SetActive(true);
        chineseSelectLevelScrollView.gameObject.SetActive(false);
        P2ScrollViewChinese.SetActive(true);
    }

    void ShowP3Chinese()
    {
        backBtnChinese.gameObject.SetActive(true);
        chineseSelectLevelScrollView.gameObject.SetActive(false);
        P3ScrollViewChinese.SetActive(true);
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
