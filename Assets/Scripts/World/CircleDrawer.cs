using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircleDrawer : MonoBehaviour {
    public int numberOfCircles = 3; 
    public float maxRadius = 5f;
    public float minRadius = 1f;  
    public float lineWidth = 0.1f; 

    public int minSegments = 36;  
    public int maxSegments = 360;

    LineRenderer lineRenderer;

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = (numberOfCircles * (maxSegments + 1)); // Total number of points for all circles

        // Set line width
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        CreateCircles();
    }

    void CreateCircles() {
        float segmentStep = (maxSegments - minSegments) / (float)(numberOfCircles - 1); // Step between min and max segments
        float radiusStep = (maxRadius - minRadius) / (numberOfCircles - 1);             // Step between min and max radius

        int currentPointIndex = 0;

        for (int i = 0; i < numberOfCircles; i++) {
            float radius = minRadius + i * radiusStep;
            int segments = Mathf.RoundToInt(minSegments + i * segmentStep);

            float angle = 0f;
            for (int j = 0; j < segments + 1; j++) {
                float x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
                float y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
                lineRenderer.SetPosition(currentPointIndex++, new Vector3(x, y, 0));
                angle += (360f / segments);
            }
        }
    }

    void OnValidate() {
        if (lineRenderer != null) {
            CreateCircles();
        }
    }
}