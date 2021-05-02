using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraCoinsLevel : MonoBehaviour
{
    public int coinsGain;
    public int quizDuration;
    public string quizName;
    public string quizSubject;

    public void InitializeValue(int coinsGain, int quizDuration, string quizName, string quizSubject)
    {
        this.coinsGain = coinsGain;
        this.quizDuration = quizDuration;
        this.quizName = quizName;
        this.quizSubject = quizSubject;
    }
}
