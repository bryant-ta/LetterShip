using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour {
    public PlayerInput PlayerInput { get; private set; }
    public Ship Ship { get; private set; }
    
    BitConnectionRenderer bitConnRenderer;
    bool showBitConns;

    Bit heldBit;
    List<Bit> heldBits = new();
    public bool IsHolding => heldBit != null;

    void Awake() {
        PlayerInput = GetComponent<PlayerInput>();
        bitConnRenderer = GetComponent<BitConnectionRenderer>();

        PlayerInput.InputPrimaryDown += GrabBit;
        PlayerInput.InputPrimaryUp += ReleaseBit;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.LeftShift)) bitConnRenderer.Render(Ship.Core);
        if (Input.GetKeyUp(KeyCode.LeftShift)) bitConnRenderer.Clear();
        
        
    }

    public void GrabBit(ClickInputArgs clickInputArgs) {
        if (heldBit != null) return;
        if (clickInputArgs.TargetObj == null) return;

        if (!clickInputArgs.TargetObj.TryGetComponent(out Bit targetBit)) return;
        if (targetBit.transform.parent.CompareTag("Enemy") || targetBit.name == "Frame_Core") return;

        if (clickInputArgs.TargetObj.CompareTag("Shop")) {
            if (!Shop.Instance.Buy(targetBit)) {
                return;
            }
        }
        
        heldBit = targetBit;

        heldBit.Dettach();

        heldBits = heldBit.AllConnectedBits();
        foreach (var bit in heldBits) {
            bit.BodyCol.enabled = false;
        }
        
        heldBit.transform.parent.rotation = Quaternion.identity;

        PlayerInput.InputPoint += DragBit;
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

        if (EventSystem.current.IsPointerOverGameObject() && CheckUIIgnoreTag()) {
            return;
        }

        // Drop bit in space
        if (clickInputArgs.TargetObj == null) {
            foreach (var bit in heldBits) {
                bit.BodyCol.enabled = true;
            }

            ReleaseTasks();
            return;
        }

        if (!clickInputArgs.TargetObj.TryGetComponent(out Bit targetBit)) return;
        if (!targetBit.transform.parent.TryGetComponent(out Ship targetShip)) return;
        if (targetShip != Ship) return;

        Collider2D rootSlot = clickInputArgs.TargetCol;

        if (!heldBit.Attach(targetBit, rootSlot)) return;

        foreach (var bit in heldBits) {
            bit.BodyCol.enabled = true;
        }

        ReleaseTasks();
    }
    void ReleaseTasks() {
        heldBit = null;
        heldBits.Clear();

        PlayerInput.InputPoint -= DragBit;
    }

    // prob only call from ShipEditor
    public void TrashBit() {
        if (heldBit == null) return;

        Destroy(heldBit.transform.parent.gameObject);

        ReleaseTasks();
    }

    public void SetShip(Ship ship) {
        if (Ship != null) {
            PlayerInput.InputKeyDown -= Ship.ActivateLetter;
            PlayerInput.InputKeyUp -= Ship.DeactivateLetter;
            Ship.DeactivateAll();
            Destroy(Ship.gameObject);
        }

        Ship = ship;
        Ship.UpdateMass();
        List<Bit> allShipBits = Ship.AllBits();
        foreach (Bit bit in allShipBits) {
            bit.tag = "Player";
        }
        
        GetComponent<FollowTarget>().SetTarget(Ship.gameObject);

        PlayerInput.InputKeyDown += Ship.ActivateLetter;
        PlayerInput.InputKeyUp += Ship.DeactivateLetter;
    }

    public bool CheckUIIgnoreTag() {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        foreach (RaycastResult result in raycastResults) {
            GameObject uiElement = result.gameObject;
            if (uiElement.CompareTag("UIIgnore")) {
                return true;
            }
        }
        return false;
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