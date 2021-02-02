using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WrongEnemyText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string Wrongans = JSONReader.instance.GetPastSimpleWrong();
        transform.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().text = Wrongans;
    }
}
