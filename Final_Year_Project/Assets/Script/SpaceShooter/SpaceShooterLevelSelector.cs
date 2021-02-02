using UnityEngine;

using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class SpaceShooterLevelSelector : MonoBehaviour {

    public Button[] levelbuttons;

    private void Start() {

        int levelReached = PlayerPrefs.GetInt("spaceshooterLevelReached", 1);

        for (int i = 0; i < levelbuttons.Length; i++) {
            if (i + 1 > levelReached)
                levelbuttons[i].interactable = false;
        }
    }

    public void Select(string levelName) {

        SceneManager.LoadScene(levelName);

    }
}