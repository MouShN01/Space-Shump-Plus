using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace __Scripts
{
    public class SaveManager : Singleton<SaveManager>
    {
        public float generalVolume;
        public float musicVolume;
        public float effectsVolume;
        
        [System.Serializable]
        class Data
        {
            public List<Player> players;
            public float generalV;
            public float musicV;
            public float effectsV;
        }

        public void SaveData()
        {
            Data data = new Data
            {
                players = PlayersManager.Instance.playersList,
                generalV = generalVolume,
                musicV = musicVolume,
                effectsV = effectsVolume
            };
            
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
                generalVolume = data.generalV;
                musicVolume = data.musicV;
                effectsVolume = data.effectsV;
            }
        }
    }
}
