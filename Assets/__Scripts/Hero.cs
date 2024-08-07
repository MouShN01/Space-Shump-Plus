using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using __Scripts;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S;

    [Header("Set in Inspector")]
    //����, ����������� ��������� �������
    public float speed = 30;

    public float rollMult = -45;
    public float pitchMult = 30;
    public float gameRestartDelay = 2f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;
    public Weapon[] weapons;
    
    public AudioSource src;
    public AudioClip shot;

    [Header("Set Dynamically")] [SerializeField]
    private float _shieldLevel = 1;

    private int _misslesCount = 1;

    private bool isPlayingSound = false;

    //���������� �������� ������ �� ��������� ������ �������������
    private GameObject lastTriggerGO = null;

    //���������� ������ �������� ���� WeaponFireDelegate
    public delegate void WeaponFireDelegate();

    //������� ���� ���� WeaponFireDelegate � ������ fireDelegate.
    public WeaponFireDelegate fireDelegate;

    void Start()
    {
        if (S == null)
        {
            S = this;
        }
        else
        {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");
        }
        //fireDelegate += TempFire;

        ClearWeapons();
        weapons[0].SetType(WeaponType.blaster);
    }

    void Update()
    {
        //���������� ���������� �� ������ Input
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        //��������� transform.position �������� �� ���������� �� ����
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        //������� �������, ��� �� ������� �������� ���������
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

        //��������� ������� ����������
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    TempFire();
        //}

        //���������� ������� �� ���� ����� ������ ������� fireDelegate
        //������� ��������� ������� �������: Axis("Jump")
        //����� ���������, ��� �������� fireDelegate �� ����� null,
        // ����� �������� ������
        if (Input.GetButton("Jump") && fireDelegate != null)
        {
            if(!isPlayingSound) StartCoroutine(PlayShootSound());
            fireDelegate();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (misslesCount > 0)
            {
                var weaponType = weapons[0].type;
                int weaponCount = 0;
                foreach (var w in weapons)
                {
                    if (w.type != WeaponType.none)
                    {
                        weaponCount += 1;
                    }
                }
                ClearWeapons();
                weapons[0].SetType(WeaponType.missile);
                fireDelegate();
                misslesCount--;
                ClearWeapons();
                for (int i = 0; i < weaponCount; i++)
                {
                    weapons[i].SetType(weaponType);
                }
            }
        }
    }

   public IEnumerator PlayShootSound()
    {
        isPlayingSound = true;
        
        src.PlayOneShot(shot);

        yield return new WaitForSeconds(weapons[0].def.delayBetweenShots);

        isPlayingSound = false;
    }

void TempFire()
    {
        GameObject projGO = Instantiate<GameObject>(projectilePrefab);
        projGO.transform.position = transform.position;
        Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
        //rigidB.velocity = Vector3.up * projectileSpeed;

        Projectile proj = projGO.GetComponent<Projectile>();
        proj.type = WeaponType.blaster;
        float tSpeed = Main.GetWeaponDefinition(proj.type).velocity;
        rigidB.velocity = Vector3.up * tSpeed;
    }

    private void OnCollisionEnter(Collision other)
    {
        GameObject otherGO = other.gameObject;
        if (otherGO.CompareTag("ProjectileEnemy"))
        {
            shieldLevel--;
            Destroy(otherGO); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        //print("Triggered: " + go.name);

        //�������� ������������� ���������� ������������ � ��� �� ��������
        if (go == lastTriggerGO) return;
        lastTriggerGO = go;

        if(go.CompareTag("Enemy"))
        {
            shieldLevel--;
            Destroy(go);
        }
        else if(go.CompareTag("PowerUp"))
        {
            AbsorbPowerUp(go);
        }
        else 
        {
            print("Triggered by non-Enemy: " + go.name);
        }
    }

    public void AbsorbPowerUp(GameObject go)
    {
        PowerUp pu = go.GetComponent<PowerUp>();
        switch (pu.type)
        {
            case WeaponType.shield:
                shieldLevel++;
                break;
            case WeaponType.missile:
                misslesCount++;
                break;
            default:
                if (pu.type == weapons[0].type)
                {
                    Weapon w = GetEmptyWeaponSlot();
                    if (w != null)
                    {
                        w.SetType(pu.type);
                    }
                }
                else
                {
                    ClearWeapons();
                    weapons[0].SetType(pu.type);
                }
                break;  
        }
        pu.AbsorbedBy(this.gameObject);
    }

    public float shieldLevel
    {
        get { return (_shieldLevel); }
        set { 
            _shieldLevel = Mathf.Min(value, 4);
            if (value < 0)
            {
                Destroy(this.gameObject);
                //��������� ������� Main.S � ������������� ����������� ����
                Main.S.DelayedRestart(gameRestartDelay);
            }
        }   
    }

    public int misslesCount
    {
        get { return (_misslesCount); }
        set { _misslesCount = value; }
    }

    Weapon GetEmptyWeaponSlot()
    {
        for(int i = 0; i<weapons.Length; i++)
        {
            if(weapons[i].type == WeaponType.none)
            {
                return (weapons[i]);
            }
        }
        return (null);
    }
    void ClearWeapons()
    {
        foreach(Weapon w in weapons)
        {
            w.SetType(WeaponType.none);
        }
    }
}
