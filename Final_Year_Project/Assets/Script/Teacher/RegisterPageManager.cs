using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPageManager : MonoBehaviour
{
    [SerializeField] Button buttonBack;
    [SerializeField] GameObject backgroundPanel;
    [SerializeField] GameObject progressPanel;
    // Start is called before the first frame update
    void Start()
    {
        buttonBack.onClick.AddListener(() => ShowPanel());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowPanel(){
        backgroundPanel.SetActive(true);
        progressPanel.SetActive(true);
    }
}
