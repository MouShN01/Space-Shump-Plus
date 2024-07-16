using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace __Scripts
{
    public class MenuUIHandler : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject loginMenu;
        [SerializeField] private GameObject leaderboard;
        [SerializeField] private GameObject volMenu;
        [SerializeField] private TMP_Text leaders;
        [SerializeField] private TMP_InputField nameField;
        [SerializeField] private TMP_Text playerHighScore;

        public void NewStart()
        {
            SceneManager.LoadScene("_Scene_1");
        }

        public void ToMenu()
        {
            loginMenu.SetActive(false);
            mainMenu.SetActive(true);
            PlayersManager.Instance.CreatePlayer(nameField.text);
            playerHighScore.text = $"Your high score - {PlayersManager.Instance.currentPlayer.score.ToString()}";

        }

        public void ShowLeaderboard()
        {
            mainMenu.SetActive(false);
            leaderboard.SetActive(true);

            var tenValues = PlayersManager.Instance.playersList.Take(5).ToList();
            string leadersLabel = "";
            for (int i = 0; i < tenValues.Count(); i++)
            {
                leadersLabel +=
                    $"\n {i+1}. {PlayersManager.Instance.playersList[i].name} - {PlayersManager.Instance.playersList[i].score}";
            }

            leaders.text = leadersLabel;
        }

        public void ShowVolumeSettings()
        {
            volMenu.SetActive(true);
            mainMenu.SetActive(false);
        }

        public void Exit()
        {
            SaveManager.Instance.SaveData();
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
        }
    }
}
