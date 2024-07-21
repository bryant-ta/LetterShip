using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SquareDrawer : MonoBehaviour {
    public float radius = 5f;
    public float lineWidth = 0.1f; 

    LineRenderer lineRenderer;

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 5;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        DrawSquare();
    }

    void DrawSquare() {
        float halfWidth = radius;

        Vector3[] corners = new Vector3[5];
        corners[0] = new Vector3(-halfWidth, -halfWidth, 0); // Bottom-left
        corners[1] = new Vector3(halfWidth, -halfWidth, 0);  // Bottom-right
        corners[2] = new Vector3(halfWidth, halfWidth, 0);   // Top-right
        corners[3] = new Vector3(-halfWidth, halfWidth, 0);  // Top-left
        corners[4] = corners[0];                             // Closing loop

        lineRenderer.SetPositions(corners);
    }

    void OnValidate() {
        if (lineRenderer != null) {
            DrawSquare();
        }
    }
}