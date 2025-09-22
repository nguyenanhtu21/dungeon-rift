using UnityEngine;
using UnityEngine.TextCore.Text;

public class CameraFollow : MonoBehaviour
{
    public Transform target;   
    public float smoothSpeed = 5f;
    public Vector3 offset;

    [Header("Map Bounds")]
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private float camHalfHeight;
    private float camHalfWidth;

    void Start()
    {
        Camera cam = Camera.main;
        camHalfHeight = cam.orthographicSize;
        camHalfWidth = camHalfHeight * cam.aspect;
        target = FindAnyObjectByType<CharacterMovement>().transform;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            float clampedX = Mathf.Clamp(smoothedPosition.x, minBounds.x + camHalfWidth, maxBounds.x - camHalfWidth);
            float clampedY = Mathf.Clamp(smoothedPosition.y, minBounds.y + camHalfHeight, maxBounds.y - camHalfHeight);

            transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        }
    }
}
