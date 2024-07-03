using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector: Enemy")]
    public float speed = 10f;
    public float fireRate = 6f;
    public float health = 10;
    public int score = 100;
    public float showDamageDuration = 0.1f;
    public delegate void WeaponFireDelegate();
    public WeaponFireDelegate fireDelegate;
    public Weapon weapon;
    public WeaponType weaponType;
    
    public float powerUpDropChance = 1f;

    public GameObject particles;
    public GameObject boomSound;

    [Header("Set Dynamically: Enemy")]
    public Color[] originalColors;
    public Material[] materials;
    public bool showingDamage = false;
    public float damageDoneTime;
    public bool notifiedOfDestruction = false;

    protected BoundsCheck bndCheck;

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        //Получить материалы и цвет этого игрового обьекта и его потомков
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for(int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }
    }

   private void Start()
    {
        if (LevelManager.S.level > 1)
        {
            weapon.SetType(weaponType);
            InvokeRepeating("Fire", 0, fireRate/LevelManager.S.level);
        }
    }

    public void Fire()
    {
        fireDelegate();
    }

    public Vector3 pos
    {
        get => (this.transform.position);
        set => this.transform.position = value;
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if(showingDamage && Time.time > damageDoneTime)
        {
            UnShowDamage();
        }

        if(bndCheck != null && bndCheck.offDown)
        {
            if(pos.y < bndCheck.camHeight-bndCheck.radius)
            {
                Destroy(gameObject);
            }
        }
    }

    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    private void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;
        switch(otherGO.tag)
        {
            case "ProjectileHero":
                Projectile p = otherGO.GetComponent<Projectile>();

                //Если вражеский корабль за нраницами экрана,
                // не наность ему повреждений
                if(!bndCheck.isOnScreen)
                {
                    Destroy(otherGO);
                    break;
                }

                //Поразить вражеский корабль
                ShowDamage();
                //Получить разрушающую силу из WEAP_DICT в классе Main.
                health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                if(health <= 0)
                {
                    if (!notifiedOfDestruction)
                    {
                        Main.S.ShipDestroyed(this);
                    }
                    notifiedOfDestruction = true;
                    Destroy(this.gameObject);
                    GameObject cloneBoom = (GameObject)Instantiate(boomSound, transform.position, Quaternion.identity);
                    Destroy(cloneBoom, 2.0f);
                    GameObject cloneParticle= (GameObject)Instantiate(particles, transform.position, Quaternion.identity);
                    Destroy(cloneParticle, 1.0f);
                }
                Destroy(otherGO);
                break;

            default:
                print("Enemy hit by non-ProjectileHero: " + otherGO.name);
                break;
        }
    }

    void ShowDamage()
    {
        foreach(Material m in materials)
        {
            m.color = Color.red;
        }
        showingDamage = true;
        damageDoneTime = Time.time + showDamageDuration;
    }

    void UnShowDamage()
    {
        for(int i = 0; i<materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
        showingDamage = false;
    }
}
