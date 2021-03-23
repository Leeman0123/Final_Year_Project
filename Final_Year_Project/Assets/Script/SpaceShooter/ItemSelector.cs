using Firebase.Database;
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
    private string userID;

    // Start is called before the first frame update
    void Start()
    {
        itemInstance = this;
        script = GameObject.Find("CheckAuth").GetComponent<CheckAuthentication>();
        userID = script.GetUserId();
        Debug.Log(userID);
        StartCoroutine(GetItemAmount());
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

    private void ShowItemAmount() {
        for (int i = 0; i <= 2; i++) {
            itemText[i].text = "Owned : " + itemOwned[i];
            itemEnable[i] = false;
            if (itemOwned[i] <= 0) {
                itemButton[i].enabled = false;
                itemButton[i].GetComponent<Image>().sprite = disableImage;
            }
        }
    }

    IEnumerator GetItemAmount() {
        var getTask = FirebaseDatabase.DefaultInstance
        .GetReference("SpaceShooter")
        .Child("ItemAmount")
        .Child(userID)
        .GetValueAsync();
        yield return new WaitUntil(() => getTask.IsCompleted || getTask.IsFaulted);
        if (getTask.IsCompleted) {
            Dictionary<string, object> results = (Dictionary<string, object>)getTask.Result.Value;
            itemOwned[0] = int.Parse(results["Invincible"].ToString());
            itemOwned[1] = int.Parse(results["SpecialMode"].ToString());
            itemOwned[2] = int.Parse(results["DoubleCoin"].ToString());
        }
        ShowItemAmount();
    }

    public IEnumerator SetInvincibleAmount(int change) {
        itemOwned[0] = itemOwned[0] + change;
        var DBTask = script.DBreference
            .Child("SpaceShooter")
            .Child("ItemAmount")
            .Child(userID)
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
            .Child("SpaceShooter")
            .Child("ItemAmount")
            .Child(userID)
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
            .Child("SpaceShooter")
            .Child("ItemAmount")
            .Child(userID)
            .Child("DoubleCoin")
            .SetValueAsync(itemOwned[2]);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        if (DBTask.Exception != null) {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        } else {
            Debug.Log("Double Coin Amount Updated. New Amount : " + itemOwned[2]);
        }
    }
}
