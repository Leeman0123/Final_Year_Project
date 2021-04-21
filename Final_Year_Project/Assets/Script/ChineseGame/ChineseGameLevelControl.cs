using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Tools;
using MoreMountains.CorgiEngine;
using Firebase.Database;
using System;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class ChineseGameLevelControl : MonoBehaviour
{
    private CheckAuthentication script;
    private DatabaseReference reference;
    [SerializeField] GameObject CheckAuth;
    [SerializeField] Button LVone;
    [SerializeField] Button LVtwo;
    [SerializeField] Button LVthree;
    [SerializeField] Button Normal;
    [SerializeField] Button Fly;
    [SerializeField] Button Shoot;
    [SerializeField] GameObject GoToLevel;
    [SerializeField] GameObject BackButton;
    [SerializeField] GameObject CharacterSelectPage;
    [SerializeField] Text MoneyText;
    [SerializeField] GameObject NotEnoughMessage;
    [SerializeField] Button NotEnoughMessageButton;
    [SerializeField] Button SelectPageBackButton;
    private string Levelname;

    private void Start()
    {

        reference = FirebaseDatabase.DefaultInstance.RootReference;
        Levelname = null;
        LVone.onClick.AddListener(() => GetLevel(1));
        LVtwo.onClick.AddListener(() => GetLevel(2));
        LVthree.onClick.AddListener(() => GetLevel(3));
        Normal.onClick.AddListener(() => GetCharacter(1));
        Fly.onClick.AddListener(() => GetCharacter(2));
        Shoot.onClick.AddListener(() => GetCharacter(3));
        NotEnoughMessageButton.onClick.AddListener(() => OKButtonControl());
        SelectPageBackButton.onClick.AddListener(() => BackButtonControl());
    }
    public void GetLevel(int level)
    {
        switch (level)
        {
            case 1:
                Levelname = "ChineseGameL1";
                BackButton.SetActive(false);
                break;
            case 2:
                Levelname = "ChineseGameL2";
                BackButton.SetActive(false);
                break;
            case 3:
                Levelname = "ChineseGameL3";
                BackButton.SetActive(false);
                break;
        }
        Debug.Log(Levelname);
        CharacterSelectPage.SetActive(true);
    }
    public void GetCharacter(int character)
    {
        MoreMountains.CorgiEngine.LevelSelector level = GameObject.Find("LevelSelector").GetComponent<MoreMountains.CorgiEngine.LevelSelector>();
        switch (character)
        {
            case 1:

                level.LevelName = Levelname;
                level.GoToLevel();
                break;
            case 2:
                if (int.Parse(MoneyText.text) < 20)
                {
                    NotEnoughMessage.SetActive(true);
                    break;
                }
                else
                {
                    Levelname += "Fly";
                    SetPlayerCoins(int.Parse(MoneyText.text), 20);
                    level.LevelName = Levelname;
                    level.GoToLevel();
                    break;
                }
            case 3:
                if (int.Parse(MoneyText.text) < 30)
                {
                    NotEnoughMessage.SetActive(true);
                    break;
                }
                else
                {
                    Levelname += "Double";
                    SetPlayerCoins(int.Parse(MoneyText.text), 40);
                    LoadingSceneManager.LoadScene(Levelname);
                    //level.LevelName = Levelname;
                    //level.GoToLevel();
                    break;
                }
        }
        Debug.Log(Levelname);
    }
    private async void SetPlayerCoins(int coins, int decreaseValue)
    {
        coins -= decreaseValue;
        bool isSuccess = await SetCoin(coins);
        if (!isSuccess)
        {
            Debug.LogError("Set conis insuccess");
        }
        else
        {
            Debug.Log("After value : " + coins);
        }
    }
    public void OKButtonControl()
    {
        NotEnoughMessage.SetActive(false);
    }
    public void BackButtonControl()
    {
        CharacterSelectPage.SetActive(false);
        BackButton.SetActive(true);
        Levelname = null;

    }

    public async Task<bool> SetCoin(int coin)
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        string userId = auth.CurrentUser.UserId;
        var task = reference.Child("students").Child(userId).Child("coins").SetValueAsync(coin);
        await task;
        if (task.Exception != null)
        {
            return false;
        }
        return true;
    }
}
