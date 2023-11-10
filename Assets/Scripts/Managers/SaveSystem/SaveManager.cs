using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Managers.Player;

namespace Managers.SaveSystem
{
    public class SaveManager : MonoBehaviour
    {
        public static void SavePlayerData(PlayerManager player)
        {

            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + "/player.txt";
            FileStream stream = new FileStream(path, FileMode.Create);

            PlayerData playersData = new PlayerData(player);
            formatter.Serialize(stream, playersData);
            stream.Close();

        }

        public static PlayerData LoadPlayerData()
        {

            string path = Application.persistentDataPath + "/player.txt";

            if (File.Exists(path))
            {

                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                stream.Close();

                Debug.Log("data loaded");
                return data;
            }
            else
            {
                Debug.Log("Major error : save file not found in " + path);

                return null;
            }
        }
    }

}
