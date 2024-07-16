using UnityEngine;

namespace __Scripts
{
    public class Leaderboard : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject leaderboard;
        
        public void BackButton()
        {
            mainMenu.SetActive(true);
            leaderboard.SetActive(false);
        }
    }
}
