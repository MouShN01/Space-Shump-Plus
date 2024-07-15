using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace __Scripts
{
    public class SaveManager : Singleton<SaveManager>
    {
        class Data
        {
            public List<Player> players;
        }
        
        public void SaveData()
        {
            Data data = new Data();
            data.players = PlayersManager.Instance.playersList;
            
            string json = JsonUtility.ToJson(data);

            File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        }

        public void LoadData() 
        {
            string path = Application.persistentDataPath + "/savefile.json";
            if(File.Exists(path))
            {
                string json = File.ReadAllText(path);

                Data data = JsonUtility.FromJson<Data>(json);

                PlayersManager.Instance.playersList = data.players;
            }
        }
    }
}
