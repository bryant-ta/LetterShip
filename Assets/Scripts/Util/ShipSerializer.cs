using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ShipSerializer {
    public void SaveShip(string input) {
        input = Clean(input);
        BitData coreBitData = SaveBit(Ref.Player.Ship.Core);

        string json = JsonConvert.SerializeObject(coreBitData, Formatting.Indented);

        string saveFilePath = Path.Combine(Application.persistentDataPath, input);
        saveFilePath = Path.ChangeExtension(saveFilePath, ".json");

        string directoryPath = Path.GetDirectoryName(saveFilePath);
        if (!Directory.Exists(directoryPath)) {
            Directory.CreateDirectory(directoryPath);
        }

        File.WriteAllText(saveFilePath, json);
        Debug.Log("Ship saved at " + saveFilePath);
    }

    BitData SaveBit(Bit bit) {
        BitData bitData = new BitData {
            Id = bit.Id,
            Type = bit.Type,
            Children = new(),
            SlotIds = new(),
            Position = new JsonVector3(bit.transform.localPosition)
        };

        foreach (KeyValuePair<int, Bit> slot in bit.Slots) {
            if (slot.Value != null) {
                if (slot.Value == bit.Root) {
                    BitData childData = new BitData {RootPlaceholder = true};
                    bitData.Children.Add(childData);
                    bitData.SlotIds.Add(slot.Key);
                } else {
                    BitData childData = SaveBit(slot.Value);
                    bitData.Children.Add(childData);
                    bitData.SlotIds.Add(slot.Key);
                }
            } else {
                bitData.Children.Add(null); // Maintain the order of slots
                bitData.SlotIds.Add(slot.Key);
            }
        }

        return bitData;
    }

    public BitData LoadBit(string input) {
        input = Clean(input);
        string saveFilePath = Path.Combine(Application.persistentDataPath, input + ".json");

        if (File.Exists(saveFilePath)) {
            string json = File.ReadAllText(saveFilePath);
            BitData bitData = JsonConvert.DeserializeObject<BitData>(json);
            return bitData;
        } else {
            Debug.LogError("No saved ship configuration found.");
            return null;
        }
    }

    string Clean(string text) {
        if (string.IsNullOrEmpty(text)) {
            text = "_";
        }
        return text;
    }
}

