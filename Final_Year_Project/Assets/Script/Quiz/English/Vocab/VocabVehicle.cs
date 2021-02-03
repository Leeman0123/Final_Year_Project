using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VocabVehicle
{
    public int correctCount;
    public string remainTime;
    public int answerTotal;

    public VocabVehicle()
    {

    }

    public VocabVehicle(int correctCount, string remainTime, int answerTotal)
    {
        this.correctCount = correctCount;
        this.remainTime = remainTime;
        this.answerTotal = answerTotal;
    }
}
