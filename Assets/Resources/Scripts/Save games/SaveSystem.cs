using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static string PlayerPath = Application.persistentDataPath + "/player.savedata";
    public static string EnemyPath = Application.persistentDataPath + "/enemy.savedata";
    public static FileStream enemyStream;
    public static FileStream playerStream;


    public static void SavePlayer(Player SaveLoad)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        playerStream = new FileStream(PlayerPath, FileMode.Create);

        enemyStream = new FileStream(EnemyPath, FileMode.Create);

        PlayerData data = new PlayerData(SaveLoad);

        formatter.Serialize(playerStream, data);
        playerStream.Close();
    }

    public static void SaveEnemy(Enemy SaveLoad)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        enemyStream = new FileStream(EnemyPath, FileMode.Append);

        EnemyData data = new EnemyData(SaveLoad);

        formatter.Serialize(enemyStream, data);
        enemyStream.Close();
    }

    public static PlayerData LoadPLayer()
    {
        if (File.Exists(PlayerPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(PlayerPath, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + PlayerPath);
            return null;
        }
    }

    public static EnemyData LoadEnemy()
    {
        if (File.Exists(EnemyPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(EnemyPath, FileMode.Open);



            EnemyData data = formatter.Deserialize(stream) as EnemyData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + EnemyPath);
            return null;
        }
    }
}

