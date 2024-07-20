using System;
using System.Collections.Generic;
using UnityEngine;

public class Bit : MonoBehaviour {
    // Properties
    public int Weight;
    public int Hp;

    // Connections
    public Bit Root;
    public Dictionary<Collider2D, Bit> Slots = new();

    public void Init() {
        Collider2D[] cols = GetComponents<Collider2D>();
        foreach (Collider2D col in cols) {
            Slots[col] = null;
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

        Vector2Int localPos = Vector2Int.RoundToInt(transform.localPosition);
        Vector2Int rootPos = Vector2Int.RoundToInt(root.transform.localPosition);
        if (root.Slots[rootSlot] != null || rootSlot.offset + rootPos != localPos) {
            return false;
        }

        Collider2D attachSlot = null;
        foreach (KeyValuePair<Collider2D, Bit> slot in Slots) {
            if (slot.Value != null && slot.Key.offset + localPos == rootPos) {
                attachSlot = slot.Key;
            }
        }
        if (attachSlot == null) return false;

        // Do attach
        Root = root;
        Slots[attachSlot] = root;
        root.Slots[rootSlot] = this;

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

        foreach (KeyValuePair<Collider2D, Bit> slot in Root.Slots) {
            if (slot.Value == this) {
                Root.Slots[slot.Key] = null;
            }
        }

        foreach (KeyValuePair<Collider2D, Bit> slot in Slots) {
            if (slot.Value == Root) {
                Slots[slot.Key] = null;
            }
        }
        Root = null;
    }

    public List<Bit> Children() {
        List<Bit> children = new();
        foreach (KeyValuePair<Collider2D, Bit> slot in Slots) {
            if (slot.Value != Root && slot.Value != null) {
                children.Add(slot.Value);
            }
        }

        return children;
    }

    public Vector3 SlotPos(Collider2D col) { return (Vector3) col.offset + transform.localPosition; }
}