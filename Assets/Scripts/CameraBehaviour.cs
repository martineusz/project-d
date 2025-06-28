using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;            // Player's transform
    public float smoothSpeed = 0.125f;  // Smoothing factor
    public Vector3 offset;              // Offset from player

    public Vector2 minBounds;           // Min X and Y clamp
    public Vector2 maxBounds;           // Max X and Y clamp

    private void LateUpdate()
    {
        if (!target) return;

        Vector3 desiredPosition = target.position + offset;

        // Clamp the camera position
        float clampedX = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y);

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, new Vector3(clampedX, clampedY, desiredPosition.z), smoothSpeed);
        transform.position = smoothedPosition;
    }
}