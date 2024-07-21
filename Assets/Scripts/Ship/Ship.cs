using System;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {
    public Bit Core;

    Rigidbody2D rb;

    void Awake() { rb = GetComponent<Rigidbody2D>(); }

    void Update() {
        if (CompareTag("Enemy")) {
            Vector3 pos = transform.position;
            if (transform.position.x > World.Instance.maxX || transform.position.x < World.Instance.minX ||
                transform.position.y > World.Instance.maxY || transform.position.y < World.Instance.minY) {
                World.Instance.NumCurEnemies--;
                Destroy(gameObject);
            }
        }
    }

    public void ActivateLetter(char letter) {
        List<Frame> matchedBits = FindFrameByLetter(letter);
        foreach (Frame frame in matchedBits) {
            frame.Activate();
        }
    }

    public void DeactivateLetter(char letter) {
        List<Frame> matchedBits = FindFrameByLetter(letter);
        foreach (Frame frame in matchedBits) {
            frame.Deactivate();
        }
    }

    List<Frame> FindFrameByLetter(char letter) {
        if (Core == null) return null;

        List<Frame> matchedFrames = new();
        Queue<Bit> queue = new();
        queue.Enqueue(Core);

        while (queue.Count > 0) {
            Bit curBit = queue.Dequeue();

            Frame frame = curBit as Frame;
            if (frame != null && char.ToLower(frame.Letter) == char.ToLower(letter)) {
                matchedFrames.Add(frame);
            }

            foreach (Bit child in curBit.Children()) {
                queue.Enqueue(child);
            }
        }

        return matchedFrames;
    }

    public void ActivateAllSecondary() {
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).TryGetComponent(out Bit bit)) {
                if (bit is Weapon weapon) {
                    weapon.Activate();
                } else if (bit is Thruster thruster) {
                    thruster.Activate();
                }
            }
        }
    }

    public void DeactivateAll() {
        List<Bit> allShipBits = AllBits();
        foreach (Bit bit in allShipBits) {
            bit.Deactivate();
        }
    }

    public void UpdateMass() {
        List<Bit> allShipBits = AllBits();
        rb.mass = (float) allShipBits.Count / 4f;
    }

    public List<Bit> AllBits() {
        List<Bit> allBits = new();
        if (Core == null) return allBits;

        Queue<Bit> queue = new();
        queue.Enqueue(Core);

        while (queue.Count > 0) {
            Bit curBit = queue.Dequeue();
            allBits.Add(curBit);

            foreach (Bit child in curBit.Children()) {
                queue.Enqueue(child);
            }
        }

        return allBits;
    }
}