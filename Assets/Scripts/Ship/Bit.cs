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
    int MaxHp;

    // Connections
    public Bit Root;
    public Dictionary<int, Bit> Slots = new(); // int = index of matching collider in SlotCols

    public Collider2D BodyCol;
    public List<Collider2D> SlotCols = new(); // ULDR order

    SpriteRenderer sr;

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
        MaxHp = Hp;
    }

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
        if (rootSlotID == -1 || root.Slots[rootSlotID] != null || rootSlot.offset + rootPos != localPos) {
            return false;
        }

        // Type Check
        if (Type == BitType.Weapon || Type == BitType.Thruster) {
            if (root.Type != BitType.Frame || !IsValidSlotAttach(root.Id, root.SlotID(rootSlot))) {
                return false;
            }
        }
        
        // Overlap Check
        for (int i = 0; i < transform.parent.childCount; i++) {
            Collider2D col = Physics2D.OverlapBox(transform.parent.GetChild(i).position, new Vector2(0.1f, 0.1f), 0);
            if (col != null && !col.isTrigger && col.CompareTag("Player")) {
                return false;
            }
        }

        // Do attach
        Root = root;
        for (int i = 0; i < Slots.Count; i++) {
            if (Slots[i] == null && SlotCols[i].offset + localPos == rootPos) {
                Slots[i] = root;
                SlotCols[i].enabled = false;
                break;
            }
        }
        root.Slots[rootSlotID] = this;
        root.SlotCols[rootSlotID].enabled = false;

        // Change parent
        Transform oldParent = transform.parent;
        for (int i = oldParent.childCount - 1; i >= 0; i--) {
            oldParent.GetChild(i).tag = "Player";
            oldParent.GetChild(i).parent = root.transform.parent;
        }
        Destroy(oldParent.gameObject);
        
        // Update ship stats
        transform.parent.GetComponent<Ship>().UpdateMass();

        return true;
    }
    public virtual void Dettach() {
        if (Root == null) {
            return;
        }

        Transform oldParent = transform.parent;
        Transform newParent = Factory.Instance.CreateSalvage(transform.position).transform;
        newParent.transform.rotation = oldParent.rotation;
        transform.tag = "Untagged";
        transform.parent = newParent;
        foreach (Bit bit in Children()) {
            bit.tag = "Untagged";
            bit.transform.parent = newParent;
        }

        for (int i = 0; i < Root.Slots.Count; i++) {
            if (Root.Slots[i] == this) {
                Root.Slots[i] = null;
                Root.SlotCols[i].enabled = true;
            }
        }

        for (int i = 0; i < Slots.Count; i++) {
            if (Slots[i] == Root) {
                Slots[i] = null;
                SlotCols[i].enabled = true;
            }
        }
        Root = null;

        if (oldParent.TryGetComponent(out Ship s)) {
            s.UpdateMass();
        }
    }

    #region Damage

    public void DoDamage(int dmg) {
        Hp -= dmg;
        if (Hp <= 0) {
            // TODO: handle destroy and dettach
            Dettach();
            foreach (Bit bit in DirectChildren()) {
                bit.Dettach();
            }
            
            Deactivate();
            Destroy(transform.parent.gameObject);
        }

        Color c = sr.color;
        c.a = (float) Hp / MaxHp;
        sr.color = c;
    }

    #endregion

    #region Helper

    public List<Bit> Children() {
        List<Bit> children = new();
        foreach (var slot in Slots) {
            if (slot.Value != Root && slot.Value != null) {
                children.Add(slot.Value);
                List<Bit> c = slot.Value.Children();
                foreach (var bit in c) {
                    children.Add(bit);
                }
            }
        }

        return children;
    }

    public List<Bit> DirectChildren() {
        List<Bit> children = new();
        foreach (var slot in Slots) {
            if (slot.Value != Root && slot.Value != null) {
                children.Add(slot.Value);
            }
        }

        return children;
    }

    public Vector3 SlotPos(Collider2D col) { return transform.parent.TransformPoint((Vector3) col.offset + transform.localPosition); }

    public int SlotID(Collider2D col) { return SlotCols.IndexOf(col); }

    // encode Frame ID -> slotID is valid for Weapons, Thrusters
    public static bool IsValidSlotAttach(int frameID, int slotID) {
        switch (frameID) {
            case 0: // Core
            case 1: // URDL
                return true;
            case 2: // U
                return slotID == 0;
            case 3: // R
                return slotID == 1;
            case 4: // D
                return slotID == 2;
            case 5: // L
                return slotID == 3;
            case 6: // UD
                return slotID == 0 || slotID == 2;
            case 7: // RL
                return slotID == 3 || slotID == 1;
            case 8: // _
                return false;
        }
        Debug.LogError("Unhandled frameID.");
        return false;
    }

    public List<Bit> AllConnectedBits() {
        List<Bit> allBits = new();

        Bit curBit = this;
        int iter = 1000;
        while (curBit.Root != null) {
            curBit = Root;
            iter--;
            if (iter == 0) {
                Debug.LogError("infinite loop prob");
                break;
            }
        }

        Queue<Bit> queue = new();
        queue.Enqueue(curBit);

        while (queue.Count > 0) {
            curBit = queue.Dequeue();
            allBits.Add(curBit);

            foreach (Bit child in curBit.Children()) {
                queue.Enqueue(child);
            }
        }

        return allBits;
    }
    
    #endregion
}