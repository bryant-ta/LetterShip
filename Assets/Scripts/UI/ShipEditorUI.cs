using System;
using UnityEngine;

public class ShipEditorUI : MonoBehaviour {
    void Awake() {
        Ship ship = Factory.Instance.CreateBaseShip(Vector3.zero);
        Ref.Player.SetShip(ship);
    }

    public void SpawnFrame(int frameID) {
        Frame frame = Factory.Instance.CreateFrame(0, Vector3.zero);
        GameObject salvageObj = Factory.Instance.CreateSalvage(Vector3.zero);
        frame.transform.parent = salvageObj.transform;

        ClickInputArgs clickInputArgs = new ClickInputArgs {
            CursorPos = Input.mousePosition,
            TargetCol = frame.BodyCol,
            TargetObj = frame.gameObject
        };

        Ref.Player.GrabBit(clickInputArgs);
    }
}