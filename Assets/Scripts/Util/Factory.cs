using System.Collections.Generic;
using UnityEngine;

public class Factory : Singleton<Factory> {
    [SerializeField] GameObject salvageBase;
    [SerializeField] GameObject shipBase;

    [SerializeField] List<GameObject> frames;
    [SerializeField] List<GameObject> weapons;
    [SerializeField] List<GameObject> thrusters;

    public GameObject CreateSalvage(Vector3 pos) {
        GameObject salvageObj = Instantiate(salvageBase, pos, Quaternion.identity);
        return salvageObj;
    }

    public Ship CreateShip(SO_Ship shipData, Vector3 pos) {
        Ship ship = Instantiate(salvageBase, pos, Quaternion.identity).GetComponent<Ship>();
        ship.shipData = shipData;
        return ship;
    }

    public Bit CreateFrame(int frameID, Vector3 pos) {
        Frame frame = Instantiate(frames[frameID], pos, Quaternion.identity).GetComponent<Frame>();
        frame.Init();
        return frame;
    }
}
