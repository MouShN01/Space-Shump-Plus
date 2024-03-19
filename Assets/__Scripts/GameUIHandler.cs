using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameUIHandler : MonoBehaviour
{
    public TMP_Text score;

    public TMP_Text weapType;

    public TMP_Text shieldLevel;
    // Update is called once per frame
    void Update()
    {
        score.text = "Score: " + Main.S.totalScore.ToString();
        weapType.text = "Weapon type: " + Hero.S.weapons[0].type;
        shieldLevel.text = "Shield level: " + Hero.S.shieldLevel;
    }
}
