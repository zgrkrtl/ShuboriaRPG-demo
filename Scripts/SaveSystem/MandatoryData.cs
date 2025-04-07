using System.IO;
using UnityEngine;

[System.Serializable] 
public class SerializableVector3
{
    public float x, y, z;
    public SerializableVector3(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }
    public Vector3 ToVector3() => new Vector3(x, y, z);
}

[System.Serializable] 
public class MandatoryData
{
    public int abilityPoints;
    public float experiencePoints;
    public int level;
    public SerializableVector3 position; 
    
    public MandatoryData()
    {
        abilityPoints = 0;
        experiencePoints = 0f;
        level = 1;
        position = new SerializableVector3(Vector3.zero); 
    }
}
