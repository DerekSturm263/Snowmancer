[System.Serializable]
public class ElementData
{
    public bool[] isLocked;

    public ElementData(bool[] lockedArray)
    {
        isLocked = new bool[5];

        isLocked[0] = lockedArray[0];
        isLocked[1] = lockedArray[1];
        isLocked[2] = lockedArray[2];
        isLocked[3] = lockedArray[3];
        isLocked[4] = lockedArray[4];
    }
}
