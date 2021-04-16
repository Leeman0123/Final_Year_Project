using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] GameObject panelBackground;
    [SerializeField] GameObject storePanel;
    [SerializeField] Button closeBtn;
    [SerializeField] GameObject buyErrorPanel;
    [SerializeField] Button okBtn;
    [Header("Coins")]
    [SerializeField] Text coinsTotal;
    private int coins;
    [Header("Space Shooter")]
    [SerializeField] GameObject[] spaceShooterItemBtn;
    [SerializeField] Text[] spaceShooterItemText;
    private int[] spaceShooterItemOwned = new int[3];


    private CheckAuthentication script;
    private DatabaseReference reference;
    private string userID;

    private Dictionary<string, int> itemDictionary = new Dictionary<string, int>();
    // Start is called before the first frame update
    void Start()
    {
        script = GameObject.Find("CheckAuth").GetComponent<CheckAuthentication>();
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        userID = script.GetUserId();
        CreateItemDictionary();
        StartCoroutine(ShowItemAmount());
        StartCoroutine(GetCoinsTotal());
        okBtn.onClick.AddListener(() => buyErrorPanel.SetActive(false));
    }

    void CreateItemDictionary() {
        itemDictionary.Add("Invincible", 25);
        itemDictionary.Add("SpecialMode", 20);
        itemDictionary.Add("DoubleCoin", 30);
    }

    void RefreshPage() {
        StartCoroutine(GetCoinsTotal());
        StartCoroutine(ShowItemAmount());
        coinsTotal.text = coins.ToString();
    }

    public IEnumerator GetCoinsTotal() {
        var getTask = FirebaseDatabase.DefaultInstance
        .GetReference("students")
        .Child(userID)
        .GetValueAsync();
        yield return new WaitUntil(() => getTask.IsCompleted || getTask.IsFaulted);
        if (getTask.IsCompleted) {
            Dictionary<string, object> results = (Dictionary<string, object>)getTask.Result.Value;
            coins = int.Parse(results["coins"].ToString());
            Debug.Log("Coins : " + coins);
        }
    }

    public IEnumerator GetSpaceShooterItem() {
        var getTask = FirebaseDatabase.DefaultInstance
        .GetReference("students")
        .Child(userID)
        .Child("ItemAmount")
        .GetValueAsync();
        yield return new WaitUntil(() => getTask.IsCompleted || getTask.IsFaulted);
        if (getTask.IsCompleted) {
            Dictionary<string, object> results = (Dictionary<string, object>)getTask.Result.Value;
            Debug.Log(results);
            spaceShooterItemOwned[0] = int.Parse(results["Invincible"].ToString());
            spaceShooterItemOwned[1] = int.Parse(results["SpecialMode"].ToString());
            spaceShooterItemOwned[2] = int.Parse(results["DoubleCoin"].ToString());
        }

        //FirebaseDatabase dbInstance = FirebaseDatabase.DefaultInstance;
        //dbInstance.GetReference("SpaceShooter").Child("ItemAmount").Child(userID).GetValueAsync().ContinueWithOnMainThread(task => { 
        //    if (task.IsCompleted) {                
        //        DataSnapshot snapshot = task.Result;
        //        Debug.Log(snapshot.Child("Invincible").Value.ToString());
        //        spaceShooterItemOwned[0] = int.Parse(snapshot.Child("Invincible").Value.ToString());
        //        spaceShooterItemOwned[1] = int.Parse(snapshot.Child("SpecialMode").Value.ToString());
        //        spaceShooterItemOwned[2] = int.Parse(snapshot.Child("DoubleCoin").Value.ToString());
        //    }
        //});
    }

    private IEnumerator ShowItemAmount() {
        yield return StartCoroutine(GetSpaceShooterItem());
        for (int i = 0; i <= 2; i++) {
            spaceShooterItemText[i].text = "Owned : " + spaceShooterItemOwned[i];
        }
    }

    public void CloseStore() {
        //panelBackground.SetActive(false);
        storePanel.SetActive(false);
    }
    
    public void buyItem(string itemName) {
        int coin = itemDictionary[itemName];
        Debug.Log("ItemName : " + itemName + " ItemPrice : " + coin);
        if (coins >= coin) {
            coins -= coin;
            Debug.Log(coins);
            reference.Child("students").Child(userID).Child("coins").SetValueAsync(coins);
            FirebaseDatabase dbInstance = FirebaseDatabase.DefaultInstance;
            dbInstance.GetReference("students").Child(userID).Child("ItemAmount").GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;
                    int temp = int.Parse(snapshot.Child(itemName).Value.ToString());
                    temp++;
                    reference.Child("students").Child(userID).Child("ItemAmount").Child(itemName).SetValueAsync(temp);
                    RefreshPage();
                }
            });
        } else {
            buyErrorPanel.SetActive(true);
        }
    }
}
