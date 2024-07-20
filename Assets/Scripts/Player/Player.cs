using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] string startShipName;
    
    PlayerInput input;
    public Ship Ship { get; private set; }

    Bit heldBit;
    public bool IsHolding => heldBit != null;

    void Awake() {
        input = GetComponent<PlayerInput>();

        input.InputPrimaryDown += GrabBit;
        input.InputPrimaryUp += ReleaseBit;
    }

    public void GrabBit(ClickInputArgs clickInputArgs) {
        if (heldBit != null) return;
        
        Bit b = clickInputArgs.TargetObj.GetComponent<Bit>();
        if (b == null) return;

        heldBit = b;
        Collider2D[] cols = heldBit.GetComponents<Collider2D>();
        foreach (Collider2D col in cols) {
            col.enabled = false;
        }

        input.InputPoint += DragBit;
    }

    public void DragBit(ClickInputArgs clickInputArgs) {
        if (heldBit == null) return;
        
        // Snap to ship slots
        if (clickInputArgs.TargetObj != null) {
            if (clickInputArgs.TargetObj.TryGetComponent(out Bit b) && b.transform.parent.TryGetComponent(out Ship s)) {
                heldBit.transform.parent.position = b.SlotPos(clickInputArgs.TargetCol);
                heldBit.transform.parent.rotation = clickInputArgs.TargetObj.transform.rotation;
                return;
            }
        }
        
        heldBit.transform.parent.position = clickInputArgs.CursorPos;
    }

    public void ReleaseBit(ClickInputArgs clickInputArgs) {
        if (heldBit == null) return;
        
        Bit b = clickInputArgs.TargetObj.GetComponent<Bit>();
        if (b == null) return;
        Ship s = b.transform.parent.GetComponent<Ship>();
        if (s != Ship) return;

        Collider2D rootSlot = clickInputArgs.TargetCol;
        Bit root = b;

        if (!heldBit.Attach(root, rootSlot)) {
            return;
        }

        Collider2D[] cols = heldBit.GetComponents<Collider2D>();
        foreach (Collider2D col in cols) {
            col.enabled = true;
        }

        heldBit = null;

        input.InputPoint -= DragBit;
    }

    public void SetShip(Ship ship) {
        if (Ship != null) {
            input.InputKeyDown -= Ship.ActivateLetter;
            input.InputKeyUp -= Ship.DeactivateLetter;
            Ship.DeactivateAll();
            Destroy(Ship.gameObject);
        }
        
        Ship = ship;

        input.InputKeyDown += Ship.ActivateLetter;
        input.InputKeyUp += Ship.DeactivateLetter;
    }
}