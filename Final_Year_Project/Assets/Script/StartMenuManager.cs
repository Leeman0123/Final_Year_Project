using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    public void QuitApplication()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void LoadLoginSceneButton()
    {
        StartCoroutine(LoadLoginScene());
    }

    IEnumerator LoadLoginScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LoginScene");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
