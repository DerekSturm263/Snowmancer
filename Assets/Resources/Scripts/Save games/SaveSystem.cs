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

    public static string elementDataPath = Application.persistentDataPath + "/element.savedata";
    public static FileStream elementStream;

    public static string settingsDataPath = Application.persistentDataPath + "/settings.savedata";
    public static FileStream settingsStream;

    public static string bossDataPath = Application.persistentDataPath + "/boss.savedata";
    public static FileStream bossStream;

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

    public static void SaveElementData(bool[] unlocks)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        elementStream = new FileStream(elementDataPath, FileMode.Create);

        ElementData data = new ElementData(unlocks);

        formatter.Serialize(elementStream, data);
        elementStream.Close();
    }

    public static void SaveSettingsData(bool[] boolValues, float musicVal)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        settingsStream = new FileStream(settingsDataPath, FileMode.Create);

        SettingsData data = new SettingsData(boolValues, musicVal);

        formatter.Serialize(settingsStream, data);
        settingsStream.Close();
    }

    public static void SaveBossData(bool fire, bool electric, bool wind, bool finalSpawn, bool final)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        bossStream = new FileStream(bossDataPath, FileMode.Create);

        BossData data = new BossData(fire, electric, wind, finalSpawn, final);

        formatter.Serialize(bossStream, data);
        bossStream.Close();
    }

    public static bool[] SettingsBools()
    {
        if (File.Exists(settingsDataPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(settingsDataPath, FileMode.Open);

            SettingsData data = formatter.Deserialize(stream) as SettingsData;
            stream.Close();

            bool[] boolValues = new bool[5];

            boolValues[0] = data.isFullscreen;
            boolValues[1] = data.useParticles;
            boolValues[2] = data.usePostProcessing;
            boolValues[3] = data.hasAntiAliasing;
            boolValues[4] = data.useHints;

            return boolValues;
        }
        else
        {
            Debug.LogError("Save file not found in " + PlayerPath);
            return null;
        }
    }

    public static float SettingsFloats()
    {
        if (File.Exists(settingsDataPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(settingsDataPath, FileMode.Open);

            SettingsData data = formatter.Deserialize(stream) as SettingsData;
            stream.Close();

            return data.musicVolume;
        }
        else
        {
            Debug.LogError("Save file not found in " + PlayerPath);
            return 0f;
        }
    }

    public static bool[] LoadElementData()
    {
        if (File.Exists(elementDataPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(elementDataPath, FileMode.Open);

            ElementData data = formatter.Deserialize(stream) as ElementData;
            stream.Close();

            bool[] newUnlocks = data.isLocked;
            return newUnlocks;
        }
        else
        {
            Debug.LogError("Save file not found in " + PlayerPath);
            return null;
        }
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

    public static BossData LoadBoss()
    {
        if (File.Exists(bossDataPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(bossDataPath, FileMode.Open);

            BossData data = formatter.Deserialize(stream) as BossData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + bossDataPath);
            return null;
        }
    }

    public static void DeleteSaveData()
    {
        try
        {
            File.Delete(PlayerPath);
            File.Delete(CameraPath);
            File.Delete(ScenePath);
            File.Delete(elementDataPath);
            File.Delete(settingsDataPath);
            File.Delete(bossDataPath);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }
}