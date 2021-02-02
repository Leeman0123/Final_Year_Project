using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Tools;
using MoreMountains.CorgiEngine;

public class ChineseGameFinalResult : MonoBehaviour
{

    [SerializeField] Text Finalresult;
    [SerializeField] GameObject Final;
    [SerializeField] Text PointText;
    public int point;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Final.SetActive(true);
            point = int.Parse(PointText.text);
            Finalresult.text = "You get " + point.ToString() + " point in this game";
            
            GameObject.Find("Rectangle").GetComponent<Character>().gameObject.SetActive(false);
 
            
        }
    }
}
