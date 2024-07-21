using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircleDrawer : MonoBehaviour {
    [SerializeField] int segments = 100;
    [SerializeField] float radius = 10f;
    [SerializeField] float lineWidth = 0.2f;

    LineRenderer lineRenderer;

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        DrawCircle();
    }

    void DrawCircle() {
        lineRenderer.widthMultiplier = lineWidth;
        lineRenderer.positionCount = segments + 1;

        float angle = 2 * Mathf.PI / segments;

        for (int i = 0; i < segments + 1; i++) {
            float x = Mathf.Cos(i * angle) * radius;
            float y = Mathf.Sin(i * angle) * radius;
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
        }
    }
}