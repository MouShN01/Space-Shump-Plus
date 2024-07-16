using System.Collections;
using System.Collections.Generic;
using __Scripts;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Set In Inspector")]
    //Ќеобычное, но удобное применение Vector2.x хранит минимальное
    // значение, а y - максимальное значение дл€ метода Random.Range(),
    // который будет вызыватьс€ позже
    public Vector2 rotMinMax = new Vector2(15, 90);
    public Vector2 driftMinMax = new Vector2(.25f, 2);
    public float lifeTime = 6f;
    public float fadeTime = 4f;

    [Header("Set Dynamicaly")]
    public WeaponType type;
    public GameObject cube;
    public TextMesh letter;
    public Vector3 rotPerSecond;
    public float birthTime;

    private Rigidbody rigid;
    private BoundsCheck bndCheck;
    private Renderer cubeRend;

    void Awake()
    {
        //получение ссылки на куб
        cube = transform.Find("Cube").gameObject;
        //получение ссылки на TextMesh и другие компоненты
        letter = GetComponent<TextMesh>();
        rigid = GetComponent<Rigidbody>();
        bndCheck = GetComponent<BoundsCheck>();
        cubeRend = cube.GetComponent<Renderer>();

        //¬ыбор случайной скорости
        Vector3 vel = Random.onUnitSphere;
        vel.z = 0;
        vel.Normalize();
        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        rigid.velocity = vel;

        transform.rotation = Quaternion.identity;

        rotPerSecond = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y),
            Random.Range(rotMinMax.x, rotMinMax.y),
            Random.Range(rotMinMax.x, rotMinMax.y));

        birthTime = Time.time;
    }

    void Update()
    {
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);

        //–астворение куба
        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;

        if(u >= 1)
        {
            Destroy(this.gameObject);
            return;
        }

        if(u>0)
        {
            Color c = cubeRend.material.color;
            c.a = 1f-u;
            cubeRend.material.color = c;
            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }

        if(!bndCheck.isOnScreen)
        {
            Destroy(this.gameObject);
        }
    }
    public void SetType(WeaponType wt)
    {
        WeaponDefinition def = Main.GetWeaponDefinition(wt);
        cubeRend.material.color = def.color;
        letter.text = def.letter;
        type = wt;
    }

    public void AbsorbedBy(GameObject target)
    {
        Destroy(this.gameObject);
    }
}
