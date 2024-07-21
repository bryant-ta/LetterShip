using System.Collections.Generic;
using UnityEngine;

public class BitConnectionRenderer : MonoBehaviour {
    [SerializeField] Material mat;
    
    List<GameObject> lineObjects = new();

    public void Render(Bit bit) {
        Clear();

        var allBits = bit.AllConnectedBits();
        
        foreach (var currentBit in allBits) {
            foreach (var slot in currentBit.Slots) {
                var connectedBit = slot.Value;
                if (connectedBit != null) {
                    GameObject lineObj = new GameObject("Line");
                    LineRenderer lineRenderer = lineObj.AddComponent<LineRenderer>();

                    lineRenderer.startWidth = 0.1f;
                    lineRenderer.endWidth = 0.1f;
                    lineRenderer.positionCount = 2;
                    lineRenderer.useWorldSpace = false;
                    
                    lineRenderer.startColor = Color.cyan;
                    lineRenderer.endColor = Color.cyan;
                    lineRenderer.material = mat;

                    lineRenderer.sortingOrder = -10;

                    lineRenderer.SetPosition(0, currentBit.transform.localPosition);
                    lineRenderer.SetPosition(1, connectedBit.transform.localPosition);
                    
                    lineObj.transform.SetParent(transform, false);

                    lineObjects.Add(lineObj);
                }
            }
        }
    }

    public void Clear() {
        foreach (var lineObj in lineObjects) {
            if (lineObj != null) {
                Destroy(lineObj);
            }
        }
        lineObjects.Clear();
    }
}