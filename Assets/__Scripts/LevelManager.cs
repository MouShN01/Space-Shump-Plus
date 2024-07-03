using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager S;

    public int level = 1;
    public int scoreToNextLvl = 0;

    private void Awake()
    {
        S = this;
    }

    void Update()
    {
        int nextScoreToNextLvl = scoreToNextLvl + 1000;
        if (Main.S.totalScore >= nextScoreToNextLvl)
        {
            level++;
            scoreToNextLvl = nextScoreToNextLvl;
        }

    }
}
