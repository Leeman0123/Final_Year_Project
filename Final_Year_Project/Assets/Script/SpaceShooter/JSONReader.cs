﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONReader : MonoBehaviour
{

    public TextAsset textjson;
    public static JSONReader instance;

    [System.Serializable]
    public class Verb {
        public string basetense;
        public string pastsimple;
        public string pastparticiple;
    }

    [System.Serializable]
    public class VerbList {
        public Verb[] verb;
    }

    public VerbList myVerbList = new VerbList();

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        try {
            myVerbList = JsonUtility.FromJson<VerbList>(textjson.text);
            if (myVerbList.verb[1].basetense != null) {
                Debug.Log("Load json success");
            }
        } catch { 
            Debug.LogError("No json can read");
        }
    }

    public string GetPastSimpleWrong() {
        int randomnum = Random.Range(0, myVerbList.verb.Length);
        string basetense = myVerbList.verb[randomnum].basetense;
        string pasttense = myVerbList.verb[randomnum].pastsimple;
        string pastparticiple = myVerbList.verb[randomnum].pastparticiple;
        Debug.Log("Base:" + basetense + " Pass:" + pasttense + " PP:" + pastparticiple);
        while (pasttense.Equals(basetense) || pasttense.Equals(pastparticiple)) {
            randomnum = Random.Range(0, myVerbList.verb.Length);
            basetense = myVerbList.verb[randomnum].basetense;
            pasttense = myVerbList.verb[randomnum].pastsimple;
            pastparticiple = myVerbList.verb[randomnum].pastparticiple;
            Debug.Log("Base:" + basetense + " Pass:" + pasttense + " PP:" + pastparticiple);
        }
        return pastparticiple;
    }

    public string GetPastSimpleCorrect() {
        string correct;
        correct = myVerbList.verb[Random.Range(0, myVerbList.verb.Length)].pastsimple;
        return correct;
    }

    public string GetBaseWrong() {
        int randomnum = Random.Range(0, myVerbList.verb.Length);
        string basetense = myVerbList.verb[randomnum].basetense;
        string pasttense = myVerbList.verb[randomnum].pastsimple;
        string pastparticiple = myVerbList.verb[randomnum].pastparticiple;
        Debug.Log("Base:" + basetense + " Pass:" + pasttense + " PP:" + pastparticiple);
        while (basetense.Equals(pasttense) || basetense.Equals(pastparticiple)) {
            randomnum = Random.Range(0, myVerbList.verb.Length);
            basetense = myVerbList.verb[randomnum].basetense;
            pasttense = myVerbList.verb[randomnum].pastsimple;
            pastparticiple = myVerbList.verb[randomnum].pastparticiple;
            Debug.Log("Base:" + basetense + " Pass:" + pasttense + " PP:" + pastparticiple);
        }
        return pastparticiple;
    }

    public string GetBaseCorrect() {
        string correct;
        correct = myVerbList.verb[Random.Range(0, myVerbList.verb.Length)].basetense;
        return correct;
    }

    public string GetPastParticipleWrong() {
        int randomnum = Random.Range(0, myVerbList.verb.Length);
        string basetense = myVerbList.verb[randomnum].basetense;
        string pasttense = myVerbList.verb[randomnum].pastsimple;
        string pastparticiple = myVerbList.verb[randomnum].pastparticiple;
        Debug.Log("Base:" + basetense + " Pass:" + pasttense + " PP:" + pastparticiple);
        while (pastparticiple.Equals(pasttense) || pastparticiple.Equals(pasttense)) {
            randomnum = Random.Range(0, myVerbList.verb.Length);
            basetense = myVerbList.verb[randomnum].basetense;
            pasttense = myVerbList.verb[randomnum].pastsimple;
            pastparticiple = myVerbList.verb[randomnum].pastparticiple;
            Debug.Log("Base:" + basetense + " Pass:" + pasttense + " PP:" + pastparticiple);
        }
        return pasttense;
    }

    public string GetPastParticipleCorrect() {
        string correct;
        correct = myVerbList.verb[Random.Range(0, myVerbList.verb.Length)].pastparticiple;
        return correct;
    }
}