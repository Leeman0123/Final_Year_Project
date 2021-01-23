using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorrectEnemyText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string Correctans = JSONReader.instance.getCorrect();
        transform.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>().text = Correctans;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
