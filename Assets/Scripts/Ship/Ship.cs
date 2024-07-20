using System;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {
    public Bit Core;

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
            if (frame != null && frame.Letter == letter) {
                matchedFrames.Add(frame);
            }

            foreach (Bit child in curBit.Children()) {
                queue.Enqueue(child);
            }
        }

        return matchedFrames;
    }

    public void DeactivateAll() {
        List<Bit> allShipBits = AllBits();
        foreach (Bit bit in allShipBits) {
            bit.Deactivate();
        }
    }
    
    List<Bit> AllBits() {
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
