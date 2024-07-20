using System.Collections.Generic;
using UnityEngine;

public class Factory : Singleton<Factory> {
    [SerializeField] GameObject salvageBase;
    [SerializeField] GameObject shipBase;

    [SerializeField] List<GameObject> frames;
    [SerializeField] List<GameObject> weapons;
    [SerializeField] List<GameObject> thrusters;

    ShipSerializer shipSerializer;

    public GameObject CreateSalvage(Vector3 pos) {
        GameObject salvageObj = Instantiate(salvageBase, pos, Quaternion.identity);
        return salvageObj;
    }

    public Ship CreateShip(SO_Ship shipData, Vector3 pos) {
        // Instantiate according to shipData
        
        
        return null;
    }
    
    public Ship CreateShip(string shipName, Vector3 pos) {
        Ship ship = Instantiate(shipBase, pos, Quaternion.identity).GetComponent<Ship>();
        
        BitData bitData = shipSerializer.LoadBit(shipName);
        if (bitData == null) return null;
        
        // TODO: generate this
        CreateBitFromBitData(bitData, null, ship.transform);
        
        return ship;
    }

    Bit CreateBitFromBitData(BitData bitData, Bit lastBit, Transform shipTrs) {
        if (bitData == null) return null;

        Bit bit = CreateBit(bitData.Id, bitData.Type, shipTrs.position);
        bit.Root = lastBit;
        bit.transform.SetParent(shipTrs);

        for (int i = 0; i < bitData.Children.Count; i++) {
            if (bitData.Children[i] != null) {
                Bit childBit = CreateBitFromBitData(bitData.Children[i], bit, shipTrs);
                bit.Slots[bitData.slotIds[i]] = childBit;
            }
            // null Slot already set in Bit.Init
        }

        return bit;
    }

    public Bit CreateBit(int id, BitType type, Vector3 pos) {
        switch (type) {
            case BitType.Frame:
                return CreateFrame(id, pos);
            case BitType.Weapon:
                return CreateWeapon(id, pos);
            case BitType.Thruster:
                return CreateThruster(id, pos);
            default:
                Debug.LogError("Bit type is unaccepted.");
                return null;
        }
    }

    public Frame CreateFrame(int id, Vector3 pos) {
        Frame frame = Instantiate(frames[id], pos, Quaternion.identity).GetComponent<Frame>();
        frame.Init(id, BitType.Frame);
        return frame;
    }
    
    public Weapon CreateWeapon(int id, Vector3 pos) {
        Weapon weapon = Instantiate(weapons[id], pos, Quaternion.identity).GetComponent<Weapon>();
        weapon.Init(id, BitType.Weapon);
        return weapon;
    }

    public Thruster CreateThruster(int id, Vector3 pos) {
        Thruster thruster = Instantiate(thrusters[id], pos, Quaternion.identity).GetComponent<Thruster>();
        thruster.Init(id, BitType.Thruster);
        return thruster;
    }
}
