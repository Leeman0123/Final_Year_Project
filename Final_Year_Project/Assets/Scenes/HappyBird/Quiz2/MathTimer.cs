using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MathTimer : MonoBehaviour
{
    [SerializeField] Slider timeSlider;
    [SerializeField] Text timeText;
    [SerializeField] float gameTime;
    private bool stopTimer;
    // Start is called before the first frame update
    void Start()
    {
        stopTimer = false;
        timeSlider.maxValue = gameTime;
        timeSlider.value = gameTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (stopTimer == false)
        {
            float time = gameTime - Time.time;
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time - minutes * 60f);
            string textTime = string.Format("{0:0}:{1:00}", minutes, seconds);
            //Debug.Log(time);
            if (time <= 0)
            {
                gameTime += 30;
                time += 30;

            }
            if (time > 20 && time <= 30)
            {
                timeSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = new Color32(48, 246, 96, 255);
            }
            else if (time > 10 && time <= 20)
            {
                timeSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = new Color32(231, 214, 90, 255);
            }
            else if (time <= 10)
            {
                timeSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = new Color32(232, 103, 100, 255);
            }
            timeText.text = textTime;
            timeSlider.value = time;
        }
    }
}
