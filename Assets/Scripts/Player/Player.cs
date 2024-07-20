using System;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] SO_Ship starterShip;
    
    PlayerInput input;
    Ship ship;

    Bit heldBit;

    void Awake() {
        input = GetComponent<PlayerInput>();
        ship = Factory.Instance.CreateShip(starterShip, transform.position);

        input.InputKeyDown += ship.ActivateLetter;
        input.InputKeyUp += ship.DeactivateLetter;

        input.InputPrimaryDown += GrabBit;
        input.InputPrimaryUp += ReleaseBit;
    }

    public void GrabBit(ClickInputArgs clickInputArgs) {
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
        // Snap to ship slots
        if (clickInputArgs.TargetObj != null) {
            if (clickInputArgs.TargetObj.TryGetComponent(out Bit b) && b.transform.parent.TryGetComponent(out Ship s)) {
                heldBit.transform.position = b.SlotPos(clickInputArgs.TargetCol);
                return;
            }
        }
        
        heldBit.transform.position = clickInputArgs.CursorPos;
    }

    public void ReleaseBit(ClickInputArgs clickInputArgs) {
        Bit b = clickInputArgs.TargetObj.GetComponent<Bit>();
        if (b == null) return;
        Ship s = b.transform.parent.GetComponent<Ship>();
        if (s != ship) return;

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
}