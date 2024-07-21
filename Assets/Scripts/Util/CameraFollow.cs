using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField] Transform target;        // Target object for the camera to follow
    [SerializeField] float smoothTime = 0.3f; // Smoothing factor for camera movement

    Vector3 velocity = Vector3.zero; // Velocity used by SmoothDamp

    void LateUpdate() {
        if (target == null) return;

        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        transform.position = smoothedPosition;
    }
}