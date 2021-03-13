using UnityEngine;

using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class SpaceShooterLevelSelector : MonoBehaviour {

    public Button[] levelbuttons;
    public Button backButton;
    public Sprite lockimage;

    private void Start() {

        int levelReached = PlayerPrefs.GetInt("spaceshooterLevelReached", 1);

        for (int i = 0; i < levelbuttons.Length; i++) {
            if (i + 1 > levelReached) {
                levelbuttons[i].interactable = false;
                levelbuttons[i].GetComponent<Image>().sprite = lockimage;
            }
        }
    }

    public void Select(string levelName) {
        SceneManager.LoadScene(levelName);
    }

    public void BackToMain() {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs reset");
        SceneManager.LoadScene("Main");
    }
}