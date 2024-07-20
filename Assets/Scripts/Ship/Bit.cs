using System;
using System.Collections.Generic;
using UnityEngine;

public enum BitType {
    Bit = 0,
    Frame = 1,
    Weapon = 2,
    Thruster = 3,
}

public class Bit : MonoBehaviour {
    // Properties
    public int Id;
    public BitType Type;
    public int Weight;
    public int Hp;

    // Connections
    public Bit Root;
    public Dictionary<int, Bit> Slots = new(); // int = index of matching collider in SlotCols
    
    public Collider2D BodyCol;
    public List<Collider2D> SlotCols = new(); // ULDR order

    public void Init(int id, BitType type) {
        Id = id;
        Type = type;
        
        for (int i = 0; i < SlotCols.Count; i++) {
            Slots[i] = null;
        }
    }

    public virtual void Activate() { }
    public virtual void Deactivate() { }

    // rootSlot: the slot on root that will contain this bit
    public virtual bool Attach(Bit root, Collider2D rootSlot) {
        // Slot Checks
        if (Root != null) {
            Debug.LogError("Already attached to something");
            return false;
        }

        int rootSlotID = root.SlotID(rootSlot);

        Vector2 localP = transform.parent.TransformPoint(transform.localPosition);
        Vector2Int localPos = Vector2Int.RoundToInt(root.transform.parent.InverseTransformPoint(localP));
        Vector2Int rootPos = Vector2Int.RoundToInt(root.transform.localPosition);
        if (root.Slots[rootSlotID] != null || rootSlot.offset + rootPos != localPos) {
            return false;
        }

        // Do attach
        Root = root;
        for (int i = 0; i < Slots.Count; i++) {
            if (Slots[i] == null && SlotCols[i].offset + localPos == rootPos) {
                Slots[i] = root;
                break;
            }
        }
        root.Slots[rootSlotID] = this;

        Transform oldParent = transform.parent;
        for (int i = 0; i < oldParent.childCount; i++) {
            oldParent.GetChild(i).parent = root.transform.parent;
        }
        Destroy(oldParent.gameObject);

        return true;
    }
    public virtual void Dettach() {
        if (Root == null) {
            Debug.LogError("Not attached to anything.");
            return;
        }
        
        Transform newParent = Factory.Instance.CreateSalvage(transform.position).transform;
        foreach (Bit bit in Children()) {
            bit.transform.parent = newParent;
        }

        foreach (var slot in Root.Slots) {
            if (slot.Value == this) {
                Root.Slots[slot.Key] = null;
            }
        }

        foreach (var slot in Slots) {
            if (slot.Value == Root) {
                Slots[slot.Key] = null;
            }
        }
        Root = null;
    }

    public List<Bit> Children() {
        List<Bit> children = new();
        foreach (var slot in Slots) {
            if (slot.Value != Root && slot.Value != null) {
                children.Add(slot.Value);
            }
        }

        return children;
    }

    public Vector3 SlotPos(Collider2D col) {
        return transform.parent.TransformPoint((Vector3)col.offset + transform.localPosition);
    }

    public int SlotID(Collider2D col) {
        return SlotCols.IndexOf(col);
    }
}