using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIHandler : MonoBehaviour
{
    public TMP_Text score;
    // Update is called once per frame
    void Update()
    {
        score.text = "Score: " + Main.S.totalScore.ToString();
    }
}
