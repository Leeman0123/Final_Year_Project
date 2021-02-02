using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Text HPText;
    public float hp;

    void Start()
    {
        DisplayHP();
    }

    public void Sleep()
    {
        hp += 10;
        DisplayHP();
    }

    public void DisplayHP()
    {
        HPText.text = hp.ToString("#");
    }

    public void BeAttacked()
    {
        hp -= 10.6f;
        DisplayHP();
    }
}
