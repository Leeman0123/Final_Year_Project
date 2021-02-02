using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGroupControl : MonoBehaviour
{
    [Header("Subject Main View")]
    [SerializeField] GameObject mainView;
    [Header("Subject Selection")]
    [SerializeField] Button chineseBtn;
    [SerializeField] Button englishBtn;
    [SerializeField] Button mathsBtn;
    [SerializeField] GameObject chineseView;
    [SerializeField] GameObject englishView;
    [SerializeField] GameObject mathsView;
    [Header("Subject Type Close Btn")]
    [SerializeField] Button subjectCloseBtn;
    // Start is called before the first frame update
    void Start()
    {
        chineseBtn.onClick.AddListener(() => ShowChineseView());
        englishBtn.onClick.AddListener(() => ShowEnglishView());
        mathsBtn.onClick.AddListener(() => ShowMathsView());
        subjectCloseBtn.onClick.AddListener(() => BackMainButtonGroup());
    }

    void ShowMainView()
    {
        mainView.SetActive(true);
    }

    void CloseMainView()
    {
        mainView.SetActive(false);
    }

    void BackMainButtonGroup()
    {
        ShowMainView();
        CloseQuizTypeView();
        subjectCloseBtn.gameObject.SetActive(false);
    }

    void CloseQuizTypeView()
    {
        chineseView.SetActive(false);
        englishView.SetActive(false);
        mathsView.SetActive(false);
    }

    void ShowChineseView()
    {
        chineseView.SetActive(true);
        englishView.SetActive(false);
        mathsView.SetActive(false);
        subjectCloseBtn.gameObject.SetActive(true);
        CloseMainView();
    }

    void ShowEnglishView()
    {
        chineseView.SetActive(false);
        englishView.SetActive(true);
        mathsView.SetActive(false);
        subjectCloseBtn.gameObject.SetActive(true);
        CloseMainView();
    }
    void ShowMathsView()
    {
        chineseView.SetActive(false);
        englishView.SetActive(false);
        mathsView.SetActive(true);
        subjectCloseBtn.gameObject.SetActive(true);
        CloseMainView();
    }

}
