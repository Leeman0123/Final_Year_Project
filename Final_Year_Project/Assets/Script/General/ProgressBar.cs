using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{

    [Header("Loading Panel")]
    [SerializeField] Slider slider;
    public string scene;
    public string message;
    // Start is called before the first frame update

    void Awake() {
        gameObject.transform.Find("Message").GetComponent<Text>().text = message;
    }

    void LoadScene()
    {
        StartCoroutine(LoadProgressBar());
    }

    void Start()
    {
        LoadScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadProgressBar()
    {
        AsyncOperation operation;
        operation = SceneManager.LoadSceneAsync(scene);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            yield return null;
        }
    }
}
