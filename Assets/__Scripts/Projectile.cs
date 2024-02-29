using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private BoundsCheck bndCheck;
    private Renderer rend;

    [Header("Set Dynamically")]
    public Rigidbody rigid;
    [SerializeField]
    private WeaponType _type;

    // ��� ������������� �������� ��������� ���� _type � ������������
    // �������� ������������ ��� ������ ��������

    public WeaponType type
    {
        get { return (_type); }
        set { SetType(value); }
    }
    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        if(bndCheck.offTop)
        {
            Destroy(gameObject);
        }
    }

    ///<summary>
    ///�������� ������� ���� _type � ������������� ���� ����� �������,
    ///��� ���������� � WeaponDefinition
    ///</summary>
    ///<param name="eType">��� WeaponType ������������ ������.</param>
    public void SetType(WeaponType eType)
    {
        // ���������� _type
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        rend.material.color = def.projectileColor;
    }
}
