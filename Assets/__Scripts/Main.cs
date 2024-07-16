using System.Collections.Generic;
using UnityEngine;

namespace __Scripts
{
    public class Main : MonoBehaviour
    {
        public static Main S;
        static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

        [Header("Set in Inspector")]
        public GameObject[] prefabEnemies;
        public float enemySpawnerPerSecond = 0.5f;

        public float enemyDefaultPadding = 1.5f;

        public WeaponDefinition[] weaponDefinitions;
        public GameObject prefabPowerUp;
        public WeaponType[] powerUpFrequency = new WeaponType[]
        {
            WeaponType.blaster, WeaponType.blaster, WeaponType.speared, WeaponType.shield
        };

        private BoundsCheck bndCheck;

        public GameObject gameOverMenu;
    
        public int totalScore = 0;

        public void ShipDestroyed(Enemy e)
        {
            if(Random.value <= e.powerUpDropChance)
            {
                int ndx = Random.Range(0, powerUpFrequency.Length);
                WeaponType puType = powerUpFrequency[ndx];

                GameObject go = Instantiate(prefabPowerUp) as GameObject;
                PowerUp pu = go.GetComponent<PowerUp>();
                pu.SetType(puType);

                pu.transform.position = e.transform.position; 
            }

            totalScore += e.score;
        }
        private void Awake()
        {
            S = this;
            bndCheck = GetComponent<BoundsCheck>();
            Invoke("SpawnEnemy", 1f / enemySpawnerPerSecond);

            //Слварь с ключами типа WeaponType
            WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
            foreach(WeaponDefinition def in weaponDefinitions)
            {
                WEAP_DICT[def.type] = def;
            }
        }

        public void SpawnEnemy()
        {
            //Выбор случайного шаблона
            int idx = Random.Range(0, prefabEnemies.Length);
            GameObject go = Instantiate<GameObject>(prefabEnemies[idx]);

            //Помещение врга в случайной точке над игроком
            float enemyPadding = enemyDefaultPadding;
            if (go.GetComponent<BoundsCheck>() != null)
            {
                enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
            }

            //Установление начальных координат созданого корабля
            Vector3 pos = Vector3.zero;
            float xMin = -bndCheck.camWidth + enemyPadding;
            float xMax = bndCheck.camWidth - enemyPadding;
            pos.x = Random.Range(xMin, xMax);
            pos.y = bndCheck.camHeight + enemyPadding;
            go.transform.position = pos;

            Invoke(nameof(SpawnEnemy), 1f / enemySpawnerPerSecond);
        }
        public void DelayedRestart(float delay)
        {
            Invoke(nameof(GameOver), delay);
        }

        public void GameOver()
        {
            gameOverMenu = GameObject.Find("Canvas").transform.Find("GameOverMenu").gameObject;
            if(totalScore > PlayersManager.Instance.currentPlayer.score)
                PlayersManager.Instance.currentPlayer.score = totalScore;
            gameOverMenu.SetActive(true);
            Time.timeScale = 0;
        }

        ///<summary>
        ///Статическая функция, возвращающая WeaponDefinition из статического
        /// защищоного поля WEAP_DICT класса Main.
        ///</summary>
        ///<returns>Экземпляр WeaponDefinition или, если нет такого определения 
        /// для указаного WeaponType, возвращает новый экземпляр WeaponDefinition
        /// с типом none.</returns>
        /// <param name="wt">Тип оружия WeaponType, для которого требуется получить 
        ///  WeaponDefinition </param>
        static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
        {
            if (WEAP_DICT.ContainsKey(wt))
            {
                return (WEAP_DICT[wt]);
            }
            return (new WeaponDefinition());
        }
    }
}
