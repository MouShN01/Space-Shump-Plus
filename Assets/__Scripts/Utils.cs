using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    //===================================== Функция для работы с материалами =======================================
    //Возварщает список всех материалов в данном игровом обьекте
    // и его дочерних обьектоах
    static public Material[] GetAllMaterials(GameObject go)
    {
        Renderer[] rends = go.GetComponentsInChildren<Renderer>();

        List<Material> mats = new List<Material>();
        foreach (Renderer rend in rends)
        {
            mats.Add(rend.material);
        }
        return (mats.ToArray());
    }
}
