using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VocabAnimals
{
    public int correctCount;
    public string remainTime;
    public int answerTotal;

    public VocabAnimals()
    {

    }

    public VocabAnimals(int correctCount, string remainTime, int answerTotal)
    {
        this.correctCount = correctCount;
        this.remainTime = remainTime;
        this.answerTotal = answerTotal;
    }
}
