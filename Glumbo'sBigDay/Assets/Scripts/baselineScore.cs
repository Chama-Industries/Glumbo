using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baselineScore 
{
    private static int currentScore = 0;
    public int getScore()
    {
        return currentScore;
    }
    public void addScore(int inScore)
    {
        currentScore = currentScore + inScore;
    }
}
