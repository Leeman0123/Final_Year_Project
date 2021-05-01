using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuizDetails
{
    public int quizDuration;
    public string quizName;
    public string quizSubject;
    public int coinsGain;

    public QuizDetails(string quizName, string quizSubject, int quizDuration, int coinsGain)
    {
        this.quizDuration = quizDuration;
        this.quizSubject = quizSubject;
        this.quizName = quizName;
        this.coinsGain = coinsGain;
    }
}
