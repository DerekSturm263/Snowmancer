[System.Serializable]
public class BossData
{
    public bool isFireAlive;
    public bool isElectricAlive;
    public bool isWindAlive;
    public bool isFinalSpawned;
    public bool isFinalAlive;

    public BossData(bool fire, bool electric, bool wind, bool finalSpawn, bool final)
    {
        isFireAlive = fire;
        isElectricAlive = electric;
        isWindAlive = wind;
        isFinalSpawned = finalSpawn;
        isFinalAlive = final;
    }
}
