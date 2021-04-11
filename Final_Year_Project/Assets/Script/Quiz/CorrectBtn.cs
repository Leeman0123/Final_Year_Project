using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class CorrectBtn
{
    public int index { get; set; }
    public Button button { get; set; }

    public CorrectBtn(int index, Button button)
    {
        this.index = index;
        this.button = button;
    }
}
