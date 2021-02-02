using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WrongEnemyTextPastParticiple : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string Wrongans = JSONReader.instance.GetPastParticipleWrong();
        transform.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().text = Wrongans;
    }
}
