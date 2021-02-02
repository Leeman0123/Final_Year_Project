using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorrectEnemyTextPastParticiple : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string Correctans = JSONReader.instance.GetPastParticipleCorrect();
        transform.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().text = Correctans;
    }
}
