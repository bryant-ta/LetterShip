using System;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour {
    public SO_Ship shipData;
    
    Bit core;

    public void Awake() {
        core = shipData.Core;
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
        if (core == null) return null;

        List<Frame> matchedFrames = new();
        Queue<Bit> queue = new();
        queue.Enqueue(core);

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
}
