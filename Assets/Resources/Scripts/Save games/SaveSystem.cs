using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static string PlayerPath = Application.persistentDataPath + "/player.savedata";
    public static FileStream playerStream;

    public static string CameraPath = Application.persistentDataPath + "/camera.savedata";
    public static FileStream cameraStream;

    public static string ScenePath = Application.persistentDataPath + "/scene.savedata";
    public static FileStream sceneStream;


    public static void SavePlayer(Player SaveLoad)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        playerStream = new FileStream(PlayerPath, FileMode.Create);

        PlayerData data = new PlayerData(SaveLoad);

        formatter.Serialize(playerStream, data);
        playerStream.Close();
    }
    public static void SaveScene(string scene)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        sceneStream = new FileStream(ScenePath, FileMode.Create);

        LevelData data = new LevelData(scene);

        formatter.Serialize(sceneStream, data);
        sceneStream.Close();
    }

    public static void SaveCamera(Vector3 pos)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        cameraStream = new FileStream(CameraPath, FileMode.Create);
    
        CamData data = new CamData(pos);
    
        formatter.Serialize(cameraStream, data);
        cameraStream.Close();
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

    public static Vector3 LoadCamera()
    {
        if (File.Exists(CameraPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(CameraPath, FileMode.Open);

            CamData data = formatter.Deserialize(stream) as CamData;
            stream.Close();

            Vector3 newPos = new Vector3();
            newPos.x = data.camPos[0];
            newPos.y = data.camPos[1];
            newPos.z = data.camPos[2];

            return newPos;
        }
        else
        {
            Debug.LogError("Save file not found in " + CameraPath);
            return Vector3.zero;
        }
    }

    public static string LoadScene()
    {
        if (File.Exists(ScenePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(ScenePath, FileMode.Open);

            LevelData data = formatter.Deserialize(stream) as LevelData;
            stream.Close();

            return data.scene;
        }
        else
        {
            Debug.LogError("Save file not found in " + PlayerPath);
            return null;
        }
    }
}