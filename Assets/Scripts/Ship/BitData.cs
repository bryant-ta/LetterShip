using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BitData {
    public int Id;
    public BitType Type;
    public List<BitData> Children = new(); // keep same order as Slots (colInit)
    public List<int> SlotIds = new();

    public JsonVector3 Position;

    public bool RootPlaceholder;
}

public struct JsonVector3 {
    public float x;
    public float y;
    public float z;
    
    public JsonVector3(Vector3 pos) {
        x = pos.x;
        y = pos.y;
        z = pos.z;
    }
}