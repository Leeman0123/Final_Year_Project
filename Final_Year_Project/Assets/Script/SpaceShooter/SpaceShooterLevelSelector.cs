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

    private CheckAuthentication script;

    private void Start() {

        script = GameObject.Find("CheckAuth").GetComponent<CheckAuthentication>();
        StartCoroutine(GetPlayerLevel());

        Screen.orientation = ScreenOrientation.Portrait;
    }

    public void Select(string levelName) {
        SceneManager.LoadScene(levelName);
    }

    public void BackToMain() {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs reset");
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
        string userID = script.GetUserId();
        Debug.Log(userID);
        var getTask = FirebaseDatabase.DefaultInstance
        .GetReference("SpaceShooter")
        .Child("levelReached")
        .Child(userID)
        .GetValueAsync();
        yield return new WaitUntil(() => getTask.IsCompleted || getTask.IsFaulted);
        if (getTask.IsCompleted) {
            Dictionary<string, object> results = (Dictionary<string, object>)getTask.Result.Value;
            int levelReached = int.Parse(results["levelReached"].ToString());
            Debug.Log("Player reached level " + levelReached);
            UnlockLevel(levelReached);
        }
    }
}