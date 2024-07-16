using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace __Scripts
{
    public class GameOverMenu : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverMenu;
        [SerializeField] private TMP_Text scoreText; 
        void Start()
        {
            gameOverMenu.SetActive(false);
        }

        private void Update()
        {
            if (gameObject.activeInHierarchy)
            {
                scoreText.text = $"Your Score: {Main.S.totalScore.ToString()}";
            }
        }

        public void Restart()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("_Scene_1");
        }

        public void Exit()
        {
            Time.timeScale = 1;
            PlayersManager.Instance.SortPlayersList();
            SaveManager.Instance.SaveData();
            SceneManager.LoadScene("_Scene_0");
        }
    }
}
