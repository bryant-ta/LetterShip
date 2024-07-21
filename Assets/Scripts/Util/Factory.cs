using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Factory : Singleton<Factory> {
    [SerializeField] GameObject salvageBase;
    [SerializeField] GameObject shipBase;

    [SerializeField] GameObject simpleShip;

    [SerializeField] List<GameObject> frames;
    [SerializeField] List<GameObject> weapons;
    [SerializeField] List<GameObject> thrusters;

    ShipSerializer shipSerializer;

    void Awake() { shipSerializer = new ShipSerializer(); }

    public GameObject CreateSalvage(Vector3 pos) {
        GameObject salvageObj = Instantiate(salvageBase, pos, Quaternion.identity);
        return salvageObj;
    }

    public Ship CreateBaseShip(Vector3 pos) {
        Ship ship = Instantiate(simpleShip, pos, Quaternion.identity).GetComponent<Ship>();
        ship.Core = ship.GetComponentInChildren<Bit>();
        ship.Core.Init(ship.Core.Id, ship.Core.Type);
        return ship;
    }

    public Ship CreateShip(string shipName, Vector3 pos) {
        Ship ship = Instantiate(shipBase, pos, Quaternion.identity).GetComponent<Ship>();

        BitData bitData = shipSerializer.LoadBit(shipName);
        if (bitData == null) return null;

        ship.Core = CreateBitFromBitData(bitData, null, ship.transform);
        

        return ship;
    }

    Bit CreateBitFromBitData(BitData bitData, Bit lastBit, Transform shipTrs) {
        if (bitData == null) return null;

        Bit bit = CreateBit(bitData.Id, bitData.Type, shipTrs.position);
        bit.Root = lastBit;
        bit.transform.SetParent(shipTrs);
        bit.transform.localPosition = new Vector3(bitData.Position.x, bitData.Position.y, bitData.Position.z);

        for (int i = 0; i < bitData.Children.Count; i++) {
            if (bitData.Children[i] != null) {
                if (bitData.Children[i].RootPlaceholder) {
                    bit.Slots[i] = bit.Root;
                    bit.SlotCols[i].enabled = false;
                } else {
                    Bit childBit = CreateBitFromBitData(bitData.Children[i], bit, shipTrs);
                    bit.Slots[i] = childBit;
                    bit.SlotCols[i].enabled = false;
                }
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

        char c = '_';
        if (id != 8) { // frame_ (no attachments)
            int randomIndex = Random.Range(0, 26);
            c = (char) ('A' + randomIndex);
        }
        
        frame.Init(id, BitType.Frame, c);
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

    #region ShipSerializer

    public void SaveShip(string input) { shipSerializer.SaveShip(input); }

    // Used by ShipEditor Load Button
    public void LoadShip(string input) {
        Ship ship = CreateShip(input, Vector3.zero);
        Ref.Player.SetShip(ship);
    }

    #endregion
}