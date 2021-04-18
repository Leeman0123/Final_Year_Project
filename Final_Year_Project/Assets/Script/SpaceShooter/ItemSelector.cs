using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelector : MonoBehaviour
{

    public Button[] itemButton;
    public Text[] itemText;

    public static ItemSelector itemInstance;

    public int[] itemOwned;

    public bool[] itemEnable = new bool[3];

    public Sprite enableImage, disableImage, selectedImage;

    private CheckAuthentication script;
    private DatabaseReference reference;
    private string userID;

    // Start is called before the first frame update
    void Start()
    {
        itemInstance = this;
        script = GameObject.Find("CheckAuth").GetComponent<CheckAuthentication>();
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        userID = script.GetUserId();
        Debug.Log(userID);
        StartCoroutine(ShowItemAmount());
    }

    public void ChangeColor(int itemselected) {
        if (!itemEnable[itemselected]) {
            itemEnable[itemselected] = true;
            itemButton[itemselected].GetComponent<Image>().sprite = selectedImage;
            Debug.Log(itemButton[itemselected].GetComponentInChildren<Text>().text + " Enable");
        } else {
            itemEnable[itemselected] = false;
            itemButton[itemselected].GetComponent<Image>().sprite = enableImage;
            Debug.Log(itemButton[itemselected].GetComponentInChildren<Text>().text + " Disable");
        }
    }

    private IEnumerator ShowItemAmount() {
        yield return StartCoroutine(GetItemAmount());
        for (int i = 0; i <= 2; i++) {
            itemText[i].text = "Owned : " + itemOwned[i];
            itemEnable[i] = false;
            if (itemOwned[i] <= 0) {
                itemButton[i].interactable = false;
                itemButton[i].GetComponent<Image>().sprite = disableImage;
            }
        }
    }

    IEnumerator GetItemAmount() {
        var getTask = FirebaseDatabase.DefaultInstance
        .GetReference("students")
        .Child(userID)
        .Child("ItemAmount")
        .GetValueAsync();
        yield return new WaitUntil(() => getTask.IsCompleted || getTask.IsFaulted);
        if (getTask.IsCompleted) {
            Dictionary<string, object> results = (Dictionary<string, object>)getTask.Result.Value;
            Debug.Log(results);
            itemOwned[0] = int.Parse(results["Invincible"].ToString());
            itemOwned[1] = int.Parse(results["SpecialMode"].ToString());
            itemOwned[2] = int.Parse(results["DoubleCoin"].ToString());
        }
    }

    public IEnumerator SetInvincibleAmount(int change) {
        itemOwned[0] = itemOwned[0] + change;
        var DBTask = script.DBreference
            .Child("students")
            .Child(userID)
            .Child("ItemAmount")
            .Child("Invincible")
            .SetValueAsync(itemOwned[0]);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        if (DBTask.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        } else {
            Debug.Log("Invincible Amount Updated. New Amount : " + itemOwned[0]);
        }
    }

    public IEnumerator SetSpecialModeAmount(int change) {
        itemOwned[1] = itemOwned[1] + change;
        var DBTask = script.DBreference
            .Child("students")
            .Child(userID)
            .Child("ItemAmount")
            .Child("SpecialMode")
            .SetValueAsync(itemOwned[1]);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        if (DBTask.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        } else {
            Debug.Log("Special Mode Amount Updated. New Amount : " + itemOwned[1]);
        }
    }

    public IEnumerator SetDoubleCoinAmount(int change) {
        itemOwned[2] = itemOwned[2] + change;
        var DBTask = script.DBreference
            .Child("students")
            .Child(userID)
            .Child("ItemAmount")
            .Child("DoubleCoin")
            .SetValueAsync(itemOwned[2]);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        if (DBTask.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        } else {
            Debug.Log("Double Coin Amount Updated. New Amount : " + itemOwned[2]);
        }
    }

    public void SetItemAmount(string itemName) {
        FirebaseDatabase dbInstance = FirebaseDatabase.DefaultInstance;
        dbInstance.GetReference("students").Child(userID).Child("ItemAmount").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                int temp = int.Parse(snapshot.Child(itemName).Value.ToString());
                temp--;
                reference.Child("students").Child(userID).Child("ItemAmount").Child(itemName).SetValueAsync(temp);
            }
        });
    }
}
