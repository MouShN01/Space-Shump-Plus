using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ������������� ����� �������� ������� �� ������� ������.
/// ����: �������� ������ � ��������������� ���������� ������
/// </summary>
public class BoundsCheck : MonoBehaviour
{

    [Header("Set in Inspector")]
    public float radius = 0.1f;
    public bool keepOnScreen = true;

    [Header("Set Dynamically")]
    public bool isOnScreen = true;
    public float camWidth;
    public float camHeight;

    [HideInInspector]
    public bool offRight, offLeft, offTop, offDown;

    void Awake()
    {
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        isOnScreen = true;
        offRight = offLeft = offTop = offDown = false;

        if(pos.x > camWidth - radius)
        {
            pos.x = camWidth - radius;
            offRight = true;
        }    
        if(pos.x < -camWidth + radius)
        {
            pos.x = -camWidth + radius;
            offLeft = true;
        }
        if(pos.y > camHeight -radius)
        {
            pos.y = camHeight - radius;
            offTop = true;
        }
        if(pos.y < -camHeight + radius)
        {
            pos.y = -camHeight + radius;
            offDown = true;
        }
        isOnScreen = !(offRight || offLeft || offTop || offDown);
        if (keepOnScreen && !isOnScreen)
        {
            transform.position = pos;
            isOnScreen = true;
            offRight = offLeft = offTop = offDown = false;
        }
    }


    //������ ������� � ������ �����
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Vector3 boundSize = new Vector3(camWidth * 2, camHeight * 2, 0.1f);
        Gizmos.DrawWireCube(Vector3.zero, boundSize);
    }
}
