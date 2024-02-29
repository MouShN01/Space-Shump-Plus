using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float rotationPerSecond = 0.1f;

    [Header("Set Dynamically")]
    public int levelShown = 0;

    Material mat;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        //Получение текущего уровня щита
        int currLevel = Mathf.FloorToInt(Hero.S.shieldLevel);
        
        if(levelShown != currLevel)
        {
            levelShown = currLevel;
            mat.mainTextureOffset = new Vector3(0.2f * levelShown, 0);
        }

        //Анимация щита
        float rZ = -(rotationPerSecond * Time.time * 360) % 360;
        transform.rotation = Quaternion.Euler(0, 0, rZ);
    }
}
