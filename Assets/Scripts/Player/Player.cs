using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] string startShipName;
    
    public PlayerInput Input { get; private set; }
    public Ship Ship { get; private set; }

    Bit heldBit;
    public bool IsHolding => heldBit != null;

    void Awake() {
        Input = GetComponent<PlayerInput>();

        Input.InputPrimaryDown += GrabBit;
        Input.InputPrimaryUp += ReleaseBit;
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

        Input.InputPoint += DragBit;
    }

    public void DragBit(ClickInputArgs clickInputArgs) {
        if (heldBit == null) return;
        
        // Snap to ship slots
        if (clickInputArgs.TargetObj != null && clickInputArgs.TargetCol.isTrigger) {
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

        Input.InputPoint -= DragBit;
    }

    // prob only call from ShipEditor
    public void TrashBit() {
        if (heldBit == null) return;
        
        Destroy(heldBit.gameObject);
        heldBit = null;
        
        Input.InputPoint -= DragBit;
    }

    public void SetShip(Ship ship) {
        if (Ship != null) {
            Input.InputKeyDown -= Ship.ActivateLetter;
            Input.InputKeyUp -= Ship.DeactivateLetter;
            Ship.DeactivateAll();
            Destroy(Ship.gameObject);
        }
        
        Ship = ship;

        Input.InputKeyDown += Ship.ActivateLetter;
        Input.InputKeyUp += Ship.DeactivateLetter;
    }
    
    
    
    

    // bool IsOverlappingShip() {
    //     Collider2D[] overlappingCol = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.1f, 0.1f), 0f);
    //     foreach (Collider2D col in overlappingCol)
    //     {
    //         if (!col.isTrigger && col.CompareTag("Player"))
    //         {
    //             Debug.Log("Overlapping collider: " + col.name);
    //             return false;
    //         }
    //     }
    //
    //     return true;
    // }
}