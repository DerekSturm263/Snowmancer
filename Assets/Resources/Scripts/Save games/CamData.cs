using UnityEngine;

[System.Serializable]
public class CamData
{
    public float[] camPos;

    public CamData(Vector3 position)
    {
        camPos = new float[3];

        camPos[0] = position.x;
        camPos[1] = position.y;
        camPos[2] = position.z;
    }
}
