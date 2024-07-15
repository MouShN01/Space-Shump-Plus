using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
            currentPlayer = playersList.FirstOrDefault(player => player.Name == name);
            if (currentPlayer == null)
            {
                currentPlayer = new Player
                {
                    Name = name,
                    Score = 0
                };
                playersList.Add(currentPlayer);
            }
        }
    }
}
