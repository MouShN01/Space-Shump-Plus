using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using __Scripts;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// ��� ������������ ���� ��������� ����� ������
/// ��� �� �������� ��� "shield" ����� ���� ����������� ���������������� ������
/// </summary>
public enum WeaponType
{
    none,       //�� ���������
    blaster,    //������� �������
    speared,    //������� �����, ���������� ����������� ���������
    phaser,     //�������� �����
    missile,    //��������������� ������
    laser,      //������� ����������� ��� �������������� �����������
    shield      //����������� shieldLevel
}

/// <summary>
/// ����� WeaponDefinition ��������� ����������� ��������
/// ����������� ���� ������ � ����������. ��� ����� ���cc Main
/// ����� ������� ������ ��������� ���� WeaponDefinition
/// </summary>
[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string letter;   //����� � ������ 

    public Color color = Color.white;//���� ������ ������ � ������
    public GameObject projectilePrefab;
    public Color projectileColor = Color.white;
    public float damageOnHit = 0;
    public float countinousDamage = 0;

    public float delayBetweenShots = 0;
    public float velocity = 20; //�������� �������
}
public class Weapon : MonoBehaviour
{
    public static Transform PROJECTILE_ANCHOR;

    [Header("Set Dynamically")]
    [SerializeField]
    private WeaponType _type = WeaponType.none;
    public WeaponDefinition def;
    public GameObject collar;
    public float lastShotTime;
    private Renderer collarRend;

    void Start()
    {
        collar = transform.Find("Collar").gameObject;
        collarRend = collar.GetComponent<Renderer>();

        //������� SetType(), ����� ������� ��� ������ �� ���������
        //WeaponType.none
        SetType(_type);
        //����������� ������� ����� �������� ��� ���� ��������
        if(PROJECTILE_ANCHOR == null)
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }
        //����� fireDelegate � �������� ������� �������
        GameObject rootGO = transform.root.gameObject;
        if (rootGO.GetComponent<Hero>() != null)
        {
            rootGO.GetComponent<Hero>().fireDelegate += Fire;
        }

        if (rootGO.GetComponent<Enemy>() != null)
        {
            rootGO.GetComponent<Enemy>().fireDelegate += Fire;
        }
    }
    
    public WeaponType type
    {
        get => (_type);
        set => SetType(value);
    }
    public void SetType(WeaponType wt)
    {
        _type = wt;
        if(type == WeaponType.none)
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
        }
        def = Main.GetWeaponDefinition(_type);
        collarRend.material.color = def.color;
        lastShotTime = 0;
    }

    public void Fire()
    {
        //���� this.gameObject ���������, �����
        if (!gameObject.activeInHierarchy) return;
        //���� ����� ���������� ������ ���������� ����� �������, �����
        if (Time.time - lastShotTime < def.delayBetweenShots && type != WeaponType.missile)
        {
            return;
        }
        Projectile p;
        Vector3 vel = Vector3.up * def.velocity;
        if(transform.up.y < 0)
        {
            vel.y = -vel.y;
        }
        switch (type)
        {
            case WeaponType.blaster:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                break;
            case WeaponType.speared:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                p = MakeProjectile();
                p.transform.rotation = Quaternion.AngleAxis(10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                p = MakeProjectile();
                p.transform.rotation = Quaternion.AngleAxis(-10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                break;
            case WeaponType.phaser:
                p = MakeProjectile();
                StartCoroutine(SinusoidalMotion(p, Vector3.left));
                p.rigid.velocity = vel; 
                p = MakeProjectile();
                StartCoroutine(SinusoidalMotion(p, Vector3.right));
                p.rigid.velocity = vel;
                break;
            case WeaponType.missile:
                p = MakeProjectile();
                StartCoroutine(MoveToEnemy(p, NearestEnemy()));
                break;
            case WeaponType.laser:
                p = MakeProjectile();
                Vector3 scaleFactor = new Vector3(1, 100, 1);
                p.transform.localScale = Vector3.Scale(p.transform.localScale, scaleFactor);
                p.transform.position += new Vector3(0, 50, 0);
                break;
        }
    }
    
    IEnumerator SinusoidalMotion(Projectile projectile, Vector3 traectory)
    {
        Rigidbody rigid = projectile.rigid;
        
        while (projectile != null)
        {
            float yOffset = Mathf.Sin(Time.time * 10) * 0.5f;
            Vector3 newPosition = projectile.transform.position + traectory * yOffset;
            
            rigid.MovePosition(newPosition);
            
            Vector3 direction = newPosition - projectile.transform.position;

            projectile.transform.Rotate(0, 0, direction.x);

            yield return null;
        }
    }

    IEnumerator MoveToEnemy(Projectile projectile, GameObject nearestEnemy)
    {
        Rigidbody rigid = projectile.rigid;

        while (projectile != null && nearestEnemy != null)
        {
            Vector3 direction = nearestEnemy.transform.position - projectile.transform.position;
            direction.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            
            projectile.transform.rotation = targetRotation;
            
            projectile.transform.Rotate(-90, 0, 0);
            
            // projectile.transform.rotation = Quaternion.RotateTowards(projectile.transform.rotation, targetRotation, Time.deltaTime * 1000);

            rigid.velocity = -projectile.transform.up * def.velocity;
            

            yield return null;
        }
    }
    
   public GameObject NearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;
        Vector3 playerPos = Hero.S.transform.position;
        foreach (var enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, playerPos);
            if (distance < nearestDistance)
            {
                nearestEnemy = enemy;
                nearestDistance = distance;
            }
        }

        return nearestEnemy;
    }

    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePrefab);
        if(transform.parent.gameObject.tag == "Hero")
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }
        go.transform.position = collar.transform.position;
        go.transform.SetParent(PROJECTILE_ANCHOR, true);
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShotTime = Time.time;
        return (p);
    }
}
