using System.Collections.Generic;

public class BitData {
    public int Id;
    public BitType Type;
    public List<BitData> Children = new(); // keep same order as Slots (colInit)
    public List<int> slotIds = new();
}