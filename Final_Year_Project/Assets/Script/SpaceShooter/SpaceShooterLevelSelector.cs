using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class SpaceShooterLevelSelector : MonoBehaviour {

    public Button[] levelbuttons;
    public Button backButton;
    public Sprite lockimage;

    public int _levelReached;
    private string userID;

    private CheckAuthentication script;
    private DatabaseReference reference;

    public static SpaceShooterLevelSelector ssLevelSelectorInstance;

    private void Start() {

        script = GameObject.Find("CheckAuth").GetComponent<CheckAuthentication>();
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        userID = script.GetUserId();
        StartCoroutine(GetPlayerLevel());

        Screen.orientation = ScreenOrientation.Portrait;
        ssLevelSelectorInstance = this;
    }

    public void Select(string levelName) {
        SceneManager.LoadScene(levelName);
    }

    public void BackToMain() {
        SceneManager.LoadScene("Main");
    }

    private void UnlockLevel(int levelReached) {
        for (int i = 0; i < levelbuttons.Length; i++) {
            if (i + 1 > levelReached) {
                levelbuttons[i].interactable = false;
                levelbuttons[i].GetComponent<Image>().sprite = lockimage;
            }
        }
    }
    IEnumerator GetPlayerLevel() {
        var getTask = FirebaseDatabase.DefaultInstance
        .GetReference("SpaceShooter")
        .Child("levelReached")
        .GetValueAsync();
        yield return new WaitUntil(() => getTask.IsCompleted || getTask.IsFaulted);
        if (getTask.IsCompleted) {
            Dictionary<string, object> results = (Dictionary<string, object>)getTask.Result.Value;
            int levelReached = int.Parse(results[userID].ToString());
            _levelReached = levelReached;
            Debug.Log("Player reached level " + levelReached);
            UnlockLevel(levelReached);
        }
    }

    public void SetPlayerLevel(int level) {
        reference.Child("SpaceShooter").Child("levelReached").Child(userID).SetValueAsync(level);
    }
}