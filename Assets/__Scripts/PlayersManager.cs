using System.Collections.Generic;
using System.Linq;

namespace __Scripts
{
    public class PlayersManager : Singleton<PlayersManager>
    {
        public List<Player> playersList;
        public Player currentPlayer;

        private void Start()
        {
            playersList ??= new List<Player>();
        }

        public void CreatePlayer(string name)
        {
            currentPlayer = playersList.FirstOrDefault(player => player.name == name);
            if (currentPlayer == null)
            {
                currentPlayer = new Player
                {
                    name = name,
                    score = 0
                };
                playersList.Add(currentPlayer);
            }
        }

        public void SortPlayersList()
        {
            if (playersList != null && playersList.Any())
            {
                playersList = playersList.OrderByDescending(player => player.score).ToList();
            }
        }
    }
}
