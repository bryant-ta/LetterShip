using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship")]
public class SO_Ship : ScriptableObject {
    public BitData Core;
}

public class BitData {
    public int Id;
    public BitType Type;
    public List<BitData> Children = new(); // keep same order as Slots (colInit)
    public List<int> slotIds = new();
}