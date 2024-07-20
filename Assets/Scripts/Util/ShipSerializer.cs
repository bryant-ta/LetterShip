using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ShipSerializer {
    const string SaveFilePath = "Assets/Ships/";

    public void SaveShip() {
        BitData coreBitData = SaveBit(Ref.Player.Ship.Core);

        string json = JsonUtility.ToJson(coreBitData, true);
        File.WriteAllText(SaveFilePath, json);
        Debug.Log("Ship configuration saved to JSON at " + SaveFilePath);
    }

    public BitData SaveBit(Bit bit) {
        BitData bitData = new BitData {
            Id = bit.Id,
            Type = bit.Type,
            Children = new()
        };

        foreach (KeyValuePair<int, Bit> slot in bit.Slots) {
            if (slot.Value != null) {
                BitData childData = SaveBit(slot.Value);
                bitData.Children.Add(childData);
                bitData.slotIds.Add(slot.Key);
            } else {
                bitData.Children.Add(null); // Maintain the order of slots
                bitData.slotIds.Add(slot.Key);
            }
        }

        return bitData;
    }
    
    public BitData LoadBit(string fileName) {
        if (File.Exists(SaveFilePath + fileName + ".json")) {
            string json = File.ReadAllText(SaveFilePath);
            BitData bitData = JsonUtility.FromJson<BitData>(json);
            return bitData;
        } else {
            Debug.LogError("No saved ship configuration found.");
            return null;
        }
    }
}
