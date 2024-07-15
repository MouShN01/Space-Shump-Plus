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
            playerHighScore.text = $"Your high score - {PlayersManager.Instance.currentPlayer.Score.ToString()}";

        }

        public void Exit()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
        }
    }
}
